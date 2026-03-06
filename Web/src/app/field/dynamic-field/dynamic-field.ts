import { Component, computed, input } from '@angular/core';
import { BaseField } from '../../api/api.g';
import { FormModel } from '../../dynamic-form/form-model';

@Component({
  selector: 'app-dynamic-field',
  imports: [],
  templateUrl: './dynamic-field.html',
})
export class DynamicField {
  readonly field = input<BaseField>();
  readonly model = input<FormModel>();

  readonly label = computed(() => {
    const label = this.field()?.Label;
    if (label?.$type == 'constant') return label.Value;
    if (label?.$type == 'property' && label.Value) {
      const model = this.model();
      if (model) return model[label.Value];
    }
    return '';
  });
}
