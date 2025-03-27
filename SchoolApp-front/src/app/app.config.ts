import { DatePipe } from '@angular/common';
import { HttpClient, provideHttpClient } from '@angular/common/http';
import {
    APP_INITIALIZER,
    ApplicationConfig,
    LOCALE_ID,
    importProvidersFrom,
} from '@angular/core';
import { provideFirebaseApp } from '@angular/fire/app';

import { getStorage, provideStorage } from '@angular/fire/storage';
import { LuxonDateAdapter } from '@angular/material-luxon-adapter';
import {
    DateAdapter,
    MAT_DATE_FORMATS,
    MAT_DATE_LOCALE,
} from '@angular/material/core';
import { MatPaginatorIntl } from '@angular/material/paginator';
import { provideAnimations } from '@angular/platform-browser/animations';
import {
    PreloadAllModules,
    provideRouter,
    withInMemoryScrolling,
    withPreloading,
} from '@angular/router';
import { provideFuse } from '@fuse';
import { FuseConfirmationConfig } from '@fuse/services/confirmation';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { appRoutes } from 'app/app.routes';
import { provideAuth } from 'app/core/auth/auth.provider';
import { provideIcons } from 'app/core/icons/icons.provider';
import { AppConfig } from 'config/model/app.config.model';
import { StartupConfigService } from 'config/startup-config.service';
import { environment } from 'environments/environment';
import { initializeApp } from 'firebase/app';
import { NgxPermissionsModule } from 'ngx-permissions';
import { QuillConfigModule, QuillModule } from 'ngx-quill';
import { provideToastr } from 'ngx-toastr';
import { UserEffects } from './core/user/store/user.effects';
import { userReducer } from './core/user/store/user.reducer';

export const defaultToolbar = [
    ['bold', 'italic', 'underline', 'strike'],
    [{ list: 'ordered' }, { list: 'bullet' }],
    [{ align: [] }],
    [{ color: [] }, { background: [] }],
    ['clean'],
];

const rangeLabel = (page: number, pageSize: number, length: number) => {
    if (length == 0 || pageSize == 0) {
        return `0 to ${length}`;
    }

    length = Math.max(length, 0);

    const startIndex = page * pageSize;

    const endIndex =
        startIndex < length
            ? Math.min(startIndex + pageSize, length)
            : startIndex + pageSize;

    return `${startIndex + 1} - ${endIndex} of ${length}`;
};

function getPaginatorIntl() {
    const paginatorIntl = new MatPaginatorIntl();

    paginatorIntl.itemsPerPageLabel = 'Rows per page:';
    paginatorIntl.nextPageLabel = 'Next page';
    paginatorIntl.previousPageLabel = 'Previous page';
    paginatorIntl.firstPageLabel = 'First page';
    paginatorIntl.lastPageLabel = 'Last page';

    paginatorIntl.getRangeLabel = rangeLabel;

    return paginatorIntl;
}

function StartupConfigFactory$(StartupConfigService: StartupConfigService) {
    return () => {
        return StartupConfigService.load$();
    };
}

export const appConfig: ApplicationConfig = {
    providers: [
        DatePipe,
        { provide: LOCALE_ID, useValue: 'en-US' },
        { provide: MAT_DATE_LOCALE, useValue: 'en-US' },
        { provide: MatPaginatorIntl, useValue: getPaginatorIntl() },
        {
            provide: AppConfig,
            deps: [HttpClient, DatePipe],
            useExisting: StartupConfigService,
        },
        {
            provide: APP_INITIALIZER,
            multi: true,
            deps: [StartupConfigService],
            useFactory: StartupConfigFactory$,
        },
        provideFirebaseApp(() => initializeApp(environment.firebase)),
        provideStorage(() => getStorage()),
        importProvidersFrom(StoreModule.forRoot({ user: userReducer })),
        importProvidersFrom(EffectsModule.forRoot([UserEffects])),
        importProvidersFrom(NgxPermissionsModule.forRoot()),
        importProvidersFrom(QuillModule.forRoot()),
        importProvidersFrom(
            QuillConfigModule.forRoot({
                modules: {
                    syntax: false,
                    toolbar: defaultToolbar,
                },
            })
        ),

        provideAnimations(),
        provideToastr(),
        provideHttpClient(),
        provideRouter(
            appRoutes,
            withPreloading(PreloadAllModules),
            withInMemoryScrolling({ scrollPositionRestoration: 'enabled' })
        ),

        // Material Date Adapter
        {
            provide: DateAdapter,
            useClass: LuxonDateAdapter,
        },
        {
            provide: MAT_DATE_FORMATS,
            useValue: {
                parse: {
                    dateInput: 'D',
                },
                display: {
                    dateInput: 'DDD',
                    monthYearLabel: 'LLL yyyy',
                    dateA11yLabel: 'DD',
                    monthYearA11yLabel: 'LLLL yyyy',
                },
            },
        },
        provideAuth(),
        provideIcons(),
        provideFuse({
            fuse: {
                layout: 'classy',
                scheme: 'light',
                screens: {
                    sm: '600px',
                    md: '960px',
                    lg: '1280px',
                    xl: '1440px',
                },
                theme: 'theme-default',
                themes: [
                    {
                        id: 'theme-default',
                        name: 'Default',
                    },
                    {
                        id: 'theme-brand',
                        name: 'Brand',
                    },
                    {
                        id: 'theme-teal',
                        name: 'Teal',
                    },
                    {
                        id: 'theme-rose',
                        name: 'Rose',
                    },
                    {
                        id: 'theme-purple',
                        name: 'Purple',
                    },
                    {
                        id: 'theme-amber',
                        name: 'Amber',
                    },
                ],
            },
        }),
    ],
};

export const removeDialogConfig = {
    title: 'Remove item',
    message:
        'Are you sure you want to permanently remove this item? <span class="font-medium">This action cannot be undone!</span>',
    icon: {
        show: true,
        name: 'heroicons_outline:exclamation-triangle',
        color: 'warn',
    },
    actions: {
        confirm: {
            show: true,
            label: 'Remove',
            color: 'warn',
        },
        cancel: {
            show: true,
            label: 'Cancel',
        },
    },
    dismissible: true,
} as FuseConfirmationConfig;
