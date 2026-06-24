import { inject, Injectable, Injector, signal } from '@angular/core';
import { ControlPath } from '../../utils/form-utils';
import { FileResponse, RepositoryClient } from '../../api/api.g';
import { FormModel } from './form-model';

type RepositoryFormStackEntry = {
  $type: 'repository';
  view: number;
  type: string;
  id?: string;
};

type SubpropertyFormStackEntry = {
  $type: 'subproperty';
  view: number;
  path: ControlPath;
};

export type FormStackEntry = RepositoryFormStackEntry | SubpropertyFormStackEntry;
type FormStackEntryWithModel = FormStackEntry & { model: FormModel };

@Injectable()
export class FormStackService {
  private readonly formStack: FormStackEntryWithModel[] = [];
  private readonly repositoryClient = inject(RepositoryClient);
  private readonly injector = inject(Injector);
  private _activeModel: FormModel | undefined;

  activeEntry = signal<FormStackEntry | undefined>(undefined);
  get activeModel() {
    if (!this._activeModel) throw Error('Cannot access activeModel before pushing to Form Stack');
    return this._activeModel;
  }

  pushRepository(viewId: number, type: string, id?: string) {
    this.formStack.push({
      $type: 'repository',
      model: new FormModel(type, this.injector),
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
    const last = this.formStack.at(-1);
    if (!last) return;
    this.formStack.push({
      $type: 'subproperty',
      model: last.model.clone(),
      view: viewId,
      path: path,
    });
    this.updateActive();
  }

  commitActive() {
    const active = this.formStack.pop();
    if (!active) return;
    if (active.$type == 'repository')
      this.repositoryClient.save(active.type, active.model.toRecord());
    else {
      const current = this.formStack.at(-1);
      if (current) current.model.patchValues(active.model.toRecord());
    }

    this.updateActive();
  }

  cancelActive() {
    this.formStack.pop();
    this.updateActive();
  }

  private updateActive() {
    this.activeEntry.set(this.formStack.at(-1));
    this._activeModel = this.formStack.at(-1)?.model;
  }

  private handleRepositoryResponse(index: number, resp: FileResponse) {
    resp.data.text().then((text) => {
      const model = JSON.parse(text);
      this.formStack[index].model.patchValues(model);
      for (let i = index + 1; i < this.formStack.length; i++)
        this.formStack[i].model.patchValues(model);
      this._activeModel = this.formStack.at(-1)?.model;
    });
  }
}
