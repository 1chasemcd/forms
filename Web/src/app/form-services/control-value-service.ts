import { inject, Injectable } from '@angular/core';
import { FormModelService } from './form-model-service';
import { Observable, of, startWith } from 'rxjs';
import { PropertyOrConstant } from '../api/api.g';
import { ControlPath, isControlPath, joinPath, parentPath } from '../utils/form-utils';
import { MetadataType } from '../utils/api-utils';
import { MetadataLookupService } from '../metadata/metadata-lookup-service';
import { AbstractControl, FormArray, FormControl, FormGroup } from '@angular/forms';

@Injectable()
export class ControlValueService {
  private readonly formModelService = inject(FormModelService);
  private readonly metadataLookup = inject(MetadataLookupService);

  observe<T>(path: ControlPath): Observable<T> | undefined;
  observe<T>(control: FormControl | null): Observable<T> | undefined;
  observe<T>(basePath: ControlPath, relativePath: ControlPath): Observable<T> | undefined;
  observe<T>(basePath: ControlPath, poc: PropertyOrConstant): Observable<T> | undefined;
  observe<T>(
    baseControl: FormArray | FormGroup | null,
    relativePath: ControlPath,
  ): Observable<T> | undefined;
  observe<T>(
    baseControl: FormArray | FormGroup | null,
    poc: PropertyOrConstant,
  ): Observable<T> | undefined;
  observe<T>(
    param1: ControlPath | AbstractControl | null,
    param2?: PropertyOrConstant | ControlPath,
  ): Observable<T> | undefined {
    if (param1 === null || param1 instanceof AbstractControl) {
      if (!param2) return this.observeControl(param1, []);
      if (isControlPath(param2)) return this.observeControl(param1, param2);
      return this.observePropertyOrConstant(param1, param2);
    } else {
      if (!param2) return this.observePath(param1);
      if (isControlPath(param2)) return this.observePath(joinPath(param1, param2));
      return this.observePropertyOrConstant(param1, param2);
    }
  }

  private observeControl<T>(
    baseControl: AbstractControl | null,
    relativePath: ControlPath,
  ): Observable<T> | undefined {
    if (!baseControl) return undefined;
    const observed = baseControl.get(relativePath) as FormControl<T> | null;
    if (!observed) return undefined;
    return observed.valueChanges.pipe(startWith(observed.value));
  }

  private observePath<T>(path: ControlPath): Observable<T> | undefined {
    const control = this.formModelService.get(path);
    if (!control) return undefined;
    return control.valueChanges.pipe(startWith(control.value));
  }

  private observePropertyOrConstant<T>(
    parent: ControlPath | AbstractControl | null,
    propertyOrConstant: PropertyOrConstant,
  ): Observable<T> | undefined {
    if (propertyOrConstant.$type === 'constant') return of(propertyOrConstant.value);
    if (isControlPath(parent)) return this.observePath(joinPath(parent, propertyOrConstant.value));
    return this.observeControl(parent, propertyOrConstant.value);
  }

  observeMetadata<T>(
    path: ControlPath,
    metadataType: Exclude<MetadataType, 'controlType' | 'formServiceMethod'>,
  ): Observable<T> | undefined {
    const poc = this.metadataLookup.getPropertyMetadata(path, metadataType);
    if (!poc) return undefined;
    return this.observePropertyOrConstant<T>(parentPath(path), poc);
  }
}
