import { Component, input, OnInit, signal } from '@angular/core';
import { FormControl, ReactiveFormsModule, Validators } from '@angular/forms';
import { CustomInputComponent } from '../../field-resolution/custom-field-registration';

@Component({
  selector: 'app-custom-checkbox',
  imports: [ReactiveFormsModule],
  templateUrl: './custom-checkbox.html',
})
export class CustomCheckbox implements OnInit, CustomInputComponent {
  readonly label = input.required<string>();
  readonly formControl = input.required<FormControl>();
  readonly isRequired = signal(false);

  get disabled() {
    return this.formControl().disabled;
  }

  ngOnInit() {
    this.formControl().statusChanges.subscribe(() =>
      this.isRequired.set(this.formControl().hasValidator(Validators.required)),
    );
  }
}
