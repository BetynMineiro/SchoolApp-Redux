import {
    Component,
    input,
    OnInit,
    signal,
    ViewChild,
    ViewEncapsulation,
} from '@angular/core';
import { FormBuilder, FormGroup, NgForm, Validators } from '@angular/forms';
import { FuseValidators } from '@fuse/validators';
import { UserService } from 'app/core/user/user.service';
import { UserQueryResult } from 'app/core/user/user.types';
import { materialProviders } from 'app/layout/common/material.providers';
import { ToastrService } from 'ngx-toastr';

@Component({
    selector: 'app-security-update',
    encapsulation: ViewEncapsulation.None,
    imports: [materialProviders],
    templateUrl: './security-update.component.html',
    styleUrl: './security-update.component.scss',
})
export class SecurityUpdateComponent implements OnInit {
    userId = input<string>(null);
    @ViewChild('passwordFormNgForm') passwordFormNgForm: NgForm;
    passwordForm: FormGroup;

    private user = signal<UserQueryResult>(null);
    private StrongPasswordRegex: RegExp =
        /^(?=[^A-Z]*[A-Z])(?=[^a-z]*[a-z])(?=\D*\d).{8,}$/; // 8 characters, at least one uppercase letter, one lowercase letter, and one digit

    constructor(
        private _formBuilder: FormBuilder,
        private toastr: ToastrService,
        private _userService: UserService
    ) {}

    async ngOnInit(): Promise<void> {
        await this.requestUser();
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

    private async requestUser() {
        await this._userService
            .getUserById(this.userId())
            .then((u) => {
                if (u.success) {
                    this.user.set(u.data);
                } else {
                    u.messages.forEach((err) => {
                        this.toastr.error(err);
                    });
                }
            })
            .catch((err) => {
                this.toastr.error('Unexpected error identifying user');
            });
    }
}
