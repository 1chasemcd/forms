import { Component, computed, input } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { CustomInputComponent } from '../../field-resolution/custom-field-registration';

@Component({
  selector: 'app-custom-text-area-input',
  imports: [ReactiveFormsModule],
  templateUrl: './custom-text-area-input.html',
})
export class CustomTextAreaInput implements CustomInputComponent {
  formControl = input.required<FormControl>();
  readonly label = input.required<string>();
  readonly isRequired = input<boolean>();

  readonly requiredMark = computed(() => (this.isRequired() ? ' *' : ''));

  private static _nextId = 0;
  readonly id = `input-${CustomTextAreaInput._nextId++}`;
}
