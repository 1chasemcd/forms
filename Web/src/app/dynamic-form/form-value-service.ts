import { inject, Injectable } from '@angular/core';
import { FormArray, FormGroup } from '@angular/forms';
import { FormFactory } from './form-factory';
import { FormContext } from './form-context';

@Injectable({ providedIn: 'root' })
export class FormValueService {
  private formFactory = inject(FormFactory);

  patchValues(formContext: FormContext, values: Record<string, unknown>) {
    for (const [key, value] of Object.entries(values)) {
      const toSet = formContext.getOrAddControl(key);
      if (toSet instanceof FormArray) {
        if (Array.isArray(value)) {
          this.patchGridValues(key, toSet, value as Record<string, unknown>[], formContext);
        } else toSet.clear();
        continue;
      }
      if (toSet.value === value) continue;
      toSet.setValue(value);
    }
  }

  private patchGridValues(
    gridId: string,
    formArray: FormArray<FormGroup>,
    valuesArray: Record<string, unknown>[],
    parentContext: FormContext,
  ) {
    formArray.clear();

    // TODO make this more efficient
    for (const row of valuesArray) {
      const rowContext = this.formFactory.createGridRowContext(gridId, parentContext);
      if (rowContext === null) continue;
      this.patchValues(rowContext, row);
      formArray.push(rowContext.formGroup);
    }
  }

  toRecord(formGroup: FormGroup): Record<string, unknown> {
    const result: Record<string, unknown> = {};

    for (const kv of Object.entries(formGroup.controls)) {
      if (kv[1] instanceof FormArray) result[kv[0]] = this.toArray(kv[1]);
      else result[kv[0]] = kv[1].value;
    }

    return result;
  }

  private toArray(formArray: FormArray<FormGroup>): Record<string, unknown>[] {
    const result = [];
    for (const group of formArray.controls) {
      result.push(this.toRecord(group));
    }

    return result;
  }
}
