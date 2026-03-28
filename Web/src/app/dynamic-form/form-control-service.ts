import { inject, Injectable } from '@angular/core';
import { FormArray, FormControl, FormGroup, Validators } from '@angular/forms';
import {
  FormDefinition,
  BaseView,
  CombinedView,
  BaseField,
  SubPropertyGridView,
  PropertyOrConstant,
} from '../api/api.g';
import {
  createMaxLengthValidator,
  createPrecisionScaleValidator,
  createRangeValidator,
} from '../utils/validators';
import { GridDefinitionService } from './grid-definition-service';

type FormRecord = Record<string, FormControl | FormArray<FormGroup>>;

@Injectable()
export class FormControlService {
  private gridDefinitionService = inject(GridDefinitionService);

  createFromDefinition(form: FormDefinition) {
    const formRecord: FormRecord = {};
    if (form.View) this.processView(form.View, formRecord);

    return new FormGroup(formRecord);
  }

  private processView(view: BaseView, formRecord: FormRecord) {
    switch (view.$type) {
      case 'combinedview':
        this.processCombinedView(view, formRecord);
        break;
      case 'dataview':
        this.processFieldView(view, formRecord);
        break;
      case 'subpropertygridview':
        this.processGridView(view, formRecord);
        break;
      case 'repositorygridview':
        break;
    }
  }

  private processCombinedView(view: CombinedView, formRecord: FormRecord) {
    view.Views?.forEach((v) => this.processView(v, formRecord));
  }

  private processFieldView<T extends BaseView & { Fields: BaseField[] }>(
    view: T,
    formRecord: FormRecord,
  ) {
    view.Fields?.forEach((f) => this.processField(f, formRecord));
  }

  private processGridView(view: SubPropertyGridView, formRecord: FormRecord) {
    const formArray = new FormArray<FormGroup>([]);
    formRecord[view.SubPropertyName] = formArray;

    this.gridDefinitionService.registerDefinition(view.SubPropertyName, () => {
      const controls: Record<string, FormControl> = {};
      this.processFieldView(view, controls);
      return new FormGroup(controls);
    });
  }

  private processField(field: BaseField, formRecord: FormRecord) {
    if (field.$type == 'statictextfield' || field.$type == 'buttonfield') return;

    const control = this.getOrAddControl(field.Property, formRecord);

    this.handlePropertyOrConst(field.Required, formRecord, (required) => {
      if (required) control.addValidators(Validators.required);
      else control.removeValidators(Validators.required);
    });

    this.handlePropertyOrConst(field.Disabled, formRecord, (disabled) => {
      if (disabled) control.disable();
      else control.enable();
    });

    if (
      field.$type === 'numericinput' ||
      field.$type === 'currencyinput' ||
      field.$type === 'dateinput' ||
      field.$type === 'timeinput'
    ) {
      const rangeValidator = createRangeValidator();
      control.addValidators(rangeValidator.validator);

      this.handlePropertyOrConst(field.MaxValue, formRecord, (value: number) => {
        rangeValidator.setMax(value);
      });

      this.handlePropertyOrConst(field.MinValue, formRecord, (value: number) => {
        rangeValidator.setMin(value);
      });
    }

    if (field.$type === 'textareainput' || field.$type === 'textinput') {
      const maxLengthValidator = createMaxLengthValidator();
      control.addValidators(maxLengthValidator.validator);

      this.handlePropertyOrConst(field.MaxLength, formRecord, (value: number) => {
        maxLengthValidator.setMaxLength(value);
      });
    }

    if (field.$type === 'numericinput') {
      const psValidator = createPrecisionScaleValidator();
      control.addValidators(psValidator.validator);

      this.handlePropertyOrConst(field.Precision, formRecord, (value: number) => {
        psValidator.setPrecision(value);
      });

      this.handlePropertyOrConst(field.Scale, formRecord, (value: number) => {
        psValidator.setScale(value);
      });
    }
  }

  private getOrAddControl(key: string, formRecord: FormRecord) {
    if (!Object.hasOwn(formRecord, key)) formRecord[key] = new FormControl();
    return formRecord[key] as FormControl;
  }

  private handlePropertyOrConst<T>(
    poc: PropertyOrConstant | null | undefined,
    formRecord: FormRecord,
    callback: (value: T) => void,
  ) {
    if (poc === null || poc === undefined) return;
    if (poc.$type === 'constant') callback(poc.Value);
    else {
      const propertyControl = this.getOrAddControl(poc.Value, formRecord);
      propertyControl.valueChanges.subscribe(callback);
    }
  }
}
