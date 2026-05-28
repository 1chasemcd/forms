import { Component, computed, inject, input } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { DynamicInput } from '../../dynamic-control/dynamic-input/dynamic-input';
import { CheckboxInput } from '../../dynamic-control/checkbox/checkbox-input';
import { ControlType, FormControlInfoContainer } from '../../api/api.g';
import { MetadataLookupService } from '../../metadata/metadata-lookup-service';
import { ControlPath, joinPath } from '../../utils/form-utils';
import { FormModelService } from '../../form-services/form-model-service';
import { ServiceMethodService } from '../../service-method/service-method-service';

@Component({
  selector: 'app-grid-cell',
  imports: [ReactiveFormsModule, DynamicInput, CheckboxInput],
  templateUrl: './grid-cell.html',
})
export class GridCell {
  readonly ControlType = ControlType;

  readonly controlInfo = input.required<FormControlInfoContainer>();
  readonly parentPath = input.required<ControlPath>();

  private readonly serviceMethodService = inject(ServiceMethodService);
  private readonly metadataLookup = inject(MetadataLookupService);
  private readonly formModelService = inject(FormModelService);

  private readonly path = computed(() =>
    joinPath(this.parentPath(), this.controlInfo().propertyName),
  );
  readonly control = computed(() => this.formModelService.get<FormControl>(this.path()));
  readonly controlType = computed(
    () => this.metadataLookup.getPropertyMetadata(this.path(), 'controlType') ?? ControlType.Text,
  );

  executeServiceMethod() {
    const method = this.metadataLookup.getPropertyMetadata(this.path(), 'formServiceMethod');
    if (method) this.serviceMethodService.runMethod(this.parentPath(), method);
  }
}
