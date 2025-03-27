import { HttpErrorResponse } from '@angular/common/http';
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
import { SchoolService } from 'app/core/school/school.service';
import { SchoolQueryResult } from 'app/core/school/school.types';
import { UserService } from 'app/core/user/user.service';
import {
    CreateUserInput,
    genderOptions,
    PersonType,
    ProfileType,
} from 'app/core/user/user.types';
import { materialProviders } from 'app/layout/common/material.providers';
import { NgxPermissionsService } from 'ngx-permissions';
import { ToastrService } from 'ngx-toastr';
import { Subject, takeUntil } from 'rxjs';
import { profileTypeOptions } from '../../../../core/user/user.types';

@Component({
    selector: 'app-user-add',
    encapsulation: ViewEncapsulation.None,
    imports: [materialProviders],
    templateUrl: './user-add.component.html',
    styleUrl: './user-add.component.scss',
})
export class UserAddComponent implements OnInit, OnDestroy {
    userForm: FormGroup;

    private destroy$ = new Subject<void>();
    @ViewChild('userFormNgForm') userFormNgForm: NgForm;
    today = new Date();
    protected fullUser = signal<CreateUserInput>(null);
    profileType = ProfileType;
    protected schoolList = signal<SchoolQueryResult[]>(null);
    genderOptions = genderOptions;
    profileTypeOptions = profileTypeOptions;
    private StrongPasswordRegex: RegExp =
        /^(?=[^A-Z]*[A-Z])(?=[^a-z]*[a-z])(?=\D*\d).{8,}$/;

    constructor(
        private _formBuilder: FormBuilder,
        private toastr: ToastrService,
        private _router: Router,
        private _userService: UserService,
        private _permissionService: NgxPermissionsService,
        private _schoolService: SchoolService
    ) {}

    ngOnDestroy(): void {
        this.destroy$.next();
        this.destroy$.complete();
    }

    async ngOnInit(): Promise<void> {
        if (!(await this._permissionService.hasPermission('Admin'))) {
            this._router.navigate(['']);
        }

        this.userForm = this._formBuilder.group(
            {
                fullName: ['', Validators.required],
                socialName: [null],
                taxDocument: ['', Validators.required],
                gender: [null],
                profileType: [null, Validators.required],
                personType: [PersonType.Person, Validators.required],
                birthDay: [null],
                address: [null],
                phoneNumber: [null],
                managedSchoolId: [null],
                email: ['', [Validators.required, Validators.email]],
                password: ['', Validators.required],
                passwordConfirm: [
                    '',
                    [
                        Validators.required,
                        Validators.pattern(this.StrongPasswordRegex),
                    ],
                ],
            },
            {
                validators: FuseValidators.mustMatch(
                    'password',
                    'passwordConfirm'
                ),
            }
        );

        this.userForm.valueChanges
            .pipe(takeUntil(this.destroy$))
            .subscribe((formValues) => {
                this.fullUser.set({ ...this.fullUser(), ...formValues });
            });
        await this.getSchools();
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
                this.toastr.error('Unexpected error while identifying school');
            });
    }
    async save() {
        if (this.userForm.invalid) {
            return;
        }
        this.userForm.disable();
        this._userService
            .create(new CreateUserInput(this.fullUser()))
            .then((done) => {
                if (done.success) {
                    this.toastr.success('User created successfully!');
                    this._router.navigate(['admin', 'user']);
                }
            })
            .catch((err: HttpErrorResponse) => {
                err.error.messages.forEach((err) => {
                    this.toastr.error(err);
                });
            });

        this.userForm.enable();

        return;
    }
}
