import { Component, computed, DestroyRef, inject, input, OnInit, signal } from '@angular/core';
import { Grid } from '@angular/aria/grid';
import { FormArray, FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { applyPropertyOrConstant, getLabel, getMetadata } from '../../utils/api-utils';
import { GridCell } from '../grid-cell/grid-cell';
import {
  FieldDefinition,
  GridSelectionType,
  MetadataType,
  SubPropertyGridViewDefinition,
} from '../../api/api.g';
import { CheckboxInput } from '../../dynamic-field/checkbox/checkbox-input';
import { FormContext } from '../../dynamic-form/form-context';

@Component({
  selector: 'app-subproperty-grid-view',
  imports: [Grid, ReactiveFormsModule, GridCell, CheckboxInput],
  templateUrl: './subproperty-grid-view.html',
  host: {
    class: 'col-span-12',
  },
})
export class SubpropertyGridViewComponent implements OnInit {
  readonly GridSelectionType = GridSelectionType;
  readonly gridView = input.required<SubPropertyGridViewDefinition>();
  readonly formContext = input.required<FormContext>();
  readonly labels: string[] = [];
  private destroyRef = inject(DestroyRef);
  readonly rowContexts = signal<FormContext[]>([]);
  private readonly formArray = computed(
    () => this.formContext().formGroup.get(this.gridView().subPropertyName) as FormArray<FormGroup>,
  );

  readonly selectAllControl = new FormControl(false);

  ngOnInit() {
    for (const field of this.gridView().fields) {
      const index = this.labels.push('') - 1;
      applyPropertyOrConstant(
        getLabel(field),
        this.formContext(),
        (label: string) => (this.labels[index] = label),
      );
    }

    this.formArray().valueChanges.subscribe(() =>
      this.rowContexts.set(
        this.formArray().controls.map((c) => new FormContext(this.destroyRef, c)),
      ),
    );
  }

  readonly gridTemplateColumns = computed(() => {
    let columns = this.columnSpans()
      .map((span) => `minmax(max-content, ${span}fr)`)
      .join(' ');

    if (this.gridView().selectionOptions) columns = 'max-content ' + columns;

    return columns;
  });

  private readonly columnSpans = computed(() => {
    const columns = this.gridView().fields;

    const explicit = columns.map((c) => this.getFieldWidth(c));
    const definedTotal = explicit.reduce((sum, w) => (sum ?? 0) + (w ?? 0), 0) ?? 0;

    const undefinedCount = explicit.filter((w) => w == null).length;

    const remaining = Math.max(12 - definedTotal, 0);

    let autoWidth = undefinedCount > 0 ? remaining / undefinedCount : 0;
    autoWidth = Math.max(autoWidth, 1);

    return columns.map((c, i) => {
      const w = explicit[i];
      return w ?? autoWidth;
    });
  });

  getFieldWidth(field: FieldDefinition) {
    return getMetadata<number>(field, MetadataType.Width);
  }

  getRowId(rowContext: FormContext) {
    return rowContext.formGroup.get(this.gridView().idProperty)?.value;
  }

  getControlFromRow(rowContext: FormContext, propertyName: string) {
    return rowContext.formGroup.get(propertyName) as FormControl;
  }

  selectionPropertyUpdated(rowContext: FormContext) {
    if (this.gridView().selectionOptions?.selectionType == GridSelectionType.Single) {
      if (!this.getSelectionControl(rowContext).value) return;
      this.unselectAllOthers(this.getRowId(rowContext));
    } else if (this.gridView().selectionOptions?.selectionType == GridSelectionType.Multiple) {
      this.updateSelectAllState();
    }
  }

  selectAllUpdated() {
    const value = this.selectAllControl.value;
    for (const context of this.rowContexts()) {
      this.getSelectionControl(context).setValue(value);
    }
  }

  private unselectAllOthers(idToKeep: unknown) {
    for (const context of this.rowContexts()) {
      if (this.getRowId(context) === idToKeep) continue;
      this.getSelectionControl(context).setValue(false);
    }
  }

  private updateSelectAllState() {
    let allSelected = true;
    for (const context of this.rowContexts()) {
      const value = this.getSelectionControl(context).value;
      if (!value) allSelected = false;
    }
    if (allSelected) this.selectAllControl.setValue(true);
    else this.selectAllControl.setValue(false);
  }

  private getSelectionControl(rowContext: FormContext) {
    const selectionProperty = this.gridView().selectionOptions?.selectionProperty ?? '';
    return this.getControlFromRow(rowContext, selectionProperty);
  }
}
