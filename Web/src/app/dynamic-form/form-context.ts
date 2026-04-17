import { DestroyRef } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { Observable } from 'rxjs';

export class FormContext {
  constructor(
    public readonly formGroup: FormGroup,
    public readonly destroyRef: DestroyRef,
  ) {}

  getOrAddControl(key: string) {
    if (!this.formGroup.get(key)) this.formGroup.addControl(key, new FormControl());
    return this.formGroup.get(key) as FormControl;
  }

  untilDestroyed<T>(observable: Observable<T>): Observable<T> {
    return observable.pipe(takeUntilDestroyed(this.destroyRef));
  }
}
