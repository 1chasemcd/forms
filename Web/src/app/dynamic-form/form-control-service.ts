import { Injectable } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import {
  FormDefinition,
  BaseView,
  CombinedView,
  DataView,
  BaseField,
} from '../api/api.g';

@Injectable()
export class FormControlService {
  createFromDefinition(form: FormDefinition) {
    let controls: Record<string, FormControl> = {};
    if (form.View) controls = this.processView(form.View);

    return new FormGroup(controls);
  }

  private processView(view: BaseView): Record<string, FormControl> {
    switch (view.$type) {
      case 'combinedview':
        return this.processCombinedView(view);
      case 'dataview':
        return this.processDataView(view);
      case 'repositorygridview':
      case 'subpropertygridview':
        return {};
    }
  }

  private processCombinedView(view: CombinedView): Record<string, FormControl> {
    const controls = view.Views?.map((v) => this.processView(v)) ?? [];
    return Object.assign({}, ...controls);
  }

  private processDataView(view: DataView): Record<string, FormControl> {
    const fields = view.Fields?.map((f) => this.processField(f)) ?? [];
    return Object.fromEntries(fields);
  }

  private processField(field: BaseField): [string, FormControl] {
    if (!field.Id) throw Error('Id unspecified');

    const control = new FormControl();

    if (field.Required?.$type == 'constant' && field.Required.Value == true)
      control.addValidators(Validators.required);

    if (field.Disabled?.$type == 'constant' && field.Disabled.Value == true)
      control.disable();

    return [field.Id, control];
  }
}
