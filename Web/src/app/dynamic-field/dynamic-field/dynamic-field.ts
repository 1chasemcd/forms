import { Component, computed, input, OnInit, signal } from '@angular/core';
import { BaseField } from '../../api/api.g';
import { widthToCss } from '../../utils/width-utils';
import { ControlContainer, FormGroupDirective } from '@angular/forms';
import { FormModel } from '../../dynamic-form/form-model';
import { DynamicInputField } from '../dynamic-input-field/dynamic-input-field';
import { isBaseInput } from '../../utils/api-utils';
import { StaticTextField } from '../../field/static-text-field/static-text-field';
import { DynamicButtonField } from '../dynamic-button-field/dynamic-button-field';

@Component({
  selector: 'app-dynamic-field',
  host: {
    '[class]': 'width() + " h-10 content-center"',
  },
  templateUrl: './dynamic-field.html',
  imports: [DynamicInputField, StaticTextField, DynamicButtonField],
  viewProviders: [{ provide: ControlContainer, useExisting: FormGroupDirective }],
})
export class DynamicField implements OnInit {
  readonly field = input.required<BaseField>();
  readonly model = input.required<FormModel>();

  readonly width = computed(() => widthToCss(this.field().Width));
  readonly label = signal('');

  ngOnInit() {
    this.model().registerPocDependency(this.field().Label, this.label.set);
  }

  readonly buttonField = computed(() => {
    const f = this.field();
    return f.$type === 'buttonfield' ? f : null;
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
