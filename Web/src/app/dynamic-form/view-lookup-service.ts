import { Injectable } from '@angular/core';
import { View } from '../api/api.g';

@Injectable()
export class ViewLookupService {
  private views: View[] = [];

  initialize(views: View[]) {
    this.views = views;
  }

  lookupById(id: number) {
    if (id < 0 || id > this.views.length) return undefined;
    return this.views[id];
  }
}
