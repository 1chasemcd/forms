import { Component, computed, forwardRef, inject, input, OnInit, signal } from '@angular/core';
import { BaseInput } from '../../api/api.g';
import { FormModel } from '../../dynamic-form/form-model';
import {
  ControlContainer,
  ControlValueAccessor,
  FormControl,
  FormGroupDirective,
  NG_VALUE_ACCESSOR,
  ReactiveFormsModule,
} from '@angular/forms';
import { CustomCheckbox } from '../../custom-field/custom-checkbox/custom-checkbox';
import { ObsoleteInputField } from '../input-field/input-field';

@Component({
  selector: 'app-dynamic-input-field',
  imports: [CustomCheckbox, ObsoleteInputField, ReactiveFormsModule],
  templateUrl: './dynamic-input-field.html',
  viewProviders: [{ provide: ControlContainer, useExisting: FormGroupDirective }],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => DynamicInputField),
      multi: true,
    },
  ],
})
export class DynamicInputField implements OnInit, ControlValueAccessor {
  readonly value = signal('');
  readonly disabled = signal(false);
  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  onChange = (_: string) => {};
  onTouched = () => {};

  writeValue(value: string): void {
    this.value.set(value);
  }
  registerOnChange(fn: (_: string) => void): void {
    this.onChange = fn;
  }
  registerOnTouched(fn: () => void): void {
    this.onTouched = fn;
  }
  setDisabledState(isDisabled: boolean): void {
    this.disabled.set(isDisabled);
  }

  readonly baseInput = input.required<BaseInput>();
  readonly model = input.required<FormModel>();
  private parentForm = inject(ControlContainer) as FormGroupDirective;

  readonly label = signal('');
  readonly control = computed(
    () => this.parentForm.control.get(this.baseInput().Property) as FormControl,
  );

  ngOnInit(): void {
    const i = this.baseInput();
    this.model().registerPocDependency(i.Label, this.label.set);
  }
}
