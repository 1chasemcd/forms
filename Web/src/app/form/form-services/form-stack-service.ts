import { inject, Injectable } from '@angular/core';
import { ControlPath } from '../../utils/form-utils';
import { BehaviorSubject, Observable } from 'rxjs';
import { FileResponse, RepositoryClient } from '../../api/api.g';

type RepositoryFormStackEntry = {
  $type: 'repository';
  model: Record<string, unknown>;
  view: number;
  type: string;
  id?: string;
};

type SubpropertyFormStackEntry = {
  $type: 'subproperty';
  model: Record<string, unknown>;
  view: number;
  path: ControlPath;
};

export type FormStackEntry = RepositoryFormStackEntry | SubpropertyFormStackEntry;

@Injectable()
export class FormStackService {
  private readonly formStack: FormStackEntry[] = [];
  private readonly activeSubject = new BehaviorSubject<FormStackEntry | undefined>(undefined);

  private readonly repositoryClient = inject(RepositoryClient);

  active(): Observable<FormStackEntry | undefined> {
    return this.activeSubject.asObservable();
  }

  pushRepository(viewId: number, type: string, id?: string) {
    this.formStack.push({
      $type: 'repository',
      model: {},
      view: viewId,
      type: type,
      id: id,
    });
    const index = this.formStack.length - 1;
    const request = id ? this.repositoryClient.get(type, id) : this.repositoryClient.create(type);
    request.subscribe((resp) => this.handleRepositoryResponse(index, resp));
    this.updateActive();
  }

  pushSubproperty(viewId: number, path: ControlPath = '') {
    this.formStack.push({
      $type: 'subproperty',
      model: structuredClone(this.formStack.at(-1)?.model ?? {}),
      view: viewId,
      path: path,
    });
    this.updateActive();
  }

  commitActive(modelToCommit: Record<string, unknown>) {
    const active = this.formStack.pop();
    if (active?.$type == 'repository') this.repositoryClient.save(active.type, modelToCommit);
    else {
      const current = this.formStack.at(-1);
      if (current) current.model = modelToCommit;
    }

    this.updateActive();
  }

  cancelActive() {
    this.formStack.pop();
    this.updateActive();
  }

  private updateActive() {
    const current = this.formStack.at(-1);
    if (current) this.activeSubject.next(current);
    else this.activeSubject.next(undefined);
  }

  private handleRepositoryResponse(index: number, resp: FileResponse) {
    resp.data.text().then((text) => {
      const model = JSON.parse(text);
      this.formStack[index].model = model;
      for (let i = index + 1; i < this.formStack.length; i++)
        this.formStack[i].model = structuredClone(model);
      this.updateActive();
    });
  }
}
