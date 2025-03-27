import { Component, ViewEncapsulation } from '@angular/core';

import { materialProviders } from 'app/layout/common/material.providers';
import { AccountTabComponent } from './tabs/account/account-tab.component';
import { PasswordTabComponent } from './tabs/security/password-tab.component';

@Component({
    selector: 'app-profile',
    encapsulation: ViewEncapsulation.None,
    imports: [materialProviders, AccountTabComponent, PasswordTabComponent],
    templateUrl: './profile.component.html',
    styleUrl: './profile.component.scss'
})
export class ProfileComponent {
    activeTab = 0;
    visible = true;

    toggleShow() {
        this.visible = !this.visible;
    }

    toggleActiveTab(tab: number) {
        this.activeTab = tab;
    }
}
