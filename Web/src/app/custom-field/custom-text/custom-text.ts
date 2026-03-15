import { Component, input } from '@angular/core';

@Component({
  selector: 'app-custom-text',
  imports: [],
  templateUrl: './custom-text.html',
})
export class CustomText {
  readonly text = input.required<string>();
}
