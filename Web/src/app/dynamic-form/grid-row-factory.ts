import { DestroyRef, inject, Injectable } from '@angular/core';
import { SubPropertyGridViewDefinition } from '../api/api.g';
import { FormProcessorService } from '../form-processor/form-processor-service';
import { FormContext } from './form-context';
import { FormFieldEnablementService } from '../form-processor/form-field-enablement-service';
import { getPocObservable } from '../utils/api-utils';

@Injectable({ providedIn: 'root' })
export class GridRowFactory {
  private processorService = inject(FormProcessorService);
  private enablementService = inject(FormFieldEnablementService);

  private destroyRef = inject(DestroyRef);

  private definitions = new Map<
    string,
    { definition: SubPropertyGridViewDefinition; parentContext: FormContext }
  >();

  createGridRow(id: string): FormContext | null {
    const entry = this.definitions.get(id);
    if (!entry) return null;
    const gridView = entry.definition;

    const context = new FormContext(this.destroyRef);
    this.enablementService.enabledForParent(context, gridView);

    if (!gridView.editForm && gridView.canEditRow)
      this.enablementService.enabledFor(context, getPocObservable(gridView.canEditRow, context));

    gridView.fields.forEach((f) => {
      const fieldControl = this.processorService.processField(f, context);
      if (fieldControl) this.enablementService.enabledForParent(fieldControl, context);
    });

    return context;
  }

  register(id: string, definition: SubPropertyGridViewDefinition, parentContext: FormContext) {
    this.definitions.set(id, { definition, parentContext });
  }

  clear() {
    this.definitions.clear();
  }
}
