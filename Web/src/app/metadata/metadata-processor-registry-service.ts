import { inject, Injectable } from '@angular/core';
import { PropertyMetadata } from '../api/api.g';
import { AbstractControl, FormGroup, Validators } from '@angular/forms';
import { getPocObservable, MetadataByType, MetadataType } from '../utils/api-utils';
import { ControlEnablementService } from '../form-services/control-enablement-service';
import {
  createMaxLengthValidator,
  createMaxValueValidator,
  createMinValueValidator,
  createPrecisionValidator,
  createScaleValidator,
} from '../utils/validators';

export interface MetadataProcessor<T extends PropertyMetadata> {
  process(control: AbstractControl, formGroup: FormGroup, metadata: T): void;
}

@Injectable()
export class MetadataProcessorRegistryService {
  private enablementService = inject(ControlEnablementService);

  private metadataProcessors = new Map<string, MetadataProcessor<PropertyMetadata>>();

  private registerMetadataProcessor<T extends MetadataType>(
    type: T,
    processor: MetadataProcessor<MetadataByType<T>>,
  ) {
    this.metadataProcessors.set(type, processor);
  }

  getMetadataProcessor(
    metadata: PropertyMetadata,
  ): MetadataProcessor<PropertyMetadata> | undefined {
    return this.metadataProcessors.get(metadata.$type);
  }

  initialize() {
    this.registerMetadataProcessor('required', {
      process: (control, formGroup, metadata) => {
        getPocObservable(metadata.value, formGroup).subscribe((value) => {
          if (value) control.addValidators(Validators.required);
          else control.removeValidators(Validators.required);
          control.updateValueAndValidity({ emitEvent: false });
        });
      },
    });

    this.registerMetadataProcessor('enabled', {
      process: (control, formGroup, metadata) => {
        const fieldEnabled = getPocObservable(metadata.value, formGroup);
        this.enablementService.enabledFor(control, fieldEnabled);
      },
    });

    this.registerMetadataProcessor('minValue', {
      process: (control, formGroup, metadata) => {
        const validator = createMinValueValidator();
        getPocObservable(metadata.value, formGroup).subscribe((value) => {
          validator.setMin(value as string | number);
          control.updateValueAndValidity({ emitEvent: false });
        });
      },
    });

    this.registerMetadataProcessor('maxValue', {
      process: (control, formGroup, metadata) => {
        const validator = createMaxValueValidator();
        getPocObservable(metadata.value, formGroup).subscribe((value) => {
          validator.setMax(value as string | number);
          control.updateValueAndValidity({ emitEvent: false });
        });
      },
    });

    this.registerMetadataProcessor('maxLength', {
      process: (control, formGroup, metadata) => {
        const validator = createMaxLengthValidator();
        getPocObservable(metadata.value, formGroup).subscribe((value) => {
          validator.setMaxLength(value as number);
          control.updateValueAndValidity({ emitEvent: false });
        });
      },
    });

    this.registerMetadataProcessor('precision', {
      process: (control, formGroup, metadata) => {
        const validator = createPrecisionValidator();
        getPocObservable(metadata.value, formGroup).subscribe((value) => {
          validator.setPrecision(value as number);
          control.updateValueAndValidity({ emitEvent: false });
        });
      },
    });

    this.registerMetadataProcessor('scale', {
      process: (control, formGroup, metadata) => {
        const validator = createScaleValidator();
        getPocObservable(metadata.value, formGroup).subscribe((value) => {
          validator.setScale(value as number);
          control.updateValueAndValidity({ emitEvent: false });
        });
      },
    });
  }
}
