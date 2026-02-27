import { Component, input } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { DataView } from '../../api/api.g';

@Component({
  selector: 'app-data-view',
  imports: [],
  templateUrl: './data-view.html',
})
export class AppDataView {
  readonly group = input<FormGroup>();
  readonly formDataView = input<DataView>();
}
