import { Injectable } from '@angular/core';
import { AbstractControl, FormArray, FormControl, FormGroup } from '@angular/forms';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable()
export class ControlEnablementService {
  private readonly dependencies = new WeakMap<AbstractControl, boolean[]>();
  private readonly enablementSubjects = new WeakMap<AbstractControl, BehaviorSubject<boolean>>();

  private registerControl(control: AbstractControl) {
    if (this.enablementSubjects.has(control)) return;

    const subject = new BehaviorSubject<boolean>(true);
    this.enablementSubjects.set(control, subject);
    this.dependencies.set(control, []);

    subject.subscribe((value) => {
      if (value) {
        if (this.areParentsEnabled(control)) this.maybeEnableChildren(control);
      } else control.disable({ emitEvent: false });
    });
  }

  private areParentsEnabled(control: AbstractControl): boolean {
    const parent = control.parent;
    if (!parent) return true;
    const deps = this.dependencies.get(parent);
    if (deps && deps.findIndex((x) => !x) >= 0) return false;
    return this.areParentsEnabled(parent);
  }

  private maybeEnableChildren(control: AbstractControl) {
    const enablementDeps = this.dependencies.get(control);
    if (enablementDeps && enablementDeps.findIndex((x) => !x) >= 0) return;
    if (control instanceof FormControl) control.enable({ emitEvent: false });
    else if (control instanceof FormArray)
      control.controls.forEach((c) => this.maybeEnableChildren(c));
    else if (control instanceof FormGroup)
      Object.values(control.controls).forEach((c) => this.maybeEnableChildren(c));
  }

  enabledFor(control: AbstractControl, event?: Observable<unknown>) {
    this.registerControl(control);
    const deps = this.dependencies.get(control) as unknown as boolean[];
    const index = deps.push(false) - 1;

    event?.subscribe((x) => {
      const deps = this.dependencies.get(control) as unknown as boolean[];
      const subj = this.enablementSubjects.get(control) as unknown as BehaviorSubject<boolean>;
      deps[index] = !!x;
      subj.next(deps.every((v) => v));
    });
  }
}
