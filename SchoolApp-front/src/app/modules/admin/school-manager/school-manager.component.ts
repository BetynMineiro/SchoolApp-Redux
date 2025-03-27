import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
    selector: 'app-school-manager',
    imports: [RouterOutlet],
    template: '<router-outlet></router-outlet>',
})
export class SchoolManagerComponent {}

export enum SchoolModules {
    Account,
}

export const schoolModulesOptions = [
    {
        key: SchoolModules.Account,
        value: { title: 'Account', subTitle: 'Update school data' },
    },
];
