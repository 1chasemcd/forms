import { Component, input } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { CustomInputComponent } from '../../field-resolution/custom-field-registration';
import { CustomInputContainer } from '../custom-input-container/custom-input-container';
import { CustomInputDirective } from '../custom-input-directive';

@Component({
  selector: 'app-custom-textarea',
  imports: [ReactiveFormsModule, CustomInputContainer, CustomInputDirective],
  template: `<app-custom-input-container [label]="label()">
    <textarea rows="3" [formControl]="formControl()" appCustomInputDirective></textarea>
  </app-custom-input-container>`,
})
export class CustomTextArea implements CustomInputComponent {
  label = input.required<string>();
  formControl = input.required<FormControl>();
}
