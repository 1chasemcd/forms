import { Component, computed, input } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { CustomInputComponent } from '../../field-resolution/custom-field-registration';

@Component({
  selector: 'app-custom-time-input',
  imports: [ReactiveFormsModule],
  templateUrl: './custom-time-input.html',
})
export class CustomTimeInput implements CustomInputComponent {
  formControl = input.required<FormControl>();
  readonly label = input.required<string>();
  readonly isRequired = input<boolean>();

  readonly requiredMark = computed(() => (this.isRequired() ? ' *' : ''));

  private static _nextId = 0;
  readonly id = `time-input-${CustomTimeInput._nextId++}`;
}
