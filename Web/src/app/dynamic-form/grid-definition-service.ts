import { Injectable } from '@angular/core';
import { FormGroup } from '@angular/forms';

@Injectable()
export class GridDefinitionService {
  private gridRowDefinitions: Record<string, () => FormGroup> = {};

  registerDefinition(gridId: string, callback: () => FormGroup) {
    this.gridRowDefinitions[gridId] = callback;
  }

  getNewGridRow(gridId: string) {
    const def = this.gridRowDefinitions[gridId];
    if (def) return def();
    return null;
  }
}
