import {
    ChangeDetectionStrategy,
    Component,
    input,
    ViewEncapsulation,
} from '@angular/core';

@Component({
    selector: 'maintenance',
    templateUrl: './maintenance.component.html',
    encapsulation: ViewEncapsulation.None,
    changeDetection: ChangeDetectionStrategy.OnPush,
    standalone: true,
})
export class MaintenanceComponent {
    /**
     * Constructor
     */
    title = input<string>('We are under scheduled maintenance.');
    subTitle = input<string>(
        'Sorry for the inconvenience, we will be back shortly!'
    );
    constructor() {}
}
