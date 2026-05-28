import { inject, Injectable } from '@angular/core';
import { MetadataProcessorRegistryService } from './metadata-processor-registry-service';
import { ControlEnablementService } from '../form-services/control-enablement-service';
import { ControlValueService } from '../form-services/control-value-service';
import { Validators } from '@angular/forms';
import {
  createMaxLengthValidator,
  createMaxValueValidator,
  createMinValueValidator,
  createPrecisionValidator,
  createScaleValidator,
} from '../utils/validators';

@Injectable()
export class DefaultMetadataProcessorsService {
  private readonly registry = inject(MetadataProcessorRegistryService);
  private readonly enablementService = inject(ControlEnablementService);
  private readonly controlValues = inject(ControlValueService);

  initialize() {
    this.registry.registerMetadataProcessor('required', {
      process: (control, metadata) => {
        this.controlValues.observe(control.parent, metadata.value)?.subscribe((value) => {
          if (value) control.addValidators(Validators.required);
          else control.removeValidators(Validators.required);
          control.updateValueAndValidity({ emitEvent: false });
        });
      },
    });

    this.registry.registerMetadataProcessor('enabled', {
      process: (control, metadata) => {
        const fieldEnabled = this.controlValues.observe(control.parent, metadata.value);
        this.enablementService.enabledFor(control, fieldEnabled);
      },
    });

    this.registry.registerMetadataProcessor('minValue', {
      process: (control, metadata) => {
        const validator = createMinValueValidator();
        this.controlValues.observe(control.parent, metadata.value)?.subscribe((value) => {
          validator.setMin(value as string | number);
          control.updateValueAndValidity({ emitEvent: false });
        });
      },
    });

    this.registry.registerMetadataProcessor('maxValue', {
      process: (control, metadata) => {
        const validator = createMaxValueValidator();
        this.controlValues.observe(control.parent, metadata.value)?.subscribe((value) => {
          validator.setMax(value as string | number);
          control.updateValueAndValidity({ emitEvent: false });
        });
      },
    });

    this.registry.registerMetadataProcessor('maxLength', {
      process: (control, metadata) => {
        const validator = createMaxLengthValidator();
        this.controlValues.observe(control.parent, metadata.value)?.subscribe((value) => {
          validator.setMaxLength(value as number);
          control.updateValueAndValidity({ emitEvent: false });
        });
      },
    });

    this.registry.registerMetadataProcessor('precision', {
      process: (control, metadata) => {
        const validator = createPrecisionValidator();
        this.controlValues.observe(control.parent, metadata.value)?.subscribe((value) => {
          validator.setPrecision(value as number);
          control.updateValueAndValidity({ emitEvent: false });
        });
      },
    });

    this.registry.registerMetadataProcessor('scale', {
      process: (control, metadata) => {
        const validator = createScaleValidator();
        this.controlValues.observe(control.parent, metadata.value)?.subscribe((value) => {
          validator.setScale(value as number);
          control.updateValueAndValidity({ emitEvent: false });
        });
      },
    });
  }
}
