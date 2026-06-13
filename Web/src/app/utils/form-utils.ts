export type ControlPath = (string | number)[] | string;

export function isControlPath(value: unknown): value is ControlPath {
  if (typeof value === 'string') return true;

  if (
    Array.isArray(value) &&
    value.every((part) => typeof part === 'string' || typeof part === 'number')
  )
    return true;

  return false;
}

export function parentPath(path?: ControlPath): ControlPath {
  if (!path) return [];
  if (typeof path !== 'string') return path.slice(0, -1);

  const lastIndexOf = path.lastIndexOf('.');
  if (lastIndexOf > 0) return path.slice(0, lastIndexOf);
  return '';
}

export function lastOfPath(path?: ControlPath): string | number {
  if (!path) return '';
  if (typeof path !== 'string') return path[-1];

  const lastIndexOf = path.lastIndexOf('.');
  if (lastIndexOf > 0) return path.slice(lastIndexOf + 1);
  return path;
}

export function joinPath(path1?: ControlPath, path2?: ControlPath | number | string): ControlPath {
  if (path2 === undefined) return path1 ?? [];
  if (path1 === undefined) return typeof path2 === 'number' ? [path2] : path2;
  if ((typeof path1) in ['string', 'number'] && (typeof path2) in ['string', 'number'])
    return `${path1}.${path2}`;
  if (!Array.isArray(path2)) return [...path1, path2];
  if (!Array.isArray(path1)) return [path1, ...path2];
  return [...path1, ...path2];
}

export function pathAsString(path: ControlPath): string {
  if (Array.isArray(path)) return path.join('.');
  return path;
}
