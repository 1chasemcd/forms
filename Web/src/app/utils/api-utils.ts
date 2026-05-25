import { AbstractControl, FormArray, FormControl, FormGroup } from '@angular/forms';
import { PropertyMetadata, PropertyOrConstant } from '../api/api.g';
import { Observable, of, startWith } from 'rxjs';

export type MetadataType = PropertyMetadata['$type'];
export type MetadataByType<TType extends MetadataType> = Extract<
  PropertyMetadata,
  { $type: TType }
>;

export type MetadataValueByType<TType extends MetadataType> = Extract<
  PropertyMetadata,
  { $type: TType }
>['value'];

export function getPocObservable(
  poc: PropertyOrConstant,
  formGroup: FormGroup,
): Observable<unknown> {
  if (poc.$type === 'constant') return of(poc.value);
  const control = getOrAddControl(formGroup, poc.value);
  return control.valueChanges.pipe(startWith(control.value));
}

export function getOrAddControl(group: FormGroup, key: string) {
  const existing = group.get(key) as FormControl;
  if (existing) return existing;
  const control = new FormControl();
  group.addControl(key, control);
  return control;
}

export function getOrAddArray(group: FormGroup, key: string) {
  const existing = group.get(key) as FormArray<AbstractControl>;
  if (existing) return existing;
  const control = new FormArray<AbstractControl>([]);
  group.addControl(key, control);
  return control;
}

export function getOrAddGroup(
  group: FormGroup,
  key: string,
  toAdd: FormGroup | null = null,
): FormGroup {
  const existing = group.get(key) as FormGroup;
  if (existing) return existing;
  const control = toAdd ?? new FormGroup({});
  group.addControl(key, control);
  return control;
}
