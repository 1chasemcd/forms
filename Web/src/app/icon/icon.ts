import { Component, input } from '@angular/core';

@Component({
  selector: 'app-icon',
  template: `
    <svg viewBox="0 0 24 24" class="aspect-square size-6">
      <use [attr.href]="'public/icons.svg#' + name()"></use>
    </svg>
  `,
  host: {
    class: 'inline-flex',
  },
})
export class Icon {
  name = input.required<string>();
}
