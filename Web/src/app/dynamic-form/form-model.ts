import { PropertyOrConstant } from '../api/api.g';

export class FormModel {
  private readonly _model: Record<string, unknown | FormModelArray> = {};
  private readonly _onChangeActions: Record<string, Array<(value: unknown) => void>> = {};

  patch(values: Record<string, unknown>) {
    for (const [key, value] of Object.entries(values)) {
      if (this._model[key] instanceof FormModelArray) {
        if (Array.isArray(value)) this._model[key].patch(value);
        else this._model[key].patch([]);
      }
      if (!this.needsUpdate(key, value)) continue;
      this._model[key] = value;
      this.applyChanges(key);
    }
  }

  set(key: string, value: unknown) {
    if (!this.needsUpdate(key, value)) return;
    this._model[key] = value;
    this.applyChanges(key);
  }

  get<T = unknown>(key: string): T {
    return this._model[key] as T;
  }

  asRecord(): Record<string, unknown> {
    return Object.fromEntries(
      Object.entries(this._model).map(([k, v]) => [
        k,
        v instanceof FormModelArray ? v.asArray() : v,
      ]),
    );
  }

  registerDependency<T = unknown>(key: string, actionOnChange: (value: T) => void) {
    this._onChangeActions[key] ??= [];
    this._onChangeActions[key].push(actionOnChange as (value: unknown) => void);
  }

  registerPocDependency<T = unknown>(
    poc: PropertyOrConstant | undefined,
    action: (value: T) => void,
  ) {
    if (!poc) return;
    if (poc.$type === 'constant') action(poc.Value);
    else if (poc.$type === 'property') this.registerDependency(poc.Value, action);
  }

  private needsUpdate(key: string, newValue: unknown) {
    return !Object.hasOwn(this._model, key) || this._model[key] !== newValue;
  }

  private applyChanges(key: string) {
    const actions = this._onChangeActions[key];
    if (!actions) return;
    for (const action of actions) {
      action(this._model[key]);
    }
  }
}

export class FormModelArray {
  private readonly _rows: Map<unknown, FormModel> = new Map<unknown, FormModel>();
  private readonly _indices: unknown[] = [];
  private readonly _idProperty: string;
  private _onAddRow: (rowModel: FormModel) => void;
  private _onRemoveRow: (id: unknown) => void;

  patch(updated: Record<string, unknown>[]) {
    const updatedIds = new Set(updated.map((r) => r[this._idProperty]));

    // remove deleted rows
    for (const [rowId] of this._rows)
      if (!updatedIds.has(rowId)) {
        this._onRemoveRow(rowId);
        this._rows.delete(rowId);
      }

    this._indices.splice(0);

    // update existing and add new rows
    for (const updatedRow of updated) {
      const rowId = updatedRow[this._idProperty];
      this._indices.push(rowId);
      const currentRow = this._rows.get(rowId);

      if (currentRow === undefined) {
        const formModel = new FormModel();
        this._onAddRow(formModel);
        this._rows.set(rowId, formModel);
      } else currentRow.patch(updatedRow);
    }
  }

  asArray() {
    return this._indices.map((id) => this._rows.get(id)?.asRecord()).filter((x) => x !== undefined);
  }
  constructor(
    idProperty: string,
    onAddRow: (value: FormModel) => void,
    onRemoveRow: (id: unknown) => void,
  ) {
    this._idProperty = idProperty;
    this._onAddRow = onAddRow;
    this._onRemoveRow = onRemoveRow;
  }
}
