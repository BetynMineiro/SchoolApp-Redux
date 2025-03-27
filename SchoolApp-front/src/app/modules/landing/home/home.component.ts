import {
    Component,
    inject,
    OnDestroy,
    OnInit,
    ViewEncapsulation,
} from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { Router } from '@angular/router';
import { AuthService } from 'app/core/auth/auth.service';
import { UserService } from 'app/core/user/user.service';
import { User } from 'app/core/user/user.types';
import { ToastrService } from 'ngx-toastr';
import { Subject, takeUntil } from 'rxjs';

@Component({
    selector: 'landing-home',
    templateUrl: './home.component.html',
    encapsulation: ViewEncapsulation.None,
    imports: [MatButtonModule, MatIconModule],
})
export class LandingHomeComponent implements OnInit, OnDestroy {
    private _userService = inject(UserService);
    private _router = inject(Router);
    private _toastService = inject(ToastrService);
    private authService = inject(AuthService);
    private _unsubscribeAll: Subject<any> = new Subject<any>();
    ngOnDestroy(): void {
        this._unsubscribeAll.next(null);
        this._unsubscribeAll.complete();
    }
    ngOnInit(): void {
        this._userService.user$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((user: User) => {
                if (user.isActive === false) {
                    this.authService.signOut();
                    this._toastService.error(
                        'Your account has been deactivated. Please contact the system administrator.'
                    );
                    this._router.navigate(['/sign-in']);
                    return;
                }
            });
    }
}
