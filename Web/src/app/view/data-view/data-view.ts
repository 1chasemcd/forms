import { Component, input } from '@angular/core';
import { DataView } from '../../api/api.g';

@Component({
  selector: 'app-data-view',
  imports: [],
  templateUrl: './data-view.html',
})
export class AppDataView {
  readonly dataView = input<DataView>();
}
