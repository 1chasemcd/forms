import { Injectable } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { FormDefinition, BaseView, CombinedView, DataView, BaseField } from '../api/api.g';
import { FormModel } from '../utils/api-model-utils';

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
    return Object.fromEntries(fields);
  }

  private processField(field: BaseField, model: FormModel): [string, FormControl] {
    if (!field.Id) throw Error('Id unspecified');

    const control = new FormControl();
    control.setValue(model[field.Id]);

    if (
      field.$type != 'statictextfield' &&
      field.$type != 'button' &&
      field.Required?.$type == 'constant' &&
      field.Required.Value == true
    )
      control.addValidators(Validators.required);

    if (
      field.$type != 'statictextfield' &&
      field.Disabled?.$type == 'constant' &&
      field.Disabled.Value == true
    )
      control.disable();

    return [field.Id, control];
  }
}
