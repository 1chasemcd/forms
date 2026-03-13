import { Component, computed, input } from '@angular/core';
import { BaseField } from '../../api/api.g';
import { widthToCss } from '../../utils/width-utils';
import { ButtonField } from '../button-field/button-field';
import { InputField } from '../input-field/input-field';
import { ControlContainer, FormGroupDirective } from '@angular/forms';
import { CheckboxField } from '../checkbox-field/checkbox-field';
import { FormModel } from '../../dynamic-form/form-model';

@Component({
  selector: 'app-dynamic-field',
  host: {
    '[class]': 'width() + " h-10 content-center"',
  },
  templateUrl: './dynamic-field.html',
  imports: [ButtonField, InputField, CheckboxField],
  viewProviders: [{ provide: ControlContainer, useExisting: FormGroupDirective }],
})
export class DynamicField {
  readonly field = input.required<BaseField>();
  readonly model = input.required<FormModel>();

  readonly width = computed(() => widthToCss(this.field().Width));

  readonly buttonField = computed(() => {
    const f = this.field();
    return f.$type === 'buttoninput' ? f : null;
  });

  readonly inputField = computed(() => {
    const f = this.field();
    switch (f.$type) {
      case 'currencyinput':
      case 'dateinput':
      case 'timeinput':
      case 'numericinput':
      case 'textinput':
        return f;
      default:
        return null;
    }
  });

  readonly checkboxField = computed(() => {
    const f = this.field();
    return f.$type == 'checkboxinput' ? f : null;
  });
}
