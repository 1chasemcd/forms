import { Component, computed, inject, input, OnInit, signal, Type } from '@angular/core';
import { BaseInput } from '../../api/api.g';
import { FormModel } from '../../dynamic-form/form-model';
import {
  ControlContainer,
  FormControl,
  FormGroupDirective,
  ReactiveFormsModule,
} from '@angular/forms';
import { CUSTOM_FIELDS } from '../../field-resolution/custom-field-provider';
import { CustomInputComponent } from '../../field-resolution/custom-field-registration';
import { NgComponentOutlet } from '@angular/common';

@Component({
  selector: 'app-dynamic-input-field',
  imports: [ReactiveFormsModule, NgComponentOutlet],
  template: `
    @if (inputComponent(); as component) {
      <ng-container
        *ngComponentOutlet="component; inputs: { label: label(), formControl: control() }"
      >
      </ng-container>
    }
  `,
  viewProviders: [{ provide: ControlContainer, useExisting: FormGroupDirective }],
})
export class DynamicInputField implements OnInit {
  readonly baseInput = input.required<BaseInput>();
  readonly model = input.required<FormModel>();
  private parentForm = inject(ControlContainer) as FormGroupDirective;
  private registry = inject(CUSTOM_FIELDS);

  readonly label = signal('');
  readonly control = computed(
    () => this.parentForm.control.get(this.baseInput().Property) as FormControl,
  );

  inputComponent = computed(() => {
    return this.registry.find((r) => r.type === this.baseInput().$type)
      ?.component as Type<CustomInputComponent>;
  });

  ngOnInit(): void {
    const i = this.baseInput();
    this.model().registerPocDependency(i.Label, this.label.set);
  }
}
