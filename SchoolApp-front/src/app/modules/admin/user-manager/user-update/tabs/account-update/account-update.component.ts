import {
    Component,
    input,
    OnDestroy,
    OnInit,
    signal,
    ViewChild,
    ViewEncapsulation,
} from '@angular/core';
import { FormBuilder, FormGroup, NgForm, Validators } from '@angular/forms';
import { Store } from '@ngrx/store';
import { statusOptions } from 'app/core/enums/status.enum';
import { SchoolService } from 'app/core/school/school.service';
import { SchoolQueryResult } from 'app/core/school/school.types';
import { selectUserDetail } from 'app/core/user/store/user.selectors';
import { UserService } from 'app/core/user/user.service';
import {
    Gender,
    genderOptions,
    PersonType,
    profileTypeOptions,
    UpdateUserInput,
    UserQueryResult,
} from 'app/core/user/user.types';
import { materialProviders } from 'app/layout/common/material.providers';
import { ToastrService } from 'ngx-toastr';
import { Subject, takeUntil } from 'rxjs';
import * as UserActions from '../../../../../../core/user/store/user.actions';
import { ProfileType } from '../../../../../../core/user/user.types';
@Component({
    selector: 'app-account-update',
    encapsulation: ViewEncapsulation.None,
    imports: [materialProviders],
    templateUrl: './account-update.component.html',
    styleUrl: './account-update.component.scss',
})
export class AccountUpdateComponent implements OnInit, OnDestroy {
    userId = input<string>(null);
    userForm: FormGroup;
    today = new Date();
    private destroy$ = new Subject<void>();
    @ViewChild('userFormNgForm') userFormNgForm: NgForm;
    fullUser = signal<UserQueryResult>(null);
    protected schoolList = signal<SchoolQueryResult[]>(null);
    genderOptions = genderOptions;
    profileTypeOptions = profileTypeOptions;
    profileType = ProfileType;
    statusOptions = statusOptions;

    constructor(
        private _formBuilder: FormBuilder,
        private toastr: ToastrService,
        private store: Store,
        private _schoolService: SchoolService,
        private _userService: UserService
    ) {}

    private requestUserFromStore() {
        this.store.dispatch(UserActions.loadUserById({ id: this.userId() }));

        this.store
            .select(selectUserDetail)
            .pipe(takeUntil(this.destroy$))
            .subscribe((user) => {
                if (user) {
                    this.fullUser.set(user as UserQueryResult);
                    this.setFormValuesFromSignal();
                }
            });
    }
    ngOnDestroy(): void {
        this.destroy$.next();
        this.destroy$.complete();
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
            profileType: [null, Validators.required],
            status: [null, Validators.required],
            personType: [PersonType.Person, Validators.required],
            phoneNumber: [null],
            managedSchoolId: [null],
            email: ['', [Validators.required, Validators.email]],
        });
        this.requestUserFromStore();
        await this.getSchools();

        this.userForm.valueChanges
            .pipe(takeUntil(this.destroy$))
            .subscribe((formValues) => {
                this.fullUser.set({ ...this.fullUser(), ...formValues });
            });
    }

    private async requestUser() {
        await this._userService
            .getUserById(this.userId())
            .then((u) => {
                if (u.success) {
                    this.fullUser.set(u.data);
                    this.setFormValuesFromSignal();
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
    private async getSchools() {
        await this._schoolService
            .getList('', 1, 500)
            .then((u) => {
                if (u.success) {
                    this.schoolList.set(u.data.items);
                } else {
                    u.messages.forEach((err) => {
                        this.toastr.error(err);
                    });
                }
            })
            .catch(() => {
                this.toastr.error('Unexpected error identifying school');
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
                profileType: this.fullUser().profileType,
                status: this.fullUser().status,
                personType: this.fullUser().personType,
                managedSchoolId: this.fullUser().managedSchoolId,
            });
        }
    }

    async save() {
        if (this.userForm.invalid) {
            return;
        }
        this.userForm.disable();
        this.store.dispatch(
            UserActions.updateUser({
                user: new UpdateUserInput(this.fullUser()),
            })
        );

        this.userForm.enable();

        return;
    }

    async onFileSelected(event) {
        const file = event.target.files[0] as File;

        if (this.fullUser()) {
            this.userForm.disable();
            const url = await this._userService.uploadAvatar(
                file,
                this.fullUser()
            );
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
