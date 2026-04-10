import { Injectable } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { SubPropertyGridViewDefinition } from '../api/api.g';
import { Observable } from 'rxjs';

@Injectable()
export class GridDefinitionService {
  private gridRowDefinitions: Record<string, SubPropertyGridViewDefinition> = {};
  private rowCreationCallbacks: Record<
    string,
    (def: SubPropertyGridViewDefinition, parentEnabled: Observable<boolean>) => FormGroup
  > = {};
  private parentEnablements: Record<string, Observable<boolean>> = {};

  registerDefinition(
    gridDefinition: SubPropertyGridViewDefinition,
    parentEnabled: Observable<boolean>,
    callback: (def: SubPropertyGridViewDefinition, parentEnabled: Observable<boolean>) => FormGroup,
  ) {
    this.gridRowDefinitions[gridDefinition.subPropertyName] = gridDefinition;
    this.parentEnablements[gridDefinition.subPropertyName] = parentEnabled;
    this.rowCreationCallbacks[gridDefinition.subPropertyName] = callback;
  }

  getNewGridRow(gridId: string) {
    const def = this.gridRowDefinitions[gridId];
    const callback = this.rowCreationCallbacks[gridId];
    if (def && callback) return callback(def, this.parentEnablements[gridId]);
    return null;
  }
}
