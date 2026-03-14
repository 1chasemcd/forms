import { Component, input, OnInit, signal } from '@angular/core';
import { ButtonField } from '../../api/api.g';
import { FormModel } from '../../dynamic-form/form-model';
import { ButtonComponent } from '../../field/button-field/button-field';

@Component({
  selector: 'app-dynamic-button-field',
  imports: [ButtonComponent],
  template: `<app-button-field
    [label]="label()"
    [disabled]="disabled()"
    [onClick]="onClick"
  ></app-button-field>`,
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
