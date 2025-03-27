import { SelectionModel } from '@angular/cdk/collections';
import {
    Component,
    computed,
    EventEmitter,
    input,
    Input,
    OnDestroy,
    Output,
    signal,
    ViewChild,
    ViewEncapsulation,
} from '@angular/core';
import { FormControl } from '@angular/forms';

import {
    MAT_FORM_FIELD_DEFAULT_OPTIONS,
    MatFormFieldDefaultOptions,
} from '@angular/material/form-field';

import { debounceTime, Subject, takeUntil } from 'rxjs';

import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

import { MatDialog } from '@angular/material/dialog';
import {
    FuseConfirmationConfig,
    FuseConfirmationService,
} from '@fuse/services/confirmation';
import { removeDialogConfig } from 'app/app.config';
import { materialProviders } from 'app/layout/common/material.providers';
import { Pagination } from 'config/model/app.config.model';
import { cloneDeep } from 'lodash';
import { CardImagePreviewComponent } from '../card-image/card-image-preview/card-image-preview.component';
import { fadeInUp400ms } from './animations/fade-in-up.animation';
import { stagger40ms } from './animations/stagger.animation';
import { RefreshDataTable, TableColumn } from './interfaces/caps-table-types';

@Component({
    selector: 'caps-table',
    templateUrl: './caps-table.component.html',
    styleUrls: ['./caps-table.component.scss'],
    animations: [fadeInUp400ms, stagger40ms],
    encapsulation: ViewEncapsulation.None,
    imports: [materialProviders],
    providers: [
        {
            provide: MAT_FORM_FIELD_DEFAULT_OPTIONS,
            useValue: {
                appearance: 'fill',
            } as MatFormFieldDefaultOptions,
        },
    ],
})
export class CapsTableComponent implements OnDestroy {
    @Input('title')
    tableHeaderText = '';
    @Input()
    showDeleteAction = false;
    @Input()
    showUpdateAction = false;
    @Input()
    showCustomAction = false;
    @Input()
    customActionTitle = '';
    @Input()
    customActionIcon = '';
    @Input()
    showActions = false;
    @Input()
    showSelections = false;
    @Input()
    readOnly = false;
    @Input()
    showSearchField = true;
    @Input()
    showImageModal = true;
    @Input() enableUpdateOnDoubleClick = true;
    @Input() columnsHeaders: TableColumn<any>[] = [];
    @Input() useWaterMarks = false;
    pageSizeOptions = [5, 10, 15, 20, 25, 50];

    removeDialogConfig = input<FuseConfirmationConfig>(removeDialogConfig);

    columnsData = input<Pagination<any[]>>();
    page = computed<PageEvent>(() => {
        const pagination = this.columnsData();
        this.dataSource.data = this.columnsData().items;
        this.dataSource.sort = this.sort;
        if (!pagination) {
            return {
                pageIndex: 0,
                pageSize: 0,
                length: 0,
            } as PageEvent;
        }

        const pageIndex = pagination.pageNumber - 1;
        const pageSize = pagination.pageSize;
        const length = pagination.totalItems;

        return {
            pageIndex: pageIndex,
            pageSize: pageSize,
            length: length,
        } as PageEvent;
    });
    private destroy$ = new Subject<void>();

    @Output()
    onDeleteItem: EventEmitter<any> = new EventEmitter<any>();

    @Output()
    onCustomAction: EventEmitter<any> = new EventEmitter<any>();

    @Output()
    onDeleteManyItems: EventEmitter<any[]> = new EventEmitter<any[]>();

    @Output()
    onUpdateItem: EventEmitter<any> = new EventEmitter<any>();

    @Output()
    onReadOnlyPreviewItem: EventEmitter<any> = new EventEmitter<any>();

    @Output()
    onExportSelectedItemsEvent: EventEmitter<any> = new EventEmitter<any>();

    @Output('onPageChanged')
    onPageChangedEvent: EventEmitter<RefreshDataTable> =
        new EventEmitter<RefreshDataTable>();

    @Output('onSearch') onSearchInputChange: EventEmitter<RefreshDataTable> =
        new EventEmitter<RefreshDataTable>();

    filterValue = signal<string>('');

    selectionHeader: TableColumn<any> = {
        label: '',
        property: 'checkbox',
        type: 'checkbox',
        visible: true,
    };

    actionsHeader: TableColumn<any> = {
        label: '',
        property: 'actions',
        type: 'button',
        visible: true,
    };

    get columns(): TableColumn<any>[] {
        let columns: TableColumn<any>[] = [];
        if (this.showSelections) {
            columns.push(this.selectionHeader);
        }

        columns.push(...this.columnsHeaders);
        if (this.showActions) {
            columns.push(this.actionsHeader);
        }
        return columns;
    }

    dataSource: MatTableDataSource<any> | null = new MatTableDataSource();
    selection = new SelectionModel<any>(true, []);
    searchCtrl: FormControl;
    searchVisibility: boolean = false;

    @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: false }) sort: MatSort;

    constructor(
        private _fuseConfirmationService: FuseConfirmationService,
        private _matDialog: MatDialog
    ) {
        this.searchCtrl = new FormControl('');

        this.searchCtrl.valueChanges
            .pipe(debounceTime(800))
            .subscribe((value) => {
                this.onFilterChange(value);
            });
    }

    ngOnDestroy(): void {
        this.destroy$.next();
        this.destroy$.complete();
    }

    get visibleColumns() {
        return this.columns
            .filter((column) => column.visible)
            .map((column) => column.property);
    }

    get renderedRows(): Array<any> {
        let startIndex: number =
            this.paginator.pageSize * this.paginator.pageIndex;
        let endIndex: number = this.paginator.pageSize + startIndex;

        let array: Array<any> = [];

        if (this.dataSource.filteredData.length) {
            array = this.dataSource.filteredData;
        } else {
            array = this.dataSource.data;
        }
        return array.slice(startIndex, endIndex);
    }

    get hasData(): boolean {
        return this.dataSource?.filteredData?.length > 0;
    }

    updateItem(item: any) {
        if (!this.readOnly) {
            this.onUpdateItem.emit(item);
        } else {
            this.onReadOnlyPreviewItem.emit(item);
        }
    }

    deleteItem(item: any) {
        const dialogRef = this._fuseConfirmationService.open(
            this.removeDialogConfig()
        );
        dialogRef
            .afterClosed()
            .pipe(takeUntil(this.destroy$))
            .subscribe(async (result) => {
                if (result === 'confirmed') {
                    this.onDeleteItem.emit(item);
                }
            });
    }
    _unsubscribeAll(_unsubscribeAll: any): any {
        throw new Error('Method not implemented.');
    }
    selectedValue(selectedValue: any) {
        throw new Error('Method not implemented.');
    }
    getQueries() {
        throw new Error('Method not implemented.');
    }

    deleteItems(items: any[]) {
        this.onDeleteManyItems.emit(items);
    }

    customAction(item: any) {
        this.onCustomAction.emit(item);
    }

    onFilterChange(value: string) {
        if (!this.dataSource) {
            return;
        }
        this.filterValue.set(value.trim().toLowerCase());
        this.onSearchInputChange.emit({
            filterText: this.filterValue(),
            pageNumber: this.page().pageIndex + 1,
            pageSize: this.page().pageSize,
        });
    }

    toggleColumnVisibility(column, event) {
        event.stopPropagation();
        event.stopImmediatePropagation();
        column.visible = !column.visible;
    }
    toggleSearchVisibility() {
        this.searchVisibility = !this.searchVisibility;
    }

    /** Whether the number of selected elements matches the total number of rows. */
    isAllSelected() {
        const numSelected = this.selection.selected.length;
        const numRows = this.renderedRows.length;
        return numSelected === numRows;
    }

    /** Selects all rows if they are not all selected; otherwise clear selection. */
    masterToggle() {
        this.isAllSelected()
            ? (this.selection = new SelectionModel<any>(true, []))
            : this.renderedRows.forEach((row) => this.selection.select(row));
    }

    trackByProperty<T>(index: number, column: TableColumn<T>) {
        return column.property;
    }

    getEnumDescription(key, enumData: { key: number; value: string }[]) {
        const enumItem = enumData.find((item) => item.key === key);
        return enumItem.value;
    }

    exportSelectedHandler() {
        this.onExportSelectedItemsEvent.emit(this.selection.selected);
    }

    public pageChange(event?: PageEvent) {
        this.onPageChangedEvent.emit({
            filterText: this.filterValue(),
            pageNumber: event.pageIndex + 1,
            pageSize: event.pageSize,
        });
    }

    openPreviewDialog(src: any): void {
        if (!src || !this.showImageModal) return;
        this._matDialog.open(CardImagePreviewComponent, {
            autoFocus: false,
            data: {
                src: cloneDeep(src),
                useWaterMarks: this.useWaterMarks,
            },
        });
    }

    truncateText(text: string, maxLength: number): string {
        if (text.length > maxLength) {
            return text.substring(0, maxLength) + '...';
        }
        return text;
    }
}
