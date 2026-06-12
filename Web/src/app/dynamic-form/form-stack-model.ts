import { ControlPath } from '../utils/form-utils';

export type FormStackModel = {
  model: Record<string, unknown>;
  modelPathRoot: ControlPath;
  viewId: number;
};

export type FormResultModel = {
  model: Record<string, unknown>;
  commitChanges: boolean;
};
