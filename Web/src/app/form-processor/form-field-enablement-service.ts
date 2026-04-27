import { Injectable } from '@angular/core';
import { AbstractControl } from '@angular/forms';
import { BehaviorSubject, Observable, of } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class FormFieldEnablementService {
  private readonly idMap = new WeakMap<object, string>();
  private readonly enablementSubjects = new Map<string, BehaviorSubject<boolean>>();
  private readonly eneablementDependentValues = new Map<string, boolean[]>();

  registerControl(control: AbstractControl) {
    const id = this.addToMaps(control);
    const subject = this.enablementSubjects.get(id);
    if (!subject) return;

    subject.subscribe((value) => {
      if (value) control.enable({ emitEvent: false });
      else control.disable({ emitEvent: false });
    });
  }

  enablementOf(obj: object): Observable<boolean> {
    const id = this.addToMaps(obj);
    return this.enablementSubjects.get(id) ?? of(false);
  }

  private addToMaps(obj: object): string {
    let id = this.idMap.get(obj);
    if (id) return id;
    id = crypto.randomUUID();
    const subject = new BehaviorSubject<boolean>(true);

    this.idMap.set(obj, id);
    this.enablementSubjects.set(id, subject);

    return id;
  }

  enabledFor(obj: object, event: Observable<unknown>) {
    const id = this.addToMaps(obj);
    let deps = this.eneablementDependentValues.get(id);
    if (!deps) {
      deps = [];
      this.eneablementDependentValues.set(id, deps);
    }

    const index = deps.push(false) - 1;
    event.subscribe((x) => {
      const deps = this.eneablementDependentValues.get(id);
      const subj = this.enablementSubjects.get(id);
      if (!deps || !subj) return;
      deps[index] = !!x;
      subj.next(deps.every((v) => v));
    });
  }

  enabledForParent(obj: object, parent: object) {
    const parentId = this.addToMaps(parent);
    const parentSubject = this.enablementSubjects.get(parentId);
    if (!parentSubject) return;

    this.enabledFor(obj, parentSubject);
  }
}
