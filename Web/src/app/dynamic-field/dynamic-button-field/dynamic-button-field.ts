import { Component, input, OnInit, signal } from '@angular/core';
import { ButtonField } from '../../api/api.g';
import { FormModel } from '../../dynamic-form/form-model';
import { CustomButton } from '../../custom-field/custom-button/custom-button';

@Component({
  selector: 'app-dynamic-button-field',
  imports: [CustomButton],
  template: `<app-custom-button
    [label]="label()"
    [disabled]="disabled()"
    [onClick]="onClick"
  ></app-custom-button>`,
})
export class DynamicButtonField implements OnInit {
  readonly button = input.required<ButtonField>();
  readonly model = input.required<FormModel>();
  readonly label = signal('');
  readonly disabled = signal(false);

  ngOnInit(): void {
    this.model().registerPocDependency(this.button().Label, this.label.set);
    this.model().registerPocDependency(this.button().Disabled, this.disabled.set);
  }

  onClick() {
    console.log('Button clicked, perform action');
  }
}
