import { inject } from '@angular/core';
import { NavigationService } from 'app/core/navigation/navigation.service';
import { concatMap, forkJoin } from 'rxjs';
import { UserService } from './core/user/user.service';

export const initialDataResolver = () => {
    const navigationService = inject(NavigationService);
    const userService = inject(UserService);

    // Fork join multiple API endpoint calls to wait all of them to finish
    return userService
        .get()
        .pipe(concatMap(() => forkJoin([navigationService.get()])));
};
