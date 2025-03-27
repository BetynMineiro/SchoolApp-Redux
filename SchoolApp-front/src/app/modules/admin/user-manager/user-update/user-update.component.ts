import {
    Component,
    OnDestroy,
    OnInit,
    signal,
    ViewEncapsulation,
} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { materialProviders } from 'app/layout/common/material.providers';
import { NgxPermissionsService } from 'ngx-permissions';
import { Subject, takeUntil } from 'rxjs';
import { AccountUpdateComponent } from './tabs/account-update/account-update.component';
import { SecurityUpdateComponent } from './tabs/security-update/security-update.component';

@Component({
    selector: 'app-user-update',
    encapsulation: ViewEncapsulation.None,
    imports: [
        materialProviders,
        AccountUpdateComponent,
        SecurityUpdateComponent,
    ],
    templateUrl: './user-update.component.html',
    styleUrl: './user-update.component.scss',
})
export class UserUpdateComponent implements OnInit, OnDestroy {
    constructor(
        private _router: Router,
        private _permissionService: NgxPermissionsService,
        private _activatedRoute: ActivatedRoute
    ) {
        this.routeParamListener();
    }
    userId = signal<string>(null);
    activeTab = 0;
    private destroy$ = new Subject<void>();
    visible = true;

    toggleShow() {
        this.visible = !this.visible;
    }

    async ngOnInit(): Promise<void> {
        if (!(await this._permissionService.hasPermission('Admin'))) {
            this._router.navigate(['']);
        }
    }

    ngOnDestroy(): void {
        this.destroy$.next();
        this.destroy$.complete();
    }

    toggleActiveTab(tab: number) {
        this.activeTab = tab;
    }
    routeParamListener() {
        this._activatedRoute.params
            .pipe(takeUntil(this.destroy$))
            .subscribe(async (p) => {
                if (p.id) {
                    this.userId.set(p.id);
                }
            });
    }
}
