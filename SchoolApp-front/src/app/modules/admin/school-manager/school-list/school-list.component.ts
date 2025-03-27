import { HttpErrorResponse } from '@angular/common/http';
import {
    ChangeDetectorRef,
    Component,
    OnInit,
    signal,
    ViewEncapsulation,
} from '@angular/core';
import { Router } from '@angular/router';
import { statusOptions } from 'app/core/enums/status.enum';
import { SchoolService } from 'app/core/school/school.service';
import { SchoolQueryResult } from 'app/core/school/school.types';
import { materialProviders } from 'app/layout/common/material.providers';
import { CapsTableComponent } from 'app/modules/components/caps-table/caps-table.component';
import {
    RefreshDataTable,
    TableColumn,
} from 'app/modules/components/caps-table/interfaces/caps-table-types';
import { Pagination } from 'config/model/app.config.model';
import { NgxPermissionsService } from 'ngx-permissions';
import { ToastrService } from 'ngx-toastr';

@Component({
    selector: 'app-school-list',
    encapsulation: ViewEncapsulation.None,
    imports: [materialProviders, CapsTableComponent],
    templateUrl: './school-list.component.html',
    styleUrl: './school-list.component.scss',
})
export class SchoolListComponent implements OnInit {
    constructor(
        private _router: Router,
        private _schoolService: SchoolService,
        private _permissionService: NgxPermissionsService,
        private toastr: ToastrService,
        private cd: ChangeDetectorRef
    ) {}
    protected schoolList = signal<Pagination<SchoolQueryResult>>(null);
    private lastRefresh = signal<RefreshDataTable>({
        filterText: '',
        pageNumber: 1,
        pageSize: 15,
    });
    private async getSchools(filterText = '', pageNumber = 1, pageSize = 15) {
        await this._schoolService
            .getList(filterText, pageNumber, pageSize)
            .then((u) => {
                if (u.success) {
                    this.schoolList.set(u.data);
                } else {
                    u.messages.forEach((err) => {
                        this.toastr.error(err);
                    });
                }
                this.cd.detectChanges();
            })
            .catch(() => {
                this.toastr.error('Unexpected error while identifying school');
            });
    }

    createNewSchool() {
        this._router.navigate(['admin', 'school', 'new']);
    }

    async ngOnInit(): Promise<void> {
        if (
            !(await this._permissionService.hasPermission(['Admin', 'Judge']))
        ) {
            this._router.navigate(['']);
        }

        await this.getSchools();
    }

    tableColumn: TableColumn<SchoolQueryResult>[] = [
        {
            label: 'Id',
            property: 'id',
            type: 'text',
            visible: false,
            cssClasses: ['text-secondary', 'font-medium'],
        },
        {
            label: 'Avatar',
            property: 'avatar',
            type: 'image',
            visible: true,
            cssClasses: ['text-secondary', 'font-medium', 'w-12'],
        },
        {
            label: 'Name',
            property: 'fullName',
            type: 'text',
            visible: true,
            cssClasses: ['text-secondary', 'font-medium'],
        },
        {
            label: 'Email',
            property: 'email',
            type: 'text',
            visible: true,
            cssClasses: ['text-secondary', 'font-medium'],
        },
        {
            label: 'Phone',
            property: 'phoneNumber',
            type: 'text',
            visible: true,
            cssClasses: ['text-secondary', 'font-medium'],
        },

        {
            label: 'Status',
            property: 'status',
            type: 'enum',
            visible: true,
            enumData: statusOptions,
            cssClasses: ['text-secondary', 'font-medium'],
        },
    ];

    async refreshSchoolList(event: RefreshDataTable) {
        await this.getSchools(
            event.filterText,
            event.pageNumber,
            event.pageSize
        );
        this.lastRefresh.set(event);
    }

    async deleteSchool(event: SchoolQueryResult) {
        await this._schoolService
            .delete(event.id)
            .then((done) => {
                if (done.success) {
                    this.toastr.success('School successfully removed!');
                }
            })
            .catch((err: HttpErrorResponse) => {
                err.error.messages.forEach((err) => {
                    this.toastr.error(err);
                });
            });

        await this.refreshSchoolList(this.lastRefresh());
    }

    updateSchool(event: SchoolQueryResult) {
        this._router.navigate(['admin', 'school', event.id]);
    }
}
