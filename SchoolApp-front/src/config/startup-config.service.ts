import { DatePipe } from '@angular/common';
import { Injectable } from '@angular/core';
import { environment } from 'environments/environment';
import { AppConfig, Auth0Client, Auth0Config } from './model/app.config.model';
import { NAVBAR } from './nav-bar.config';

@Injectable({
    providedIn: 'root',
})
export class StartupConfigService extends AppConfig {
    constructor(private datePipe: DatePipe) {
        super();
    }

    public static config(): AppConfig {
        return Object.assign(new AppConfig(), this);
    }

    public async load$(): Promise<void> {
        this.Populate();
        return Promise.resolve();
    }

    public Populate(): void {
        this.apiUrl = environment.apiUrl;
        this.navBar = NAVBAR;
        this.auth0 = new Auth0Config();
        this.auth0.domain = environment.Auth0.Domain;
        this.auth0.audience = environment.Auth0.Audience;

        this.auth0.client = new Auth0Client();
        this.auth0.client.client_id = environment.Auth0.ClientId;
        this.auth0.client.client_secret = environment.Auth0.ClientSecret;
    }

    public async logProblem(
        source: string,
        action: string = null,
        message: string = null,
        data: any = null,
        isError: boolean = false
    ): Promise<any> {
        const LogDateFormat: string = 'hh:mm:ss.sss';
        const IsDebugActive: boolean =
            localStorage.getItem('debug-active') == 'true';

        if (!IsDebugActive) {
            return Promise.resolve();
        }
        const time = this.datePipe.transform(new Date(), LogDateFormat);
        const color: string = isError ? '#e03232' : '#f76901';
        let logmsg: string = '';

        logmsg += `%c[LOG]-> [${time}][${source}]`;
        if (action && action != null) {
            logmsg += `[${action}]`;
        }
        if (message && message != null) {
            logmsg += ` ${message}`;
        }

        if (data != null) {
            console.groupCollapsed(`${logmsg}`, `color: ${color};`);
            console.groupEnd();
        } else {
            console.info(`${logmsg}`, `color: ${color};`);
        }

        return Promise.resolve();
    }
}
