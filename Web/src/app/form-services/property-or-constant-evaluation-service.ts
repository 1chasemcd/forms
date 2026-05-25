import { inject, Injectable } from '@angular/core';
import { FormModelService } from './form-model-service';
import { Observable, of, startWith } from 'rxjs';
import { PropertyOrConstant } from '../api/api.g';
import { FormControl } from '@angular/forms';
import { ControlPath, parentPath } from '../utils/form-utils';
import { MetadataType } from '../utils/api-utils';
import { MetadataLookupService } from '../metadata/metadata-lookup-service';

@Injectable()
export class PropertyOrConstantEvaluationService {
  private readonly formModelService = inject(FormModelService);
  private readonly metadataLookup = inject(MetadataLookupService);

  observe<T>(propertyOrConstant: PropertyOrConstant, modelPath: ControlPath): Observable<T> {
    if (propertyOrConstant.$type === 'constant') return of(propertyOrConstant.value);
    const control = this.formModelService.getOrAdd(
      [...modelPath, propertyOrConstant.value],
      new FormControl(),
    ) as FormControl<T>;
    return control.valueChanges.pipe(startWith(control.value));
  }

  propertyMetadataValueChanges<T>(
    path: ControlPath,
    metadataType: Exclude<MetadataType, 'controlType' | 'formServiceMethod'>,
  ): Observable<T | undefined> {
    const poc = this.metadataLookup.getPropertyMetadata(path, metadataType);
    if (!poc) return of(undefined);
    return this.observe<T>(poc, parentPath(path));
  }
}
