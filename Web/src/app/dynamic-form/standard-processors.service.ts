import { inject, Injectable } from '@angular/core';
import { FormRegistryService } from './form-registry-service';
import {
  CombinedViewDefinition,
  FieldDefinition,
  FieldType,
  FieldViewDefinition,
  MetadataType,
  PropertyOrConstant,
  SubPropertyGridViewDefinition,
} from '../api/api.g';
import { FormProcessorService } from './form-processor-service';
import { FormContext } from './form-context';
import { combineLatest, map, Observable, of, startWith } from 'rxjs';
import { FormArray, FormGroup, Validators } from '@angular/forms';
import {
  createMaxLengthValidator,
  createPrecisionScaleValidator,
  createRangeValidator,
} from '../utils/validators';
import { GridRegistry } from './form-factory';

@Injectable({ providedIn: 'root' })
export class StandardProcessorsService {
  private registry = inject(FormRegistryService);
  private formProcessorService = inject(FormProcessorService);
  private gridRegistry = inject(GridRegistry);

  register() {
    this.registerViewProcessors();
    this.registerFieldProcessors();
    this.registerMetadataProcessors();
  }

  private registerViewProcessors() {
    this.registry.registerViewProcessor('combinedview', {
      process: (view, context, parentEnabled) => {
        (view as CombinedViewDefinition).views?.forEach((v) =>
          this.formProcessorService.processView(v, context, parentEnabled),
        );
      },
    });

    this.registry.registerViewProcessor('fieldview', {
      process: (view, context, parentEnabled) => {
        (view as FieldViewDefinition).fields?.forEach((f) =>
          this.formProcessorService.processField(f, context, parentEnabled),
        );
      },
    });

    this.registry.registerViewProcessor('subpropertygridview', {
      process: (view, context, parentEnabled) => {
        const gridView = view as SubPropertyGridViewDefinition;
        const formArray = new FormArray<FormGroup>([]);
        context.formGroup.addControl(gridView.subPropertyName, formArray);

        let gridEnabled = parentEnabled;
        if (gridView.canEdit?.$type === 'constant' && !gridView.canEdit.value)
          gridEnabled = of(false);
        else if (gridView.canEdit?.$type === 'property') {
          const control = context.getOrAddControl(gridView.canEdit.value);
          gridEnabled = combineLatest([
            parentEnabled,
            control.valueChanges.pipe(startWith(control.value)),
          ]).pipe(map(([p, c]) => p && c));
        }

        this.gridRegistry.register(gridView.subPropertyName, gridView, gridEnabled);
      },
    });
  }

  private registerFieldProcessors() {
    const standardFieldProcessor = {
      process: (field: FieldDefinition, context: FormContext) => {
        if (field.type === FieldType.Button) return;
        context.getOrAddControl(field.property);
      },
    };

    Object.values(FieldType).forEach((type) => {
      this.registry.registerFieldProcessor(type, standardFieldProcessor);
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
      process: (metadata, field, context, parentEnabled) => {
        const control = context.getOrAddControl(field.property);
        const fieldEnabled = this.getPocObservable(metadata.value, context);

        context.untilDestroyed(combineLatest([parentEnabled, fieldEnabled])).subscribe(([p, f]) => {
          const shouldEnable = p && (f as boolean);
          if (shouldEnable && control.disabled) control.enable({ emitEvent: false });
          else if (!shouldEnable && control.enabled) control.disable({ emitEvent: false });
        });
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

  private getPocObservable(poc: PropertyOrConstant, context: FormContext): Observable<unknown> {
    if (poc.$type === 'constant') return of(poc.value);
    const control = context.getOrAddControl(poc.value);
    return control.valueChanges.pipe(startWith(control.value));
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
