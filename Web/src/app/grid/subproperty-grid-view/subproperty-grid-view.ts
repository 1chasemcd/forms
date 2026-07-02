import { Component, computed, inject, input, OnInit, signal } from '@angular/core';
import { FormArray, ReactiveFormsModule } from '@angular/forms';
import { SubPropertyGridView } from '../../api/api.g';
import { ControlPath, joinPath } from '../../utils/form-utils';
import { FormStackService } from '../../form/form-services/form-stack-service';
import { MatTableModule } from '@angular/material/table';
import { SubpropertyGridDataSource } from './subproperty-grid-data-source';
import { BaseTable } from '../base-table/base-table';
import { MetadataLookupService } from '../../metadata/metadata-lookup';
import { map, Observable, of, startWith } from 'rxjs';
import { SelectionChange } from '@angular/cdk/collections';

@Component({
  selector: 'app-subproperty-grid-view',
  imports: [MatTableModule, ReactiveFormsModule, BaseTable],
  templateUrl: './subproperty-grid-view.html',
  host: {
    class: 'col-span-12',
  },
})
export class SubpropertyGridViewComponent implements OnInit {
  readonly view = input.required<SubPropertyGridView>();
  readonly path = input.required<ControlPath>();

  private readonly formStack = inject(FormStackService);
  private readonly metadata = inject(MetadataLookupService);

  readonly modelType = signal<string>('');
  readonly arrayPath = computed(() => joinPath(this.path(), this.view().subProperty));
  readonly columns = computed(() => this.view().controls.map((x) => x.propertyName));
  readonly dataSource = computed(
    () => new SubpropertyGridDataSource(this.arrayPath(), this.formStack),
  );

  ngOnInit(): void {
    const arrayMetadata = this.metadata.lookupByPath(
      this.formStack.activeModel.root,
      this.arrayPath(),
    );
    if (arrayMetadata?.$type === 'enumerable') this.modelType.set(arrayMetadata.enumeratedType);
  }

  updateSelections(event: SelectionChange<unknown>) {
    const array = this.formStack.activeModel.get(this.arrayPath()) as FormArray;
    for (const id of event.added)
      array.controls.find((x) => x.get(this.view().idProperty) === id)?.setValue(true);
    for (const id of event.removed)
      array.controls.find((x) => x.get(this.view().idProperty) === id)?.setValue(false);
  }

  getSelectionState(): Observable<unknown[]> {
    const array = this.formStack.activeModel.get(this.arrayPath()) as FormArray;
    const selectionProp = this.view().gridSelectionOptions?.selectionProperty;
    if (!array || !selectionProp) return of([]);
    return array?.valueChanges.pipe(
      startWith(array.getRawValue()),
      map(() => {
        const rows = array.controls;
        const selected = rows
          .filter((r) => r.get(selectionProp))
          .map((r) => r.get(this.view().idProperty));

        return selected;
      }),
    );
  }
}
