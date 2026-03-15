import { Component, input, output } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-custom-input',
  imports: [ReactiveFormsModule],
  templateUrl: './custom-input.html',
})
export class CustomInput {
  readonly label = input.required<string>();
  readonly displayValue = input.required<string>();
  readonly isDisabled = input<boolean>();
  readonly isRequired = input<boolean>();
  readonly valueChange = output<string>();
  readonly focusOn = output();
  readonly focusOff = output();

  private static nextId = 0;
  readonly id = `input-${CustomInput.nextId++}`;

  handleInput(event: Event) {
    const value = (event.target as HTMLInputElement).value;
    this.valueChange.emit(value);
  }
}
