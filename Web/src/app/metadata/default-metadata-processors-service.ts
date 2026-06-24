import { inject, Injectable } from '@angular/core';
import { MetadataProcessorRegistryService } from './metadata-processor-registry-service';
import { Validators } from '@angular/forms';
import {
  createMaxLengthValidator,
  createMaxValueValidator,
  createMinValueValidator,
  createPrecisionValidator,
  createScaleValidator,
} from '../utils/validators';
import { parentPath } from '../utils/form-utils';

@Injectable()
export class DefaultMetadataProcessorsService {
  private readonly registry = inject(MetadataProcessorRegistryService);

  initialize() {
    this.registry.registerMetadataProcessor('required', {
      process: (payload) => {
        payload.model.valueRefAugmentor
          .getValue(parentPath(payload.controlPath), payload.metadata.value)
          ?.subscribe((value) => {
            if (value) payload.control.addValidators(Validators.required);
            else payload.control.removeValidators(Validators.required);
            payload.control.updateValueAndValidity({ emitEvent: false });
          });
      },
    });

    this.registry.registerMetadataProcessor('enabled', {
      process: (payload) => {
        const fieldEnabled = payload.model.valueRefAugmentor.getValue(
          parentPath(payload.controlPath),
          payload.metadata.value,
        );
        payload.model.controlEnablements.enabledFor(payload.control, fieldEnabled);
      },
    });

    this.registry.registerMetadataProcessor('minValue', {
      process: (payload) => {
        const validator = createMinValueValidator();
        payload.model.valueRefAugmentor
          .getValue(parentPath(payload.controlPath), payload.metadata.value)
          ?.subscribe((value) => {
            validator.setMin(value as string | number);
            payload.control.updateValueAndValidity({ emitEvent: false });
          });
      },
    });

    this.registry.registerMetadataProcessor('maxValue', {
      process: (payload) => {
        const validator = createMaxValueValidator();
        payload.model.valueRefAugmentor
          .getValue(parentPath(payload.controlPath), payload.metadata.value)
          ?.subscribe((value) => {
            validator.setMax(value as string | number);
            payload.control.updateValueAndValidity({ emitEvent: false });
          });
      },
    });

    this.registry.registerMetadataProcessor('maxLength', {
      process: (payload) => {
        const validator = createMaxLengthValidator();
        payload.model.valueRefAugmentor
          .getValue(parentPath(payload.controlPath), payload.metadata.value)
          ?.subscribe((value) => {
            validator.setMaxLength(value as number);
            payload.control.updateValueAndValidity({ emitEvent: false });
          });
      },
    });

    this.registry.registerMetadataProcessor('precision', {
      process: (payload) => {
        const validator = createPrecisionValidator();
        payload.model.valueRefAugmentor
          .getValue(parentPath(payload.controlPath), payload.metadata.value)
          ?.subscribe((value) => {
            validator.setPrecision(value as number);
            payload.control.updateValueAndValidity({ emitEvent: false });
          });
      },
    });

    this.registry.registerMetadataProcessor('scale', {
      process: (payload) => {
        const validator = createScaleValidator();
        payload.model.valueRefAugmentor
          .getValue(parentPath(payload.controlPath), payload.metadata.value)
          ?.subscribe((value) => {
            validator.setScale(value as number);
            payload.control.updateValueAndValidity({ emitEvent: false });
          });
      },
    });
  }
}
