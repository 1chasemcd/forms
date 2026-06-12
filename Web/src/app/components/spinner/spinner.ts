import { Component, computed, input } from '@angular/core';

@Component({
  selector: 'app-spinner',
  imports: [],
  template: '',
  host: {
    class: 'border-gray-200 border-t-violet-600 rounded-full animate-spin',
    '[style.width.px]': 'dimension()',
    '[style.height.px]': 'dimension()',
    '[style.border-width.px]': 'border()',
  },
})
export class Spinner {
  readonly size = input<'s' | 'l'>('l');
  readonly dimension = computed(() => (this.size() === 's' ? 32 : 64));
  readonly border = computed(() => (this.size() === 's' ? 6 : 8));
}
