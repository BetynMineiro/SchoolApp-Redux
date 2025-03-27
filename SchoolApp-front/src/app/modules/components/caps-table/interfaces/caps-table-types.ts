export interface TableColumn<T> {
    label: string;
    property: keyof T | string;
    propertyArray?: keyof T[] | string[];
    type:
        | 'text'
        | 'date'
        | 'image'
        | 'imageList'
        | 'checkbox'
        | 'button'
        | 'boolean'
        | 'array'
        | 'currency'
        | 'enum'
        | 'html';
    visible?: boolean;
    enumData?: { key?: number; value?: string; data?: any }[];
    cssClasses?: string[];
    cssDetailClass?: string[];
}

export interface RefreshDataTable {
    filterText?: string;
    pageNumber: number;
    pageSize: number;
}
