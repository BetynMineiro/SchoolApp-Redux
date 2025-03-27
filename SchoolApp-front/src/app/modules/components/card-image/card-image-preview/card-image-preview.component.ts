import {
    ChangeDetectionStrategy,
    Component,
    Inject,
    OnInit,
    ViewEncapsulation,
    signal,
} from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';

@Component({
    selector: 'app-card-image-preview',
    encapsulation: ViewEncapsulation.None,
    changeDetection: ChangeDetectionStrategy.OnPush,
    imports: [MatDialogModule],
    templateUrl: './card-image-preview.component.html',
    styleUrl: './card-image-preview.component.scss',
})
export class CardImagePreviewComponent implements OnInit {
    src = signal<string>(null);
    useWaterMarks = false;
    constructor(
        @Inject(MAT_DIALOG_DATA)
        private _data: { src: string; useWaterMarks?: boolean }
    ) {}
    ngOnInit(): void {
        if (this._data) {
            this.src.set(this._data.src);
            this.useWaterMarks = this._data.useWaterMarks;
        }
    }
}
