import { inject, Injectable } from '@angular/core';
import { FormArray, FormControl, FormGroup } from '@angular/forms';
import { FormFactory } from './form-factory';

@Injectable({ providedIn: 'root' })
export class FormValueService {
  private formFactory = inject(FormFactory);

  patchValues(formGroup: FormGroup, values: Record<string, unknown>) {
    for (const [key, value] of Object.entries(values)) {
      let toSet = formGroup.get(key);
      if (toSet === null) {
        toSet = new FormControl(value);
        formGroup.addControl(key, toSet);
      }
      if (toSet instanceof FormArray) {
        if (Array.isArray(value)) {
          this.patchGridValues(key, toSet, value as Record<string, unknown>[]);
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
  ) {
    formArray.clear();

    // TODO make this more efficient
    for (const row of valuesArray) {
      const group = this.formFactory.createGridRowGroup(gridId);
      if (group === null) continue;
      this.patchValues(group, row);
      formArray.push(group);
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
