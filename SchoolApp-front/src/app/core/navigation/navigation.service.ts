import { inject, Injectable } from '@angular/core';
import { FuseNavigationItem } from '@fuse/components/navigation';
import { Menu } from '@fuse/components/navigation/menu.model';
import { Navigation } from 'app/core/navigation/navigation.types';
import { AppConfig } from 'config/model/app.config.model';
import { NgxPermissionsService } from 'ngx-permissions';
import { map, Observable, of, ReplaySubject } from 'rxjs';
import { UserService } from '../user/user.service';

@Injectable({ providedIn: 'root' })
export class NavigationService {
    private _appConfig = inject(AppConfig);
    private permissionsService = inject(NgxPermissionsService);
    private userService = inject(UserService);
    private _navigation: ReplaySubject<Navigation> =
        new ReplaySubject<Navigation>(1);

    // -----------------------------------------------------------------------------------------------------
    // @ Accessors
    // -----------------------------------------------------------------------------------------------------

    /**
     * Getter for navigation
     */
    get navigation$(): Observable<Navigation> {
        return this._navigation.asObservable();
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Get all navigation data
     */
    get(): Observable<Navigation> {
        const menu = this._appConfig.navBar;

        return of(menu).pipe(
            map((item) => {
                const menuItems: FuseNavigationItem[] =
                    this.mapMenuToFuseNavigationItem(item);
                const navigation: Navigation = {
                    compact: menuItems,
                    default: menuItems,
                    futuristic: menuItems,
                    horizontal: menuItems,
                };

                this._navigation.next(navigation);

                return navigation;
            })
        );
    }

    private mapMenuToFuseNavigationItem(
        menuItems: Menu[]
    ): FuseNavigationItem[] {
        let navigationItem: FuseNavigationItem[] = [];

        menuItems.forEach((item) => {
            if (item.profile) {
                if (
                    !item.profile.some((profile) =>
                        this.permissionsService.getPermission(profile)
                    )
                )
                    return;
            }

            if (item.goTo == 'mySchool') {
                this.userService.user$
                    .subscribe((user) => {
                        item.goTo += `/${user?.managedSchoolId}`;
                    })
                    .unsubscribe();
            }
            navigationItem.push({
                id: item.name,
                title: item.name,
                type: this.hasChildren(item) ? 'collapsable' : 'basic',
                icon: item.icon,
                classes: item.classes,
                children: this.hasChildren(item)
                    ? this.mapMenuToFuseNavigationItem(item.subItems)
                    : null,
                link: `/${item.goTo}`,
            });
        });

        return navigationItem;
    }

    private hasChildren(menuItem: Menu): boolean {
        return menuItem.subItems ? true : false;
    }
}
