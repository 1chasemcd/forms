import { Component, computed, inject, input, OnInit } from '@angular/core';
import { Grid } from '@angular/aria/grid';
import { FormArray, FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { GridCell } from '../grid-cell/grid-cell';
import { GridSelectionType, SubPropertyGridView } from '../../api/api.g';
import { CheckboxInput } from '../../dynamic-control/checkbox/checkbox-input';
import { ControlPath } from '../../utils/form-utils';
import { FormModelService } from '../../dynamic-form/form-model-service';
import { PropertyOrConstantEvaluationService } from '../../dynamic-form/property-or-constant-evaluation-service';
import { MetadataLookupService } from '../../metadata/metadata-lookup-service';

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

  readonly view = input.required<SubPropertyGridView>();
  readonly modelPath = input.required<ControlPath>();

  private readonly modelService = inject(FormModelService);
  private readonly pocEvaluator = inject(PropertyOrConstantEvaluationService);
  private readonly metadataLookup = inject(MetadataLookupService);

  readonly arrayPath = computed(() => [...this.modelPath(), this.view().subProperty]);
  readonly labels: string[] = [];
  readonly rows = computed(() => this.modelService.get<FormArray<FormGroup>>(this.arrayPath()));

  readonly selectAllControl = new FormControl(false);

  ngOnInit() {
    for (const controlInfo of this.view().controls) {
      const index = this.labels.push('') - 1;
      const controlPath = [...this.arrayPath(), controlInfo.propertyName];
      this.pocEvaluator
        .observe<string>(this.metadataLookup.getLabelMetadata(controlPath), this.modelPath())
        .subscribe((l) => (this.labels[index] = l));
    }
  }

  readonly gridTemplateColumns = computed(() => {
    let columns = this.columnSpans()
      .map((span) => `minmax(max-content, ${span}fr)`)
      .join(' ');

    if (this.view().gridSelectionOptions) columns = 'max-content ' + columns;

    return columns;
  });

  private readonly columnSpans = computed(() => {
    const columns = this.view().controls;

    const explicit = columns.map((c) => c.width);
    const definedTotal = explicit.reduce((sum, w) => (sum ?? 0) + (w ?? 0), 0) ?? 0;

    const undefinedCount = explicit.filter((w) => w == undefined).length;

    const remaining = Math.max(12 - definedTotal, 0);

    let autoWidth = undefinedCount > 0 ? remaining / undefinedCount : 0;
    autoWidth = Math.max(autoWidth, 1);

    return columns.map((c, i) => {
      const w = explicit[i];
      return w ?? autoWidth;
    });
  });

  getRowId(row: FormGroup) {
    return row.get(this.view().idProperty)?.value;
  }

  private getRowIndex(row: FormGroup) {
    const id = this.getRowId(row);
    return this.rows().controls.findIndex((x) => this.getRowId(x) == id);
  }

  createRowPath(row: FormGroup) {
    return computed(() => [...this.arrayPath(), this.getRowIndex(row)]);
  }

  getControlFromRow(row: FormGroup, propertyName: string) {
    return row.get(propertyName) as FormControl;
  }

  selectionPropertyUpdated(row: FormGroup) {
    if (this.view().gridSelectionOptions?.selectionType == GridSelectionType.Single) {
      if (!this.getSelectionControl(row).value) return;
      this.unselectAllOthers(this.getRowId(row));
    } else if (this.view().gridSelectionOptions?.selectionType == GridSelectionType.Multiple) {
      this.updateSelectAllState();
    }
  }

  selectAllUpdated() {
    const value = this.selectAllControl.value;
    for (const row of this.rows().controls) {
      this.getSelectionControl(row).setValue(value);
    }
  }

  private unselectAllOthers(idToKeep: unknown) {
    for (const row of this.rows().controls) {
      if (this.getRowId(row) === idToKeep) continue;
      this.getSelectionControl(row).setValue(false);
    }
  }

  private updateSelectAllState() {
    let allSelected = true;
    for (const row of this.rows().controls) {
      const value = this.getSelectionControl(row).value;
      if (!value) allSelected = false;
    }
    if (allSelected) this.selectAllControl.setValue(true);
    else this.selectAllControl.setValue(false);
  }

  private getSelectionControl(row: FormGroup) {
    const selectionProperty = this.view().gridSelectionOptions?.selectionProperty ?? '';
    return this.getControlFromRow(row, selectionProperty);
  }
}
