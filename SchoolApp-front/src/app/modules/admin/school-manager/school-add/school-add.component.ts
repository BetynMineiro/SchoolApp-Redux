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
import { SchoolService } from 'app/core/school/school.service';
import { CreateSchoolInput } from 'app/core/school/school.types';
import { materialProviders } from 'app/layout/common/material.providers';
import { NgxPermissionsService } from 'ngx-permissions';
import { ToastrService } from 'ngx-toastr';
import { Subject, takeUntil } from 'rxjs';

@Component({
    selector: 'app-school-add',
    encapsulation: ViewEncapsulation.None,
    imports: [materialProviders],
    templateUrl: './school-add.component.html',
    styleUrl: './school-add.component.scss',
})
export class SchoolAddComponent implements OnInit, OnDestroy {
    schoolForm: FormGroup;
    private destroy$ = new Subject<void>();
    @ViewChild('schoolFormNgForm') schoolFormNgForm: NgForm;

    private fullSchool = signal<CreateSchoolInput>(null);

    constructor(
        private _formBuilder: FormBuilder,
        private toastr: ToastrService,
        private _router: Router,
        private _schoolService: SchoolService,
        private _permissionService: NgxPermissionsService
    ) {}

    ngOnDestroy(): void {
        this.destroy$.next();
        this.destroy$.complete();
    }

    async ngOnInit(): Promise<void> {
        if (!(await this._permissionService.hasPermission('Admin'))) {
            this._router.navigate(['']);
        }

        this.schoolForm = this._formBuilder.group({
            fullName: ['', Validators.required],
            taxDocument: ['', Validators.required],
            address: [null],
            phoneNumber: [null],
            email: ['', [Validators.email]],
        });

        this.schoolForm.valueChanges
            .pipe(takeUntil(this.destroy$))
            .subscribe((formValues) => {
                this.fullSchool.set({ ...this.fullSchool(), ...formValues });
            });
    }

    async save() {
        if (this.schoolForm.invalid) {
            return;
        }
        this.schoolForm.disable();
        this._schoolService
            .create(new CreateSchoolInput(this.fullSchool()))
            .then((done) => {
                if (done.success) {
                    this.toastr.success('School created successfully!');
                    this._router.navigate(['admin', 'school']);
                }
            })
            .catch((err: HttpErrorResponse) => {
                err.error.messages.forEach((err) => {
                    this.toastr.error(err);
                });
            });

        this.schoolForm.enable();

        return;
    }
}
