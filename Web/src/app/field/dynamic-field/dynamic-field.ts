import { Component, computed, input } from '@angular/core';
import { BaseField } from '../../api/api.g';
import { computedPropertyOrConstant, FormModel, widthToCss } from '../../utils/api-model-utils';

@Component({
  selector: 'app-dynamic-field',
  host: {
    '[class]': 'width()',
  },
  templateUrl: './dynamic-field.html',
})
export class DynamicField {
  readonly field = input<BaseField>();
  readonly model = input<FormModel>();

  readonly label = computedPropertyOrConstant(
    computed(() => this.field()?.Label),
    this.model,
  );

  readonly width = computed(() => widthToCss(this.field()?.Width));
}
