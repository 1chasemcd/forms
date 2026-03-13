import { Component, input, OnInit, signal } from '@angular/core';
import { CheckBoxInput } from '../../api/api.g';
import { ControlContainer, FormGroupDirective, ReactiveFormsModule } from '@angular/forms';
import { FormModel } from '../../dynamic-form/form-model';

@Component({
  selector: 'app-checkbox-field',
  imports: [ReactiveFormsModule],
  templateUrl: './checkbox-field.html',
  viewProviders: [{ provide: ControlContainer, useExisting: FormGroupDirective }],
})
export class CheckboxField implements OnInit {
  readonly checkboxInput = input.required<CheckBoxInput>();
  readonly model = input.required<FormModel>();

  readonly label = signal('');

  ngOnInit(): void {
    this.model().registerPocDependency(this.checkboxInput().Label, this.label.set);
  }
}
