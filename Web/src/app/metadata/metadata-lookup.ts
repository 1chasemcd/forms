import { Injectable } from '@angular/core';
import { ModelMetadataContainer, PropertyMetadata, PropertyMetadataContainer } from '../api/api.g';
import { MetadataType, MetadataValueByType } from '../utils/api-utils';
import { ControlPath, iteratePath } from '../utils/form-utils';
import { AbstractControl } from '@angular/forms';

@Injectable()
export class MetadataLookupService {
  private metadatas: ModelMetadataContainer[] = [];
  private readonly typeDict: Map<string, number> = new Map();
  private readonly pathMap = new WeakMap<AbstractControl, ControlPath>();
  private _initialized = false;
  get initialized() {
    return this._initialized;
  }

  initialize(metadatas: ModelMetadataContainer[]) {
    if (this.initialized) throw Error('cannot initialize MetadataLookupService more than once');
    this.metadatas = metadatas;
    for (let i = 0; i < this.metadatas.length; i++) this.typeDict.set(this.metadatas[i].type, i);
  }

  lookupByType(type: string | null): ModelMetadataContainer | undefined {
    if (!type) return undefined;
    const index = this.typeDict.get(type);
    if (index === undefined || index < 0 || index >= this.metadatas.length) return undefined;
    return this.metadatas[index];
  }

  lookupByPath(root: string, path: ControlPath | null): PropertyMetadataContainer | undefined {
    let modelMetadata = this.lookupByType(root);
    let propertyMetadata;

    for (const pathPart of iteratePath(path ?? [])) {
      if (typeof pathPart === 'number') continue;
      propertyMetadata = modelMetadata?.propertyMetadatas[pathPart];
      if (!propertyMetadata) return undefined;
      if (propertyMetadata.$type === 'enumerable')
        modelMetadata = this.lookupByType(propertyMetadata.enumeratedType);
      else if (propertyMetadata.$type === 'subproperty')
        modelMetadata = this.lookupByType(propertyMetadata.subPropertyType);
      else break;
    }
    return propertyMetadata;
  }

  getPropertyMetadata<T extends MetadataType>(
    root: string,
    path: ControlPath,
    metadataType: T,
  ): MetadataValueByType<T> | undefined {
    const metadataCollection = this.lookupByPath(root, path);
    if (!metadataCollection || metadataCollection.$type !== 'primitive') return undefined;
    const propMetadata = metadataCollection.metadatas.find(
      (x): x is Extract<PropertyMetadata, { $type: T }> => x.$type === metadataType,
    );
    return propMetadata?.value as MetadataValueByType<T> | undefined;
  }
}
