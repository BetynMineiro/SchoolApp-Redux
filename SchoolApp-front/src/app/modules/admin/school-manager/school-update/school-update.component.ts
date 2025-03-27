import { WatermarkDirective, WatermarkOptions } from '@acrodata/watermark';
import {
    Component,
    OnDestroy,
    OnInit,
    ViewEncapsulation,
    signal,
} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SchoolService } from 'app/core/school/school.service';
import { SchoolQueryResult } from 'app/core/school/school.types';
import { UserService } from 'app/core/user/user.service';
import { materialProviders } from 'app/layout/common/material.providers';
import { MaintenanceComponent } from 'app/modules/components/maintenance/maintenance.component';
import { NgxPermissionsService } from 'ngx-permissions';
import { ToastrService } from 'ngx-toastr';
import { Subject, takeUntil } from 'rxjs';
import { SchoolModules } from '../school-manager.component';
import { schoolModulesOptions } from './../school-manager.component';
import { SchoolAccountComponent } from './tabs/school-account/school-account.component';

@Component({
    selector: 'app-school-update',
    encapsulation: ViewEncapsulation.None,
    imports: [
        materialProviders,
        SchoolAccountComponent,
        MaintenanceComponent,
        WatermarkDirective,
    ],
    templateUrl: './school-update.component.html',
    styleUrl: './school-update.component.scss',
})
export class SchoolUpdateComponent implements OnInit, OnDestroy {
    constructor(
        private _router: Router,
        private _permissionService: NgxPermissionsService,
        private _activatedRoute: ActivatedRoute,
        private _schoolService: SchoolService,
        private toastr: ToastrService,
        private _userService: UserService
    ) {
        this._userService.user$
            .pipe(takeUntil(this.destroy$))
            .subscribe((data) => {
                this.options.set({
                    text: data.id,
                    fontSize: 20,
                    gapY: 250,
                    gapX: 200,
                    rotate: 0,
                });
            });

        this.RouteParamListener();
    }
    options = signal<WatermarkOptions>(null);
    fullSchool = signal<SchoolQueryResult>(null);
    readOnly = signal<boolean>(false);
    isSchoolManager = signal<boolean>(false);
    schoolId = signal<string>(null);
    activeTab = signal<SchoolModules>(SchoolModules.Account);
    private destroy$ = new Subject<void>();
    visible = true;
    SchoolModules = SchoolModules;
    schoolModulesOptions = schoolModulesOptions;
    toggleShow() {
        this.visible = !this.visible;
    }

    async ngOnInit(): Promise<void> {
        if (
            !(await this._permissionService.hasPermission([
                'Admin',
                'Judge',
                'SchoolManager',
            ]))
        ) {
            this._router.navigate(['']);
        }

        this.isSchoolManager.set(
            this._permissionService.getPermissions()?.SchoolManager
                ? true
                : false
        );

        this.readOnly.set(
            this._permissionService.getPermissions()?.Judge ? true : false
        );

        await this.requestSchool();
    }
    private async requestSchool() {
        await this._schoolService
            .getById(this.schoolId())
            .then((u) => {
                if (u.success) {
                    this.fullSchool.set(u.data);
                } else {
                    u.messages.forEach((err) => {
                        this.toastr.error(err);
                    });
                }
            })
            .catch((err) => {
                this.toastr.error('Unexpected error identifying school');
            });
    }

    ngOnDestroy(): void {
        this.destroy$.next();
        this.destroy$.complete();
    }

    toggleActiveTab(tab: SchoolModules) {
        this.activeTab.set(tab);
    }
    RouteParamListener() {
        this._activatedRoute.params
            .pipe(takeUntil(this.destroy$))
            .subscribe(async (p) => {
                if (p.id) {
                    this.schoolId.set(p.id);
                }
            });
    }
}
