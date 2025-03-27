import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import {
    CreateUserInput,
    CreateUserOutput,
    ProfileType,
    UpdatePasswordInput,
    UpdateUserInput,
    User,
    UserQueryResult,
    UserQueryResultForList,
} from 'app/core/user/user.types';
import { FireBaseStorageService } from 'app/services/fireBaseStorage.service';
import {
    ApiResponseBaseModel,
    AppConfig,
    Pagination,
} from 'config/model/app.config.model';
import { NgxPermissionsService } from 'ngx-permissions';
import { Observable, ReplaySubject, lastValueFrom, map, tap } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class UserService {
    private _httpClient = inject(HttpClient);
    private _appConfig = inject(AppConfig);
    private _user: ReplaySubject<User> = new ReplaySubject<User>(1);
    private _permissionService = inject(NgxPermissionsService);
    private _fireBaseStorageService = inject(FireBaseStorageService);
    // -----------------------------------------------------------------------------------------------------
    // @ Accessors
    // -----------------------------------------------------------------------------------------------------

    /**
     * Setter & getter for user
     *
     * @param value
     */
    set user(value: User) {
        // Store the value
        this._user.next(value);
    }

    get user$(): Observable<User> {
        return this._user.asObservable();
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Get the current signed-in user data
     */
    get(): Observable<ApiResponseBaseModel<User>> {
        return this._httpClient
            .get<
                ApiResponseBaseModel<User>
            >(`${this._appConfig.apiUrl}/user/profile`)
            .pipe(
                tap((user) => {
                    this._permissionService.loadPermissions([
                        ProfileType[user.data.profileType],
                    ]);
                    this._user.next(user.data);
                })
            );
    }

    getUserById(id: string) {
        return lastValueFrom(
            this._httpClient.get<ApiResponseBaseModel<UserQueryResult>>(
                `${this._appConfig.apiUrl}/user/${id}`
            )
        );
    }

    getList(filterText = '', pageNumber = 1, pageSize = 15) {
        const requestParams = new HttpParams()
            .append('filter', filterText)
            .append('PageSize', pageSize)
            .append('PageNumber', pageNumber);
        return lastValueFrom(
            this._httpClient.get<
                ApiResponseBaseModel<Pagination<UserQueryResultForList>>
            >(`${this._appConfig.apiUrl}/user`, {
                params: requestParams,
            })
        );
    }

    updatePassword(id: string, password: string) {
        return lastValueFrom(
            this._httpClient.put<ApiResponseBaseModel<any>>(
                `${this._appConfig.apiUrl}/user/update-password`,
                new UpdatePasswordInput(id, password)
            )
        );
    }

    create(user: CreateUserInput) {
        return lastValueFrom(
            this._httpClient.post<ApiResponseBaseModel<CreateUserOutput>>(
                `${this._appConfig.apiUrl}/user`,
                user
            )
        );
    }

    delete(id: string) {
        return lastValueFrom(
            this._httpClient.delete<ApiResponseBaseModel<any>>(
                `${this._appConfig.apiUrl}/user/${id}`
            )
        );
    }

    /**
     * Update the user
     *
     * @param user
     */
    updateSelf(user: UpdateUserInput): Observable<ApiResponseBaseModel<any>> {
        return this._httpClient
            .put<
                ApiResponseBaseModel<any>
            >(`${this._appConfig.apiUrl}/user/${user.id}`, user)
            .pipe(
                map((response) => {
                    this._user.next({
                        id: user.id,
                        fullName: user.fullName,
                        email: user.email,
                        profileType: user.profileType,
                        avatar: user.avatar,
                        isActive: true,
                    });
                    return response;
                })
            );
    }

    update(user: UpdateUserInput) {
        return this._httpClient.put<ApiResponseBaseModel<any>>(
            `${this._appConfig.apiUrl}/user/${user.id}`,
            user
        );
    }

    async uploadAvatar(file: File, user: User | UserQueryResult) {
        const meta = await this._fireBaseStorageService.uploadFile(
            file,
            `user/${user.id}`,
            'avatar'
        );

        const link = await this._fireBaseStorageService.getLinkFile(meta);

        return link;
    }
}
