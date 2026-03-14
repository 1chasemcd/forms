import { Component, input } from '@angular/core';

@Component({
  selector: 'app-static-text-field',
  imports: [],
  templateUrl: './static-text-field.html',
})
export class StaticTextField {
  readonly text = input.required<string>();
}
