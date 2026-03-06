import { Component, input } from '@angular/core';
import { DataView } from '../../api/api.g';
import { FormModel } from '../../dynamic-form/form-model';
import { DynamicField } from '../../field/dynamic-field/dynamic-field';

@Component({
  selector: 'app-data-view',
  imports: [DynamicField],
  templateUrl: './data-view.html',
})
export class AppDataView {
  readonly dataView = input<DataView>();
  readonly model = input<FormModel>();
}
