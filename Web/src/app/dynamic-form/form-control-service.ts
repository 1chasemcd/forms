import { Injectable } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import {
  FormModel,
  BaseView,
  CombinedView,
  DataView,
  BaseField,
} from '../api/api.g';

@Injectable()
export class FormControlService {
  getFormGroup(form: FormModel) {
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
    let label: string | undefined;
    switch (field.$type) {
      case 'button':
        label = field.MethodToRunOnChange;
        break;
      case 'statictextfield':
        label = field.Text;
        break;
      case 'checkboxinput':
      case 'currencyinput':
      case 'dateinput':
      case 'numericinput':
      case 'textareainput':
      case 'textinput':
      case 'timeinput':
        label = field.Property;
    }

    return [label ?? '', new FormControl()];
  }
}
