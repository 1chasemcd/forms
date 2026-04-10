import { Injectable } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { combineLatest, map, Observable, of, startWith } from 'rxjs';
import { FieldDefinition, MetadataType, PropertyOrConstant } from '../api/api.g';
import { getOrAddControl } from '../utils/api-utils';

@Injectable()
export class FormEnablementService {
  setControlEnablement(
    fieldDefinition: FieldDefinition,
    formGroup: FormGroup,
    parentEnabled: Observable<boolean> | null = null,
  ) {
    const control = formGroup.get(fieldDefinition.property);
    if (!control) return;
    let fieldEnabled = of(true);
    const enabledMetadata = fieldDefinition.fieldMetadatas?.find(
      (x) => x.type == MetadataType.Enabled,
    )?.value as PropertyOrConstant | undefined;
    if (enabledMetadata && enabledMetadata.$type === 'constant' && !enabledMetadata.value)
      fieldEnabled = of(false);
    else if (enabledMetadata && enabledMetadata.$type === 'property') {
      const enabledControl = getOrAddControl(enabledMetadata.value, formGroup);
      fieldEnabled = enabledControl.valueChanges.pipe(startWith(enabledControl.value));
    }

    const setEnablement = (shouldEnable: boolean) => {
      if (shouldEnable && control.disabled) {
        control.enable({ emitEvent: false });
      } else if (!shouldEnable && control.enabled) {
        control.disable({ emitEvent: false });
      }
    };

    if (parentEnabled === null) fieldEnabled.subscribe(setEnablement);
    else
      combineLatest([parentEnabled, fieldEnabled])
        .pipe(map(([x, y]) => x && y))
        .subscribe(setEnablement);
  }
}
