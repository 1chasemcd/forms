import { Component, input, OnInit, signal } from '@angular/core';
import { ButtonInput } from '../../api/api.g';
import { ControlContainer, FormGroupDirective } from '@angular/forms';
import { FormModel } from '../../dynamic-form/form-model';

@Component({
  selector: 'app-button-field',
  imports: [],
  templateUrl: './button-field.html',
  viewProviders: [{ provide: ControlContainer, useExisting: FormGroupDirective }],
})
export class ButtonField implements OnInit {
  readonly button = input.required<ButtonInput>();
  readonly model = input.required<FormModel>();

  readonly label = signal('');

  ngOnInit(): void {
    this.model().registerPocDependency(this.button().Label, this.label.set);
  }

  onClick() {
    console.log('Button clicked, perform action');
  }
}
