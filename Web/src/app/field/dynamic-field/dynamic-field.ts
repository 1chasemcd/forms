import { Component, computed, input } from '@angular/core';
import { BaseField } from '../../api/api.g';
import { FormModel, isBaseInput, widthToCss } from '../../utils/api-model-utils';
import { ButtonField } from '../button-field/button-field';
import { InputField } from '../input-field/input-field';
import { ControlContainer, FormGroupDirective } from '@angular/forms';

@Component({
  selector: 'app-dynamic-field',
  host: {
    '[class]': 'width()',
  },
  templateUrl: './dynamic-field.html',
  imports: [ButtonField, InputField],
  viewProviders: [{ provide: ControlContainer, useExisting: FormGroupDirective }],
})
export class DynamicField {
  readonly field = input<BaseField>();
  readonly model = input<FormModel>();

  readonly width = computed(() => widthToCss(this.field()?.Width));

  readonly buttonField = computed(() => {
    const f = this.field();
    if (f?.$type == 'button') return f;
    return undefined;
  });

  readonly inputField = computed(() => {
    const f = this.field();
    if (isBaseInput(f)) return f;
    return undefined;
  });
}
