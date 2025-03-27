import {
    Component,
    EventEmitter,
    OnDestroy,
    Output,
    ViewEncapsulation,
    computed,
    effect,
    input,
    signal,
} from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { FuseCardComponent } from '@fuse/components/card';
import { materialProviders } from 'app/layout/common/material.providers';
import { cloneDeep } from 'lodash';
import { Subject } from 'rxjs';
import { CardImagePreviewComponent } from './card-image-preview/card-image-preview.component';

@Component({
    selector: 'card-image',
    encapsulation: ViewEncapsulation.None,
    imports: [materialProviders, FuseCardComponent],
    templateUrl: './card-image.component.html',
    styleUrl: './card-image.component.scss',
})
export class CardImageComponent implements OnDestroy {
    initialized = false;
    private destroy$ = new Subject<void>();
    constructor(private _matDialog: MatDialog) {
        effect(
            () => {
                const updatedSrc = this.src();
                if (!this.initialized) {
                    this.initialized = true;
                    return;
                }
                if (Array.isArray(updatedSrc) && updatedSrc.length > 0) {
                    this.currentIndex.set(updatedSrc.length - 1);
                }
            },
            { allowSignalWrites: true }
        );
    }
    ngOnDestroy(): void {
        this.destroy$.next();
        this.destroy$.complete();
    }
    src = input<string | string[]>(null);
    useList = input<boolean>(false);
    showDeleteImage = input<boolean>(false);
    showAddImage = input<boolean>(false);
    readOnly = input<boolean>(false);
    useWaterMarks = input<boolean>(true);
    @Output() readonly onAddImage: EventEmitter<File> =
        new EventEmitter<File>();
    @Output() readonly onRemoveImage: EventEmitter<string> =
        new EventEmitter<string>();

    currentIndex = signal<number>(0);

    computedSrc = computed<string>(() => {
        if (!this.useList() || !this.src()) {
            return this.src() as string;
        } else {
            const list = this.src() as string[];
            const index = this.currentIndex();
            return list[index];
        }
    });

    openPreviewDialog(src: any): void {
        if (!src) return;

        this._matDialog.open(CardImagePreviewComponent, {
            autoFocus: false,
            width: '10%',
            data: {
                src: cloneDeep(src),
                useWaterMarks: this.useWaterMarks(),
            },
        });
    }

    async onFileSelected(event) {
        const file = event.target.files[0] as File;
        this.onAddImage.emit(file);
    }
    async removeFile(event: string) {
        this.onRemoveImage.emit(event);
    }

    next() {
        if (Array.isArray(this.src())) {
            const list = this.src();
            const nextIndex = this.currentIndex() + 1;
            this.currentIndex.set(nextIndex >= list.length ? 0 : nextIndex);
        }
    }

    previous() {
        if (Array.isArray(this.src())) {
            const list = this.src();
            const prevIndex = this.currentIndex() - 1;

            this.currentIndex.set(prevIndex < 0 ? list.length - 1 : prevIndex);
        }
    }
}
