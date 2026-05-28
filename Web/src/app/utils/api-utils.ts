import { PropertyMetadata } from '../api/api.g';

export type MetadataType = PropertyMetadata['$type'];
export type MetadataByType<TType extends MetadataType> = Extract<
  PropertyMetadata,
  { $type: TType }
>;

export type MetadataValueByType<TType extends MetadataType> = Extract<
  PropertyMetadata,
  { $type: TType }
>['value'];
