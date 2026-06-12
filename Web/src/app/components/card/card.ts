import { Component } from '@angular/core';

@Component({
  selector: 'app-card',
  imports: [],
  template: '<ng-content/>',
  host: {
    class: 'flex content-center rounded-lg shadow bg-white',
  },
})
export class Card {}
