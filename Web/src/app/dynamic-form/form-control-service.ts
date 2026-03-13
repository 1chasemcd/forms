import { Injectable } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { FormDefinition, BaseView, CombinedView, DataView, BaseField } from '../api/api.g';
import { FormModel } from './form-model';

@Injectable()
export class FormControlService {
  createFromDefinition(form: FormDefinition, model: FormModel) {
    let controls: Record<string, FormControl> = {};
    if (form.View) controls = this.processView(form.View, model);

    return new FormGroup(controls);
  }

  private processView(view: BaseView, model: FormModel): Record<string, FormControl> {
    switch (view.$type) {
      case 'combinedview':
        return this.processCombinedView(view, model);
      case 'dataview':
        return this.processDataView(view, model);
      case 'repositorygridview':
      case 'subpropertygridview':
        return {};
    }
  }

  private processCombinedView(view: CombinedView, model: FormModel): Record<string, FormControl> {
    const controls = view.Views?.map((v) => this.processView(v, model)) ?? [];
    return Object.assign({}, ...controls);
  }

  private processDataView(view: DataView, model: FormModel): Record<string, FormControl> {
    const fields = view.Fields?.map((f) => this.processField(f, model)) ?? [];
    return Object.fromEntries(fields.filter((v): v is [string, FormControl] => v !== null));
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

    return [field.Property, control];
  }
}
