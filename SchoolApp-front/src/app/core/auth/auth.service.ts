import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { AuthUtils } from 'app/core/auth/auth.utils';
import { AppConfig } from 'config/model/app.config.model';
import { catchError, Observable, of, switchMap, throwError } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AuthService {
    private _authenticated: boolean = false;
    private _httpClient = inject(HttpClient);
    private _appConfig = inject(AppConfig);
    // -----------------------------------------------------------------------------------------------------
    // @ Accessors
    // -----------------------------------------------------------------------------------------------------

    /**
     * Setter & getter for access token
     */
    set accessToken(token: string) {
        localStorage.setItem('accessToken', token);
    }

    get accessToken(): string {
        return localStorage.getItem('accessToken') ?? '';
    }

    set refreshToken(token: string) {
        localStorage.setItem('refreshToken', token);
    }

    get refreshToken(): string {
        return localStorage.getItem('refreshToken') ?? '';
    }
    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Forgot password
     *
     * @param email
     */
    forgotPassword(email: string): Observable<any> {
        const resetRequest = {
            client_id: this._appConfig.auth0.client.client_id,
            email: email,
            connection: 'Username-Password-Authentication',
        };
        const resetUrl = `https://${this._appConfig.auth0.domain}/dbconnections/change_password`;

        return this._httpClient.post(resetUrl, resetRequest, {
            responseType: 'text',
        });
    }

    /**
     * Reset password
     *
     * @param password
     */
    resetPassword(password: string): Observable<any> {
        return;
        //  return this._httpClient.post(`${this._appConfig.ApiURL}/api/auth/reset-password`, password);
    }

    /**
     * Sign in
     *
     * @param credentials
     */
    signIn(credentials: { email: string; password: string }): Observable<any> {
        if (this._authenticated) {
            return throwError(() => new Error('User is already logged in.'));
        }

        const auth0Token = {
            username: credentials.email,
            password: credentials.password,
            client_id: this._appConfig.auth0.client.client_id,
            client_secret: this._appConfig.auth0.client.client_secret,
            audience: this._appConfig.auth0.audience,
            grant_type: 'password',
            scope: 'offline_access',
        };

        return this._httpClient
            .post(
                `https://${this._appConfig.auth0.domain}/oauth/token`,
                auth0Token
            )
            .pipe(
                switchMap((response: any) => {
                    this.accessToken = response.access_token;
                    this.refreshToken = response.refresh_token;
                    this._authenticated = true;
                    return of(response);
                })
            );
    }

    /**
     * Sign in using the access token
     */
    signInUsingToken(): Observable<any> {
        const auth0RefreshToken = {
            client_id: this._appConfig.auth0.client.client_id,
            client_secret: this._appConfig.auth0.client.client_secret,
            grant_type: 'refresh_token',
            refresh_token: this.refreshToken,
        };

        if (!AuthUtils.isTokenExpired(this.accessToken)) {
            return of(true);
        }

        return this._httpClient
            .post(
                `https://${this._appConfig.auth0.domain}/oauth/token`,
                auth0RefreshToken
            )
            .pipe(
                catchError(() => {
                    this.accessToken = '';
                    this.refreshToken = '';
                    this._authenticated = false;

                    return of(false);
                }),
                switchMap((response: any) => {
                    this.accessToken = response.access_token;
                    this.refreshToken = this.refreshToken;
                    this._authenticated = true;

                    return of(true);
                })
            );
    }

    /**
     * Sign out
     */
    signOut(): Observable<any> {
        // Remove the access token from the local storage
        localStorage.removeItem('accessToken');
        localStorage.removeItem('refreshToken');

        // Set the authenticated flag to false
        this._authenticated = false;

        // Return the observable
        return of(true);
    }

    /**
     * Sign up
     *
     * @param user
     */
    signUp(user: {
        name: string;
        email: string;
        password: string;
        company: string;
    }): Observable<any> {
        return this._httpClient.post('api/auth/sign-up', user);
    }

    /**
     * Unlock session
     *
     * @param credentials
     */
    unlockSession(credentials: {
        email: string;
        password: string;
    }): Observable<any> {
        return this._httpClient.post('api/auth/unlock-session', credentials);
    }

    /**
     * Check the authentication status
     */
    check(): Observable<boolean> {
        // Check if the user is logged in
        if (this._authenticated) {
            return of(true);
        }

        // Check the access token availability
        if (
            !this.accessToken ||
            this.accessToken === '' ||
            this.accessToken == undefined ||
            this.accessToken === 'undefined'
        ) {
            return of(false);
        }

        // Check the access token expire date
        if (AuthUtils.isTokenExpired(this.accessToken)) {
            return of(false);
        }

        // If the access token exists and it didn't expire, sign in using it
        return this.signInUsingToken();
    }
}
