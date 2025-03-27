import {
    ChangeDetectorRef,
    Component,
    OnInit,
    signal,
    ViewEncapsulation,
} from '@angular/core';
import { Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { statusOptions } from 'app/core/enums/status.enum';
import { selectUserList } from 'app/core/user/store/user.selectors';
import {
    profileTypeOptions,
    UserQueryResultForList,
} from 'app/core/user/user.types';
import { materialProviders } from 'app/layout/common/material.providers';
import { CapsTableComponent } from 'app/modules/components/caps-table/caps-table.component';
import {
    RefreshDataTable,
    TableColumn,
} from 'app/modules/components/caps-table/interfaces/caps-table-types';
import { Pagination } from 'config/model/app.config.model';
import { NgxPermissionsService } from 'ngx-permissions';
import { ToastrService } from 'ngx-toastr';
import * as UserActions from '../../../../core/user/store/user.actions';
@Component({
    selector: 'app-user-list',
    encapsulation: ViewEncapsulation.None,
    imports: [materialProviders, CapsTableComponent],
    templateUrl: './user-list.component.html',
    styleUrl: './user-list.component.scss',
})
export class UserListComponent implements OnInit {
    constructor(
        private _router: Router,
        private _permissionService: NgxPermissionsService,
        private toastr: ToastrService,
        private cd: ChangeDetectorRef,
        private store: Store
    ) {}
    protected userList = signal<Pagination<UserQueryResultForList>>(null);
    private lastRefresh = signal<RefreshDataTable>({
        filterText: '',
        pageNumber: 1,
        pageSize: 15,
    });

    createNewUser() {
        this._router.navigate(['admin', 'user', 'new']);
    }

    async ngOnInit(): Promise<void> {
        if (!(await this._permissionService.hasPermission('Admin'))) {
            this._router.navigate(['']);
        }
        this.refreshUserList(this.lastRefresh());
        this.store.select(selectUserList).subscribe((list) => {
            this.userList.set(list);
            this.cd.detectChanges();
        });
    }

    tableColumn: TableColumn<UserQueryResultForList>[] = [
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
            label: 'Profile',
            property: 'profileType',
            type: 'enum',
            visible: true,
            enumData: profileTypeOptions,
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

    async refreshUserList(event: RefreshDataTable) {
        this.lastRefresh.set(event);
        this.store.dispatch(
            UserActions.loadUserList({
                filterText: event.filterText,
                pageNumber: event.pageNumber,
                pageSize: event.pageSize,
            })
        );
    }

    async deleteUser(event: UserQueryResultForList) {
        this.store.dispatch(UserActions.deleteUser({ id: event.id }));
        this.toastr.success('User removed (Redux)!');
        this.refreshUserList(this.lastRefresh());
    }

    updateUser(event: UserQueryResultForList) {
        this._router.navigate(['admin', 'user', event.id]);
    }
}
