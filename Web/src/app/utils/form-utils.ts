export type ControlPath = (string | number)[];

export function parentPath(path: ControlPath) {
  return path.slice(0, -1);
}
