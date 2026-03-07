import { Component, computed, input } from '@angular/core';
import { CheckBoxInput } from '../../api/api.g';
import { computedPropertyOrConstant, FormModel } from '../../utils/api-model-utils';
import { ControlContainer, FormGroupDirective, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-checkbox-field',
  imports: [ReactiveFormsModule],
  templateUrl: './checkbox-field.html',
  viewProviders: [{ provide: ControlContainer, useExisting: FormGroupDirective }],
})
export class CheckboxField {
  readonly checkboxInput = input<CheckBoxInput>();
  readonly model = input<FormModel>();

  readonly label = computedPropertyOrConstant(
    computed(() => this.checkboxInput()?.Label),
    this.model,
  );
}
