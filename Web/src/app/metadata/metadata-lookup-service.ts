import { Injectable } from '@angular/core';
import { ModelMetadataContainer, PropertyMetadata, PropertyMetadataContainer } from '../api/api.g';
import { MetadataType, MetadataValueByType } from '../utils/api-utils';
import { ControlPath } from '../utils/form-utils';

@Injectable()
export class MetadataLookupService {
  private metadatas: ModelMetadataContainer[] = [];
  private _rootType: string | null = null;
  private typeDict: Map<string, number> = new Map();
  private _isInitialized: boolean = false;

  get isInitialized() {
    return this._isInitialized;
  }

  get rootType() {
    return this._rootType ?? '';
  }

  initialize(rootType: string, metadatas: ModelMetadataContainer[]) {
    const rootIndex = metadatas.findIndex((x) => x.type == rootType);
    if (rootIndex < 0) throw Error(`No metadata found for type ${rootType}`);
    this.metadatas = metadatas;
    this._rootType = rootType;
    this.buildTypeDict();
    this._isInitialized = true;
  }

  private buildTypeDict() {
    for (let i = 0; i < this.metadatas.length; i++) this.typeDict.set(this.metadatas[i].type, i);
  }

  lookupByType(type: string | null) {
    if (!type) return undefined;
    const index = this.typeDict.get(type);
    if (index === undefined || index < 0 || index >= this.metadatas.length) return undefined;
    return this.metadatas[index];
  }

  lookupByPath(path: ControlPath | null): PropertyMetadataContainer | undefined {
    let modelMetadata = this.lookupByType(this.rootType);
    let propertyMetadata;

    for (const pathPart of path ?? []) {
      if (typeof pathPart === 'number') continue;
      if (!modelMetadata) return undefined;
      propertyMetadata = modelMetadata.propertyMetadatas[pathPart];
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
    path: ControlPath,
    metadataType: T,
  ): MetadataValueByType<T> | undefined {
    const metadataCollection = this.lookupByPath(path);
    if (!metadataCollection || metadataCollection.$type !== 'primitive') return undefined;
    const propMetadata = metadataCollection.metadatas.find(
      (x): x is Extract<PropertyMetadata, { $type: T }> => x.$type === metadataType,
    );
    return propMetadata?.value as MetadataValueByType<T> | undefined;
  }
}
