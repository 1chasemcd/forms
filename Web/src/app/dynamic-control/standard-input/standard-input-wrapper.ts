import { Component, computed, input, linkedSignal, Signal, signal } from '@angular/core';

@Component({
  selector: 'app-standard-input-wrapper',
  imports: [],
  templateUrl: './standard-input-wrapper.html',
})
export class StandardInputWrapper {
  readonly label = input.required<string>();
  readonly required = linkedSignal(() => false);
  readonly inputId = signal<string>('');
  readonly requiredMark = computed(() => (this.required() ? ' *' : ''));

  registerInput(inputId: string, required: Signal<boolean>) {
    this.inputId.set(inputId);
    this.required.set(required());
  }
}
