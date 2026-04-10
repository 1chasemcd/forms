import { inject, Injectable } from '@angular/core';
import { FormArray, FormGroup, Validators } from '@angular/forms';
import {
  BaseViewDefinition,
  CombinedViewDefinition,
  FieldDefinition,
  FieldType,
  FormDefinition,
  PropertyOrConstant,
  SubPropertyGridViewDefinition,
  MetadataType,
} from '../api/api.g';
import {
  createMaxLengthValidator,
  createPrecisionScaleValidator,
  createRangeValidator,
} from '../utils/validators';
import { GridDefinitionService } from './grid-definition-service';
import { applyPropertyOrConstant, getMetadata, getOrAddControl } from '../utils/api-utils';
import { combineLatest, map, of, startWith } from 'rxjs';
import { FormEnablementService } from './form-enablement-service';

@Injectable()
export class FormControlService {
  private gridDefinitionService = inject(GridDefinitionService);
  private formEnablementService = inject(FormEnablementService);

  createFromDefinition(form: FormDefinition) {
    const formGroup = new FormGroup({});
    if (form.view) this.processView(form.view, formGroup);

    return formGroup;
  }

  private processView(view: BaseViewDefinition, formGroup: FormGroup) {
    switch (view.$type) {
      case 'combinedview':
        this.processCombinedView(view, formGroup);
        break;
      case 'fieldview':
        view.fields?.forEach((f) => {
          this.processField(f, formGroup);
          this.formEnablementService.setControlEnablement(f, formGroup);
        });
        break;
      case 'subpropertygridview':
        this.processGridView(view, formGroup);
        break;
    }
  }

  private processCombinedView(view: CombinedViewDefinition, formGroup: FormGroup) {
    view.views?.forEach((v) => this.processView(v, formGroup));
  }

  private processGridView(view: SubPropertyGridViewDefinition, formGroup: FormGroup) {
    const formArray = new FormArray<FormGroup>([]);
    formGroup.addControl(view.subPropertyName, formArray);
    let parentEnabled = of(true);
    if (view.editForm) parentEnabled = of(false);
    else if (view.canEdit?.$type === 'constant' && !view.canEdit.value) parentEnabled = of(false);
    else if (view.canEdit?.$type === 'property') {
      const parendEnabledControl = getOrAddControl(view.canEdit.value, formGroup);
      parentEnabled = parendEnabledControl.valueChanges.pipe(startWith(parendEnabledControl.value));
    }

    this.gridDefinitionService.registerDefinition(view, parentEnabled, (view, parentEnabled) => {
      const rowGroup = new FormGroup({});

      let rowEnabled = of(true);
      if (view.canEditRow?.$type === 'constant' && !view.canEditRow.value) rowEnabled = of(false);
      else if (view.canEditRow?.$type === 'property') {
        const rowEnabledControl = getOrAddControl(view.canEditRow.value, rowGroup);
        rowEnabled = rowEnabledControl.valueChanges.pipe(startWith(rowEnabledControl.value));
      }

      view.fields?.forEach((f) => {
        this.processField(f, rowGroup);
        this.formEnablementService.setControlEnablement(
          f,
          rowGroup,
          combineLatest([parentEnabled, rowEnabled]).pipe(map(([x, y]) => x && y)),
        );
      });

      return rowGroup;
    });
  }

  private processField(field: FieldDefinition, formGroup: FormGroup) {
    const hasMetadata = (type: MetadataType) =>
      getMetadata<PropertyOrConstant>(field, type) !== undefined;
    const applyMetadataPoc = <T>(type: MetadataType, callback: (value: T) => void) => {
      const poc = getMetadata<PropertyOrConstant>(field, type);
      applyPropertyOrConstant(poc, formGroup, callback);
    };

    const control = getOrAddControl(field.property, formGroup);
    if (field.type == FieldType.Button) return;

    applyMetadataPoc(MetadataType.Required, (value) => {
      const original = control.hasValidator(Validators.required);
      if (original == value) return;
      if (value) control.addValidators(Validators.required);
      else control.removeValidators(Validators.required);
      control.updateValueAndValidity();
    });

    if (hasMetadata(MetadataType.MinValue) || hasMetadata(MetadataType.MaxValue)) {
      const rangeValidator = createRangeValidator();
      control.addValidators(rangeValidator.validator);

      applyMetadataPoc(MetadataType.MinValue, (value: string | number) => {
        rangeValidator.setMax(value);
      });

      applyMetadataPoc(MetadataType.MaxValue, (value: string | number) => {
        rangeValidator.setMin(value);
      });
    }

    if (hasMetadata(MetadataType.MaxLength)) {
      const maxLengthValidator = createMaxLengthValidator();
      control.addValidators(maxLengthValidator.validator);

      applyMetadataPoc(MetadataType.MaxLength, (value: number) => {
        maxLengthValidator.setMaxLength(value);
      });
    }

    if (hasMetadata(MetadataType.Precision) || hasMetadata(MetadataType.Scale)) {
      const psValidator = createPrecisionScaleValidator();
      control.addValidators(psValidator.validator);

      applyMetadataPoc(MetadataType.Precision, (value: number) => {
        psValidator.setPrecision(value);
      });

      applyMetadataPoc(MetadataType.Scale, (value: number) => {
        psValidator.setScale(value);
      });
    }
  }
}
