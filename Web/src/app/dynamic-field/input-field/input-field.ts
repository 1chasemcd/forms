import { Component, computed, input, OnInit, signal } from '@angular/core';
import { BaseInput } from '../../api/api.g';
import { ControlContainer, FormGroupDirective, ReactiveFormsModule } from '@angular/forms';
import { FormModel } from '../../dynamic-form/form-model';

@Component({
  selector: 'app-obsolete-input-field',
  imports: [ReactiveFormsModule],
  templateUrl: './input-field.html',
  viewProviders: [{ provide: ControlContainer, useExisting: FormGroupDirective }],
})
export class ObsoleteInputField implements OnInit {
  readonly baseInput = input.required<BaseInput>();
  readonly model = input.required<FormModel>();

  readonly label = signal('');

  ngOnInit(): void {
    this.model().registerPocDependency(this.baseInput().Label, this.label.set);
  }

  readonly inputType = computed(() => {
    switch (this.baseInput().$type) {
      case 'currencyinput':
      case 'numericinput':
        return 'number';
      case 'dateinput':
        return 'date';
      case 'timeinput':
        return 'time';
      case 'textinput':
      default:
        return 'text';
    }
  });
}
