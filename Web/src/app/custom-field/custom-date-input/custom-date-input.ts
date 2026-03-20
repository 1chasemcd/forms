import { Component, computed, input } from '@angular/core';
import { CustomInputComponent } from '../../field-resolution/custom-field-registration';
import { FormControl, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-custom-date-input',
  imports: [ReactiveFormsModule],
  templateUrl: './custom-date-input.html',
})
export class CustomDateInput implements CustomInputComponent {
  formControl = input.required<FormControl>();
  readonly label = input.required<string>();
  readonly isRequired = input<boolean>();

  readonly requiredMark = computed(() => (this.isRequired() ? ' *' : ''));

  private static _nextId = 0;
  readonly id = `date-input-${CustomDateInput._nextId++}`;
}
