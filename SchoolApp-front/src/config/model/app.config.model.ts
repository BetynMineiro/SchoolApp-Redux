import { Menu } from '@fuse/components/navigation/menu.model';
import { IndividualConfig } from 'ngx-toastr';

export class AppConfig {
    apiUrl: string;
    navBar: Menu[];
    auth0: Auth0Config;
    public Populate(data: Partial<AppConfig>): void {
        Object.assign(this, data);
    }
}

export class Auth0Client {
    client_id: string;
    client_secret: string;
}

export class Auth0Config {
    domain: string;
    audience: string;
    client: Auth0Client;
}

export interface ApiResponseBaseModel<T> {
    data?: T;
    messages?: string[];
    success?: boolean;
}

export interface Pagination<T> {
    items?: T[];
    totalItems: number;
    pageNumber: number;
    pageSize: number;
    totalPages: number;
}

export const ToastConfig = {
    progressBar: true,
    timeOut: 2000,
    tapToDismiss: true,
    newestOnTop: true,
    progressAnimation: 'increasing',
} as IndividualConfig;
