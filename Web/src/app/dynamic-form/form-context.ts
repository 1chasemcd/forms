import { DestroyRef } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { Observable } from 'rxjs';

export class FormContext {
  constructor(
    public readonly formGroup: FormGroup,
    public readonly destroyRef: DestroyRef,
  ) {}

  getOrAddControl(key: string, group: FormGroup = this.formGroup) {
    if (!group.get(key)) group.addControl(key, new FormControl({}));
    return group.get(key) as FormControl;
  }

  untilDestroyed<T>(observable: Observable<T>): Observable<T> {
    return observable.pipe(takeUntilDestroyed(this.destroyRef));
  }
}
