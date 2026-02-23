import { Injectable } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { FormModel } from '../api/api.g';
import {
  FormCombinedView,
  FormDataView,
  FormField,
  FormView,
} from '../api/types';

@Injectable()
export class FormControlService {
  getFormGroup(form: FormModel) {
    let controls: Record<string, FormControl> = {};
    if (form.view) controls = this.processView(form.view as FormView);

    return new FormGroup(controls);
  }

  private processView(view: FormView): Record<string, FormControl> {
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

  private processCombinedView(
    view: FormCombinedView,
  ): Record<string, FormControl> {
    const controls =
      view.views?.map((v) => this.processView(v as FormView)) ?? [];
    return Object.assign({}, ...controls);
  }

  private processDataView(view: FormDataView): Record<string, FormControl> {
    const fields =
      view.fields?.map((f) => this.processField(f as FormField)) ?? [];
    return Object.fromEntries(fields);
  }

  private processField(field: FormField): [string, FormControl] {
    let label: string | undefined;
    switch (field.$type) {
      case 'button':
        label = field.methodToRunOnChange;
        break;
      case 'statictextfield':
        label = field.text;
        break;
      case 'checkboxinput':
      case 'currencyinput':
      case 'dateinput':
      case 'numericinput':
      case 'textarea':
      case 'textinput':
      case 'timeinput':
        label = field.property;
    }

    return [label ?? '', new FormControl()];
  }
}
