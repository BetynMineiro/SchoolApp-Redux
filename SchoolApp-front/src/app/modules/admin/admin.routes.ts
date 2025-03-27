import { Routes } from '@angular/router';
import { SchoolAddComponent } from './school-manager/school-add/school-add.component';
import { SchoolListComponent } from './school-manager/school-list/school-list.component';
import { SchoolManagerComponent } from './school-manager/school-manager.component';
import { SchoolUpdateComponent } from './school-manager/school-update/school-update.component';
import { UserAddComponent } from './user-manager/user-add/user-add.component';
import { UserListComponent } from './user-manager/user-list/user-list.component';
import { UserManagerComponent } from './user-manager/user-manager.component';
import { UserUpdateComponent } from './user-manager/user-update/user-update.component';

export default [
    {
        path: 'user',
        component: UserManagerComponent,
        children: [
            {
                path: '',
                component: UserListComponent,
            },
            {
                path: 'new',
                component: UserAddComponent,
            },
            {
                path: ':id',
                component: UserUpdateComponent,
            },
        ],
    },
    {
        path: 'school',
        component: SchoolManagerComponent,
        children: [
            {
                path: '',
                component: SchoolListComponent,
            },
            {
                path: 'new',
                component: SchoolAddComponent,
            },
            {
                path: ':id',
                component: SchoolUpdateComponent,
            },
        ],
    },
] as Routes;
