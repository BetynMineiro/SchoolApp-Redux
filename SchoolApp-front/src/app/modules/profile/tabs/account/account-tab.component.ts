import { HttpErrorResponse } from '@angular/common/http';
import {
    Component,
    OnInit,
    signal,
    ViewChild,
    ViewEncapsulation,
} from '@angular/core';
import { FormBuilder, FormGroup, NgForm, Validators } from '@angular/forms';
import { UserService } from 'app/core/user/user.service';
import {
    Gender,
    genderOptions,
    UpdateUserInput,
    User,
    UserQueryResult,
} from 'app/core/user/user.types';
import { materialProviders } from 'app/layout/common/material.providers';
import { ToastrService } from 'ngx-toastr';
import { Subject, takeUntil } from 'rxjs';

@Component({
    selector: 'account-tab',
    templateUrl: './account-tab.component.html',
    encapsulation: ViewEncapsulation.None,
    imports: [materialProviders]
})
export class AccountTabComponent implements OnInit {
    userForm: FormGroup;
    today = new Date();
    private destroy$ = new Subject<void>();
    @ViewChild('userFormNgForm') userFormNgForm: NgForm;
    private user = signal<User>(null);
    private fullUser = signal<UserQueryResult>(null);
    genderOptions = genderOptions;

    constructor(
        private _formBuilder: FormBuilder,
        private toastr: ToastrService,
        private _userService: UserService
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
    get getAvatar() {
        return this.user().avatar;
    }

    async ngOnInit(): Promise<void> {
        this.userForm = this._formBuilder.group({
            fullName: ['', Validators.required],
            socialName: [null],
            taxDocument: ['', Validators.required],
            avatar: [null],
            gender: [null],
            birthDay: [null],
            address: [null],
            phoneNumber: [null],
            email: ['', [Validators.required, Validators.email]],
        });
        await this.requestUser();

        this.setFormValuesFromSignal();
        this.userForm.valueChanges
            .pipe(takeUntil(this.destroy$))
            .subscribe((formValues) => {
                this.fullUser.set({ ...this.fullUser(), ...formValues });
            });
    }

    private async requestUser() {
        await this._userService
            .getUserById(this.user().id)
            .then((u) => {
                if (u.success) {
                    this.fullUser.set(u.data);
                    this.user().avatar = u.data.avatar;
                    this._userService.user = this.user();
                } else {
                    u.messages.forEach((err) => {
                        this.toastr.error(err);
                    });
                }
            })
            .catch((err) => {
                this.toastr.error('Erro não esperado ao identificar usuário');
            });
    }

    private setFormValuesFromSignal() {
        if (this.fullUser()) {
            this.userForm.setValue({
                fullName: this.fullUser().fullName,
                socialName: this.fullUser().socialName || null,
                taxDocument: this.fullUser().taxDocument,
                avatar: this.fullUser().avatar || null,
                gender: this.fullUser().gender || Gender.None,
                birthDay: this.fullUser().birthDay || null,
                address: this.fullUser().address || null,
                phoneNumber: this.fullUser().phoneNumber || null,
                email: this.fullUser().email,
            });
        }
    }

    async save() {
        if (this.userForm.invalid) {
            return;
        }
        this.userForm.disable();
        this._userService
            .updateSelf(new UpdateUserInput(this.fullUser()))
            .pipe(takeUntil(this.destroy$))
            .subscribe({
                next: (response) => {
                    if (response.success) {
                        this.toastr.success('Usuário atualizado com sucesso!');
                    }
                },
                error: (err: HttpErrorResponse) => {
                    err.error.messages.forEach((errorMessage) => {
                        this.toastr.error(errorMessage);
                    });
                },
            });

        this.userForm.enable();

        return;
    }

    async onFileSelected(event) {
        const file = event.target.files[0] as File;

        if (this.user()) {
            this.userForm.disable();
            const url = await this._userService.uploadAvatar(file, this.user());
            this.userForm.patchValue({ avatar: url });
            await this.save();
            this.userForm.enable();
        }
    }
    async removeAvatar() {
        if (this.fullUser) {
            this.userForm.disable();
            this.userForm.patchValue({ avatar: null });
            await this.save();
            await new Promise((resolve) => setTimeout(resolve, 1000));
            await this.requestUser();
            this.userForm.enable();
        }
    }
}
