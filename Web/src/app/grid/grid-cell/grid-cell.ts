import { Component, computed, inject, input } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { ControlType, FormControlInfoContainer } from '../../api/api.g';
import { MetadataLookupService } from '../../metadata/metadata-lookup-service';
import { ControlPath, joinPath } from '../../utils/form-utils';
import { FormModelService } from '../../form-services/form-model-service';
import { CheckboxIcon } from '../../dynamic-control/checkbox/checkbox-icon';
import { CurrencyPipe, DatePipe, DecimalPipe } from '@angular/common';

@Component({
  selector: 'app-grid-cell',
  imports: [ReactiveFormsModule, CheckboxIcon, DatePipe, CurrencyPipe, DecimalPipe],
  templateUrl: './grid-cell.html',
  host: {
    class: 'h-full p-2 min-w-full w-0',
  },
})
export class GridCell {
  readonly ControlType = ControlType;

  readonly controlInfo = input.required<FormControlInfoContainer>();
  readonly parentPath = input.required<ControlPath>();

  private readonly metadataLookup = inject(MetadataLookupService);
  private readonly formModelService = inject(FormModelService);

  private readonly path = computed(() =>
    joinPath(this.parentPath(), this.controlInfo().propertyName),
  );
  readonly control = computed(() => this.formModelService.get<FormControl>(this.path()));
  readonly controlType = computed(
    () => this.metadataLookup.getPropertyMetadata(this.path(), 'controlType') ?? ControlType.Text,
  );

  timeToDate(time: unknown) {
    return new Date(`1970-01-01T${time}`);
  }
}
