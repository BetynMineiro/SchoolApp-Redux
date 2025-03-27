import { HttpErrorResponse } from '@angular/common/http';
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
import { statusOptions } from 'app/core/enums/status.enum';
import { SchoolService } from 'app/core/school/school.service';
import {
    SchoolQueryResult,
    UpdateSchoolInput,
} from 'app/core/school/school.types';
import { materialProviders } from 'app/layout/common/material.providers';
import { ToastrService } from 'ngx-toastr';
import { Subject, takeUntil } from 'rxjs';

@Component({
    selector: 'tab-school-account',
    encapsulation: ViewEncapsulation.None,
    imports: [materialProviders],
    templateUrl: './school-account.component.html',
    styleUrl: './school-account.component.scss',
})
export class SchoolAccountComponent implements OnInit, OnDestroy {
    schoolId = input<string>(null);
    readOnly = input<boolean>(false);
    schoolForm: FormGroup;

    @ViewChild('schoolFormNgForm') schoolFormNgForm: NgForm;
    fullSchool = signal<SchoolQueryResult>(null);
    statusOptions = statusOptions;
    private destroy$ = new Subject<void>();
    constructor(
        private _formBuilder: FormBuilder,
        private toastr: ToastrService,
        private _schoolService: SchoolService
    ) {}

    ngOnDestroy(): void {
        this.destroy$.next();
        this.destroy$.complete();
    }

    async ngOnInit(): Promise<void> {
        this.schoolForm = this._formBuilder.group({
            fullName: ['', Validators.required],
            taxDocument: ['', Validators.required],
            avatar: [null],
            address: [null],
            status: [null, Validators.required],
            phoneNumber: [null],
            email: ['', [Validators.email]],
        });
        await this.requestSchool();

        this.schoolForm.valueChanges
            .pipe(takeUntil(this.destroy$))
            .subscribe((formValues) => {
                this.fullSchool.set({ ...this.fullSchool(), ...formValues });
            });
    }

    private async requestSchool() {
        await this._schoolService
            .getById(this.schoolId())
            .then((u) => {
                if (u.success) {
                    this.fullSchool.set(u.data);
                    this.setFormValuesFromSignal();
                } else {
                    u.messages.forEach((err) => {
                        this.toastr.error(err);
                    });
                }
            })
            .catch((err) => {
                this.toastr.error('Unexpected error identifying school');
            });
    }

    private setFormValuesFromSignal() {
        if (this.fullSchool()) {
            this.schoolForm.setValue({
                fullName: this.fullSchool().fullName,
                taxDocument: this.fullSchool().taxDocument,
                avatar: this.fullSchool().avatar || null,
                address: this.fullSchool().address || null,
                phoneNumber: this.fullSchool().phoneNumber || null,
                email: this.fullSchool().email,
                status: this.fullSchool().status,
            });
        }
    }

    async save() {
        if (this.schoolForm.invalid) {
            return;
        }
        this.schoolForm.disable();
        this._schoolService
            .update(new UpdateSchoolInput(this.fullSchool()))
            .then((response) => {
                if (response.success) {
                    this.toastr.success('School updated successfully!');
                }
            })
            .catch((err: HttpErrorResponse) => {
                err.error.messages.forEach((errorMessage) => {
                    this.toastr.error(errorMessage);
                });
            });

        this.schoolForm.enable();

        return;
    }

    async onFileSelected(event) {
        const file = event.target.files[0] as File;

        if (this.fullSchool()) {
            this.schoolForm.disable();
            const url = await this._schoolService.uploadAvatar(
                file,
                this.fullSchool()
            );
            this.schoolForm.patchValue({ avatar: url });
            await this.save();
            this.schoolForm.enable();
        }
    }
    async removeAvatar() {
        if (this.fullSchool) {
            this.schoolForm.disable();
            this.schoolForm.patchValue({ avatar: null });
            this.schoolForm.enable();
        }
    }
}
