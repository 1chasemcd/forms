import { PropertyOrConstant } from '../api/api.g';

export class FormModel {
  private readonly _model: Record<string, unknown> = {};
  private readonly _onChangeActions: Record<string, Array<(value: unknown) => void>> = {};

  patch(values: Record<string, unknown>) {
    for (const [key, value] of Object.entries(values)) {
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
