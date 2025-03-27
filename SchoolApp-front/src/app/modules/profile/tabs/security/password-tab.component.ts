import {
    Component,
    OnDestroy,
    OnInit,
    signal,
    ViewChild,
    ViewEncapsulation,
} from '@angular/core';
import { FormBuilder, FormGroup, NgForm, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { FuseValidators } from '@fuse/validators';
import { UserService } from 'app/core/user/user.service';
import { User } from 'app/core/user/user.types';
import { materialProviders } from 'app/layout/common/material.providers';
import { ToastrService } from 'ngx-toastr';
import { Subject, takeUntil } from 'rxjs';

@Component({
    selector: 'password-tab',
    templateUrl: './password-tab.component.html',
    encapsulation: ViewEncapsulation.None,
    imports: [materialProviders],
})
export class PasswordTabComponent implements OnInit, OnDestroy {
    @ViewChild('passwordFormNgForm') passwordFormNgForm: NgForm;
    passwordForm: FormGroup;
    private destroy$ = new Subject<void>();
    private user = signal<User>(null);
    private StrongPasswordRegex: RegExp =
        /^(?=[^A-Z]*[A-Z])(?=[^a-z]*[a-z])(?=\D*\d).{8,}$/; // 8 characters, at least one uppercase letter, one lowercase letter, and one digit

    constructor(
        private _formBuilder: FormBuilder,
        private toastr: ToastrService,
        private _userService: UserService,
        private _router: Router
    ) {
        this._userService.user$
            .pipe(takeUntil(this.destroy$))
            .subscribe((data) => {
                this.user.set(data);
            });
    }

    ngOnDestroy(): void {
        this.destroy$.next();
        this.destroy$.complete();
    }

    ngOnInit(): void {
        this.passwordForm = this._formBuilder.group(
            {
                newPassword: ['', Validators.required],
                newPasswordConfirm: [
                    '',
                    [
                        Validators.required,
                        Validators.pattern(this.StrongPasswordRegex),
                    ],
                ],
            },
            {
                validators: FuseValidators.mustMatch(
                    'newPassword',
                    'newPasswordConfirm'
                ),
            }
        );
    }

    async save() {
        if (this.passwordForm.invalid) {
            return;
        }
        this.passwordForm.disable();
        await this._userService
            .updatePassword(
                this.user().id,
                this.passwordForm.value.newPasswordConfirm
            )
            .then((u) => {
                if (u.success) {
                    this.toastr.success('Password updated successfully');
                    this._router.navigate(['/sign-out']);
                } else {
                    u.messages.forEach((err) => {
                        this.toastr.error(err);
                    });
                }
            })
            .catch((err) => {
                this.toastr.error('Unexpected error updating password');
            });
        this.passwordForm.enable();
        this.passwordFormNgForm.resetForm();

        return;
    }
}
