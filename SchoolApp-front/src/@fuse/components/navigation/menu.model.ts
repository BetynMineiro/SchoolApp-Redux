export interface Menu {
    name: string;
    subItems: Menu[];
    profile: string[];
    icon: string;
    goTo: string;
    classes: {
        title?: string;
        icon?: string;
        wrapper?: string;
        subtitle?: string;
    };
}
