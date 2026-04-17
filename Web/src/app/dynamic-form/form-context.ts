import { DestroyRef } from '@angular/core';
import { AbstractControl, FormControl, FormGroup } from '@angular/forms';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { Observable } from 'rxjs';

export class FormContext {
  constructor(
    public readonly formGroup: FormGroup,
    public readonly destroyRef: DestroyRef,
    public readonly parentContext: FormContext | null = null,
  ) {}

  getOrAddControl(key: string): AbstractControl {
    let control = this.formGroup.get(key);
    if (!control) {
      control = new FormControl();
      this.formGroup.addControl(key, control);
    }
    return control;
  }

  untilDestroyed<T>(observable: Observable<T>): Observable<T> {
    return observable.pipe(takeUntilDestroyed(this.destroyRef));
  }
}
