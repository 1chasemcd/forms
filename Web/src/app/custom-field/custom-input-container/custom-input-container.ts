import { Component, computed, input, linkedSignal, Signal, signal } from '@angular/core';

@Component({
  selector: 'app-custom-input-container',
  imports: [],
  templateUrl: './custom-input-container.html',
})
export class CustomInputContainer {
  readonly label = input.required<string>();
  readonly required = linkedSignal(() => false);
  readonly inputId = signal<string>('');
  readonly requiredMark = computed(() => (this.required() ? ' *' : ''));

  registerInput(inputId: string, required: Signal<boolean>) {
    this.inputId.set(inputId);
    this.required.set(required());
  }
}
