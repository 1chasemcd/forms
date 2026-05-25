import { inject, Injectable } from '@angular/core';
import { FormModelService } from './form-model-service';
import { Observable, of, startWith } from 'rxjs';
import { PropertyOrConstant } from '../api/api.g';
import { FormControl } from '@angular/forms';
import { ControlPath } from '../utils/form-utils';

@Injectable()
export class PropertyOrConstantEvaluationService {
  private readonly formModelService = inject(FormModelService);
  observe<T>(propertyOrConstant: PropertyOrConstant, modelPath: ControlPath): Observable<T> {
    if (propertyOrConstant.$type === 'constant') return of(propertyOrConstant.value);
    const control = this.formModelService.getOrAdd(
      [...modelPath, propertyOrConstant.value],
      new FormControl(),
    ) as FormControl<T>;
    return control.valueChanges.pipe(startWith(control.value));
  }
}
