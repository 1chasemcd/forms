import { inject, Injectable } from '@angular/core';
import { FormArray, FormControl, FormGroup, Validators } from '@angular/forms';
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
import { getMetadata } from '../utils/api-utils';

type FormRecord = Record<string, FormControl | FormArray<FormGroup>>;

@Injectable()
export class FormControlService {
  private gridDefinitionService = inject(GridDefinitionService);

  createFromDefinition(form: FormDefinition) {
    const formRecord: FormRecord = {};
    if (form.view) this.processView(form.view, formRecord);

    return new FormGroup(formRecord);
  }

  private processView(view: BaseViewDefinition, formRecord: FormRecord) {
    switch (view.$type) {
      case 'combinedview':
        this.processCombinedView(view, formRecord);
        break;
      case 'fieldview':
        view.fields?.forEach((f) => this.processField(f, formRecord));
        break;
      case 'subpropertygridview':
        this.processGridView(view, formRecord);
        break;
    }
  }

  private processCombinedView(view: CombinedViewDefinition, formRecord: FormRecord) {
    view.views?.forEach((v) => this.processView(v, formRecord));
  }

  private processGridView(view: SubPropertyGridViewDefinition, formRecord: FormRecord) {
    const formArray = new FormArray<FormGroup>([]);
    formRecord[view.subPropertyName] = formArray;

    this.gridDefinitionService.registerDefinition(view.subPropertyName, () => {
      const controls: Record<string, FormControl> = {};
      view.fields?.forEach((f) => this.processField(f, controls));
      return new FormGroup(controls);
    });
  }

  private processField(field: FieldDefinition, formRecord: FormRecord) {
    const hasMetadata = (type: MetadataType) =>
      field.fieldMetadatas?.find((x) => x.type == type) !== undefined;

    const applyMetadata = <T>(type: MetadataType, callback: (value: T) => void) => {
      const poc = getMetadata<PropertyOrConstant>(field, type);
      if (poc === null || poc === undefined) return;

      if (poc.$type === 'constant') callback(poc.value);
      else {
        const propertyControl = this.getOrAddControl(poc.value, formRecord);
        propertyControl.valueChanges.subscribe(callback);
      }
    };

    const control = this.getOrAddControl(field.property, formRecord);
    if (field.type == FieldType.Button) return;

    applyMetadata(MetadataType.Required, (value) => {
      const original = control.hasValidator(Validators.required);
      if (original == value) return;
      if (value) control.addValidators(Validators.required);
      else control.removeValidators(Validators.required);
      control.updateValueAndValidity();
    });

    applyMetadata(MetadataType.Enabled, (value) => {
      if (value) control.enable();
      else control.disable();
    });

    applyMetadata(MetadataType.Enabled, (value) => {
      if (value) control.enable();
      else control.disable();
    });

    if (hasMetadata(MetadataType.MinValue) || hasMetadata(MetadataType.MaxValue)) {
      const rangeValidator = createRangeValidator();
      control.addValidators(rangeValidator.validator);

      applyMetadata(MetadataType.MinValue, (value: string | number) => {
        rangeValidator.setMax(value);
      });

      applyMetadata(MetadataType.MaxValue, (value: string | number) => {
        rangeValidator.setMin(value);
      });
    }

    if (hasMetadata(MetadataType.MaxLength)) {
      const maxLengthValidator = createMaxLengthValidator();
      control.addValidators(maxLengthValidator.validator);

      applyMetadata(MetadataType.MaxLength, (value: number) => {
        maxLengthValidator.setMaxLength(value);
      });
    }

    if (hasMetadata(MetadataType.Precision) || hasMetadata(MetadataType.Scale)) {
      const psValidator = createPrecisionScaleValidator();
      control.addValidators(psValidator.validator);

      applyMetadata(MetadataType.Precision, (value: number) => {
        psValidator.setPrecision(value);
      });

      applyMetadata(MetadataType.Scale, (value: number) => {
        psValidator.setScale(value);
      });
    }
  }

  private getOrAddControl(key: string, formRecord: FormRecord) {
    if (!Object.hasOwn(formRecord, key)) formRecord[key] = new FormControl();
    return formRecord[key] as FormControl;
  }
}
