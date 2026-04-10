import { DestroyRef, inject, Injectable } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { BaseViewDefinition, FormDefinition, SubPropertyGridViewDefinition } from '../api/api.g';
import { FormProcessorService } from './form-processor-service';
import { FormContext } from './form-context';
import { Observable, of } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class FormFactory {
  private processorService = inject(FormProcessorService);
  private gridRegistry = inject(GridRegistry);
  private destroyRef = inject(DestroyRef);

  createFormGroup(
    view: BaseViewDefinition,
    formDefinition: FormDefinition,
    parentEnabled: Observable<boolean> = of(true),
  ): FormGroup {
    const group = new FormGroup({});
    const context = new FormContext(group, formDefinition, this.destroyRef);
    this.processorService.processView(view, group, context, parentEnabled);
    return group;
  }

  createGridRowGroup(gridId: string, formDefinition: FormDefinition): FormGroup | null {
    const entry = this.gridRegistry.get(gridId);
    if (!entry) return null;

    const group = new FormGroup({});
    const context = new FormContext(group, formDefinition, this.destroyRef);

    if (entry.definition.canEditRow?.$type === 'constant' && !entry.definition.canEditRow.value) {
      // Parent enablement can be combined with row enablement if we have a way to resolve it
    }

    this.processorService.processView(entry.definition, group, context, entry.parentEnabled);
    return group;
  }
}

@Injectable({ providedIn: 'root' })
export class GridRegistry {
  private grids = new Map<
    string,
    { definition: SubPropertyGridViewDefinition; parentEnabled: Observable<boolean> }
  >();

  register(
    gridId: string,
    definition: SubPropertyGridViewDefinition,
    parentEnabled: Observable<boolean>,
  ) {
    this.grids.set(gridId, { definition, parentEnabled });
  }

  get(gridId: string) {
    return this.grids.get(gridId);
  }

  clear() {
    this.grids.clear();
  }
}
