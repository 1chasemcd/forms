import { Component, input, OnInit, signal } from '@angular/core';
import { ButtonField } from '../../api/api.g';
import { FormModel } from '../../dynamic-form/form-model';

@Component({
  selector: 'app-button-field',
  imports: [],
  templateUrl: './button-field.html',
})
export class ButtonComponent implements OnInit {
  readonly button = input.required<ButtonField>();
  readonly model = input.required<FormModel>();
  readonly label = signal('');

  ngOnInit(): void {
    this.model().registerPocDependency(this.button().Label, this.label.set);
  }

  onClick() {
    console.log('Button clicked, perform action');
  }
}
