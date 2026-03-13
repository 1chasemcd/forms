import { Component, computed, inject, input, OnInit, signal } from '@angular/core';
import { BaseInput } from '../../api/api.g';
import { FormModel } from '../../dynamic-form/form-model';
import { ControlContainer, FormControl, FormGroupDirective } from '@angular/forms';
import { CheckboxField } from '../checkbox-field/checkbox-field';
import { InputField } from '../input-field/input-field';

@Component({
  selector: 'app-dynamic-input-field',
  imports: [CheckboxField, InputField],
  templateUrl: './dynamic-input-field.html',
  viewProviders: [{ provide: ControlContainer, useExisting: FormGroupDirective }],
})
export class DynamicInputField implements OnInit {
  readonly baseInput = input.required<BaseInput>();
  readonly model = input.required<FormModel>();
  private parentForm = inject(ControlContainer) as FormGroupDirective;

  readonly label = signal('');
  readonly control = computed(
    () => this.parentForm.control.get(this.baseInput().Id) as FormControl,
  );

  ngOnInit(): void {
    this.model().registerPocDependency(this.baseInput().Label, this.label.set);
  }
}
