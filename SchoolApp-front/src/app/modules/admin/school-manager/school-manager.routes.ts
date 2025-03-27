import { Routes } from '@angular/router';
import { SchoolUpdateComponent } from './school-update/school-update.component';

export default [
    {
        path: ':id',
        component: SchoolUpdateComponent,
    },
] as Routes;
