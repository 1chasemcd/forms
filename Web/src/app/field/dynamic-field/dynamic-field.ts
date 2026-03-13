import { Component, computed, input } from '@angular/core';
import { BaseField } from '../../api/api.g';
import { widthToCss } from '../../utils/width-utils';
import { ButtonField } from '../button-field/button-field';
import { ControlContainer, FormGroupDirective } from '@angular/forms';
import { FormModel } from '../../dynamic-form/form-model';
import { DynamicInputField } from '../dynamic-input-field/dynamic-input-field';
import { isBaseInput } from '../../utils/api-utils';

@Component({
  selector: 'app-dynamic-field',
  host: {
    '[class]': 'width() + " h-10 content-center"',
  },
  templateUrl: './dynamic-field.html',
  imports: [ButtonField, DynamicInputField],
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

  readonly staticTextField = computed(() => {
    const f = this.field();
    return f.$type === 'statictextfield' ? f : null;
  });

  readonly inputField = computed(() => {
    const f = this.field();
    return isBaseInput(f) ? f : null;
  });
}
