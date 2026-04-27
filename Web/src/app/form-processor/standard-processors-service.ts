import { inject, Injectable } from '@angular/core';
import { FormRegistryService } from './form-registry-service';
import {
  CombinedViewDefinition,
  FieldViewDefinition,
  MetadataType,
  PropertyOrConstant,
  SubPropertyGridViewDefinition,
} from '../api/api.g';
import { FormProcessorService } from './form-processor-service';
import { FormContext } from '../dynamic-form/form-context';
import { of, startWith } from 'rxjs';
import { FormArray, FormGroup, Validators } from '@angular/forms';
import {
  createMaxLengthValidator,
  createPrecisionScaleValidator,
  createRangeValidator,
} from '../utils/validators';
import { GridRowFactory } from '../dynamic-form/grid-row-factory';
import { FormFieldEnablementService } from './form-field-enablement-service';
import { getPocObservable } from '../utils/api-utils';

@Injectable({ providedIn: 'root' })
export class StandardProcessorsService {
  private registry = inject(FormRegistryService);
  private formProcessorService = inject(FormProcessorService);
  private gridRowFactory = inject(GridRowFactory);
  private enablementService = inject(FormFieldEnablementService);

  register() {
    this.registerViewProcessors();
    this.registerMetadataProcessors();
  }

  private registerViewProcessors() {
    this.registry.registerViewProcessor('combinedview', {
      process: (view, context) => {
        (view as CombinedViewDefinition).views?.forEach((v) => {
          this.enablementService.enabledForParent(v, view);
          this.formProcessorService.processView(v, context);
        });
      },
    });

    this.registry.registerViewProcessor('fieldview', {
      process: (view, context) => {
        (view as FieldViewDefinition).fields?.forEach((f) => {
          const fieldControl = this.formProcessorService.processField(f, context);
          this.enablementService.enabledForParent(f, view);
          if (fieldControl) this.enablementService.enabledForParent(fieldControl, f);
        });
      },
    });

    this.registry.registerViewProcessor('subpropertygridview', {
      process: (view, context) => {
        const gridView = view as SubPropertyGridViewDefinition;
        const formArray = new FormArray<FormGroup>([]);
        context.formGroup.addControl(gridView.subPropertyName, formArray);
        if (gridView.editForm) this.enablementService.enabledFor(gridView, of(false));
        else if (gridView.canEdit)
          this.enablementService.enabledFor(gridView, getPocObservable(gridView.canEdit, context));

        this.gridRowFactory.register(gridView.subPropertyName, gridView, context);
      },
    });
  }

  private registerMetadataProcessors() {
    this.registry.registerMetadataProcessor(MetadataType.Required, {
      process: (metadata, field, context) => {
        const control = context.getOrAddControl(field.property);
        this.applyPoc(metadata.value, context, (value: unknown) => {
          if (value) control.addValidators(Validators.required);
          else control.removeValidators(Validators.required);
          control.updateValueAndValidity({ emitEvent: false });
        });
      },
    });

    this.registry.registerMetadataProcessor(MetadataType.Enabled, {
      process: (metadata, field, context) => {
        const control = context.getOrAddControl(field.property);
        const fieldEnabled = getPocObservable(metadata.value, context);
        this.enablementService.enabledFor(control, fieldEnabled);
      },
    });

    this.registry.registerMetadataProcessor(MetadataType.MinValue, {
      process: (metadata, field, context) => {
        const control = context.getOrAddControl(field.property);
        const rangeValidator = this.getOrAddValidator(control, 'range', createRangeValidator);
        this.applyPoc(metadata.value, context, (v: unknown) =>
          rangeValidator.setMin(v as string | number),
        );
      },
    });

    this.registry.registerMetadataProcessor(MetadataType.MaxValue, {
      process: (metadata, field, context) => {
        const control = context.getOrAddControl(field.property);
        const rangeValidator = this.getOrAddValidator(control, 'range', createRangeValidator);
        this.applyPoc(metadata.value, context, (v: unknown) =>
          rangeValidator.setMax(v as string | number),
        );
      },
    });

    this.registry.registerMetadataProcessor(MetadataType.MaxLength, {
      process: (metadata, field, context) => {
        const control = context.getOrAddControl(field.property);
        const validator = this.getOrAddValidator(control, 'maxLength', createMaxLengthValidator);
        this.applyPoc(metadata.value, context, (v: unknown) => validator.setMaxLength(v as number));
      },
    });

    this.registry.registerMetadataProcessor(MetadataType.Precision, {
      process: (metadata, field, context) => {
        const control = context.getOrAddControl(field.property);
        const validator = this.getOrAddValidator(
          control,
          'precisionScale',
          createPrecisionScaleValidator,
        );
        this.applyPoc(metadata.value, context, (v: unknown) => validator.setPrecision(v as number));
      },
    });

    this.registry.registerMetadataProcessor(MetadataType.Scale, {
      process: (metadata, field, context) => {
        const control = context.getOrAddControl(field.property);
        const validator = this.getOrAddValidator(
          control,
          'precisionScale',
          createPrecisionScaleValidator,
        );
        this.applyPoc(metadata.value, context, (v: unknown) => validator.setScale(v as number));
      },
    });
  }

  private applyPoc<T>(poc: PropertyOrConstant, context: FormContext, callback: (value: T) => void) {
    if (poc.$type === 'constant') {
      callback(poc.value);
    } else {
      const control = context.getOrAddControl(poc.value);
      context
        .untilDestroyed(control.valueChanges.pipe(startWith(control.value)))
        .subscribe(callback);
    }
  }

  private validatorCache = new WeakMap<object, Record<string, unknown>>();

  private getOrAddValidator<T>(
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    control: any,
    key: string,
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    factory: () => T & { validator: any },
  ): T {
    let controlCache = this.validatorCache.get(control);
    if (!controlCache) {
      controlCache = {};
      this.validatorCache.set(control, controlCache);
    }

    if (!controlCache[key]) {
      const instance = factory();
      controlCache[key] = instance as unknown;
      control.addValidators(instance.validator);
    }

    return controlCache[key] as T;
  }
}
