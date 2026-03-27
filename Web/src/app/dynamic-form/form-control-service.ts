import { Injectable } from '@angular/core';
import { FormArray, FormControl, FormGroup, Validators } from '@angular/forms';
import {
  FormDefinition,
  BaseView,
  CombinedView,
  BaseField,
  SubPropertyGridView,
} from '../api/api.g';
import { FormModel, FormModelArray } from './form-model';
import {
  createMaxLengthValidator,
  createPrecisionScaleValidator,
  createRangeValidator,
} from '../utils/validators';

@Injectable()
export class FormControlService {
  createFromDefinition(form: FormDefinition, model: FormModel) {
    let controls: Record<string, FormControl | FormArray> = {};
    if (form.View) controls = this.processView(form.View, model);

    return new FormGroup(controls);
  }

  private processView(view: BaseView, model: FormModel): Record<string, FormControl | FormArray> {
    switch (view.$type) {
      case 'combinedview':
        return this.processCombinedView(view, model);
      case 'dataview':
        return this.processFieldView(view, model);
      case 'subpropertygridview':
        return this.processGridView(view, model);
      case 'repositorygridview':
        return {};
    }
  }

  private processCombinedView(view: CombinedView, model: FormModel): Record<string, FormControl> {
    const controls = view.Views?.map((v) => this.processView(v, model)) ?? [];
    return Object.assign({}, ...controls);
  }

  private processFieldView<T extends BaseView & { Fields: BaseField[] }>(
    view: T,
    model: FormModel,
  ): Record<string, FormControl> {
    const fields = view.Fields?.map((f) => this.processField(f, model)) ?? [];
    return Object.fromEntries(fields.filter((v): v is [string, FormControl] => v !== null));
  }

  private processGridView(view: SubPropertyGridView, model: FormModel): Record<string, FormArray> {
    const formArray = new FormArray<FormGroup>([]);

    const onDeleteRow = (id: unknown) => {
      for (let i = 0; i < formArray.length; i++)
        if (formArray.at(i) instanceof FormGroup && formArray.at(i).get(view.IdProperty) == id)
          formArray.removeAt(i);
    };

    const onAddRow = (formModel: FormModel) => {
      const fields = this.processFieldView(view, formModel);
      formArray.push(new FormGroup(fields));
    };

    const formModelArray = new FormModelArray(view.IdProperty, onAddRow, onDeleteRow);
    model.set(view.SubPropertyName, formModelArray);

    return Object.fromEntries([[view.SubPropertyName, formArray]]);
  }

  private processField(field: BaseField, model: FormModel): [string, FormControl] | null {
    if (field.$type == 'statictextfield' || field.$type == 'buttonfield') return null;

    const control = new FormControl();
    control.valueChanges.subscribe((value) => {
      model.set(field.Property, value);
    });

    model.registerDependency(field.Property, (value) => {
      control.setValue(value);
    });

    model.registerPocDependency(field.Required, (required) => {
      if (required) control.addValidators(Validators.required);
      else control.removeValidators(Validators.required);
    });

    model.registerPocDependency(field.Disabled, (disabled) => {
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
      model.registerPocDependency(field.MaxValue, (value: number) => {
        rangeValidator.setMax(value);
      });
      model.registerPocDependency(field.MinValue, (value: number) => {
        rangeValidator.setMin(value);
      });
    }

    if (field.$type === 'textareainput' || field.$type === 'textinput') {
      const maxLengthValidator = createMaxLengthValidator();
      control.addValidators(maxLengthValidator.validator);
      model.registerPocDependency(field.MaxLength, (value: number) => {
        maxLengthValidator.setMaxLength(value);
      });
    }

    if (field.$type === 'numericinput') {
      const psValidator = createPrecisionScaleValidator();
      control.addValidators(psValidator.validator);
      model.registerPocDependency(field.Precision, (value: number) => {
        psValidator.setPrecision(value);
      });
      model.registerPocDependency(field.Scale, (value: number) => {
        psValidator.setScale(value);
      });
    }

    return [field.Property, control];
  }
}
