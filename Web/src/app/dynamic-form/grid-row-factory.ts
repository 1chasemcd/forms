import { DestroyRef, inject, Injectable } from '@angular/core';
import { SubPropertyGridViewDefinition } from '../api/api.g';
import { FormProcessorService } from '../form-processor/form-processor-service';
import { FormGroup } from '@angular/forms';
import { FormContext } from './form-context';

@Injectable({ providedIn: 'root' })
export class GridRowFactory {
  private processorService = inject(FormProcessorService);
  private destroyRef = inject(DestroyRef);

  private definitions = new Map<
    string,
    { definition: SubPropertyGridViewDefinition; parentContext: FormContext }
  >();

  createGridRow(id: string): FormContext | null {
    const entry = this.definitions.get(id);
    if (!entry) return null;
    // const gridView = entry.definition;

    const group = new FormGroup({});
    const context = new FormContext(group, this.destroyRef, entry.parentContext);
    entry.definition.fields.forEach((f) => this.processorService.processField(f, context));

    // let gridEnabled = of(true);
    // if (gridView.canEdit?.$type === 'constant' && !gridView.canEdit.value) gridEnabled = of(false);
    // else if (gridView.canEdit?.$type === 'property') {
    //   const control = context.getOrAddControl(gridView.canEdit.value);
    //   gridEnabled = combineLatest([
    //     of(true),
    //     control.valueChanges.pipe(startWith(control.value)),
    //   ]).pipe(map(([p, c]) => p && c));
    // }

    return context;
  }

  register(id: string, definition: SubPropertyGridViewDefinition, parentContext: FormContext) {
    this.definitions.set(id, { definition, parentContext });
  }

  clear() {
    this.definitions.clear();
  }
}
