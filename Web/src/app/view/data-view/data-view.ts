import { Component, input } from '@angular/core';
import { FormDataView } from '../../api/types';
import { FormGroup } from '@angular/forms';

@Component({
  selector: 'app-data-view',
  imports: [],
  templateUrl: './data-view.html',
})
export class DataView {
  readonly group = input<FormGroup>();
  readonly formDataView = input<FormDataView>();
}
