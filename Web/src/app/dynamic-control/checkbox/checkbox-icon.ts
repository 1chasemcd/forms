import { Component, computed, input } from '@angular/core';

@Component({
  selector: 'app-checkbox-icon',
  template: ` @if (checked()) {
    <svg
      xmlns="http://www.w3.org/2000/svg"
      fill="none"
      viewBox="0 0 24 24"
      stroke-width="3"
      class="stroke-white size-6"
    >
      <path stroke-linecap="round" stroke-linejoin="round" d="m4.5 12.75 6 6 9-13.5" />
    </svg>
  }`,
  host: {
    '[class]': '"flex h-5 w-5 items-center justify-center rounded border " + colorClass()',
  },
})
export class CheckboxIcon {
  checked = input.required<boolean>();
  colorClass = computed(() =>
    this.checked() ? 'bg-violet-600 border-violet-600' : 'border-gray-400',
  );
}
