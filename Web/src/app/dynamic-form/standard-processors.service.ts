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
import { RecalculateEventService } from '../recalculate-event-service/recalculate-event-service';
import { GridRegistry } from './form-factory';

@Injectable({ providedIn: 'root' })
export class StandardProcessorsService {
  private registry = inject(FormRegistryService);
  private formProcessorService = inject(FormProcessorService);
  private recalculateService = inject(RecalculateEventService);
  private gridRegistry = inject(GridRegistry);

  register() {
    this.registerViewProcessors();
    this.registerFieldProcessors();
    this.registerMetadataProcessors();
  }

  private registerViewProcessors() {
    this.registry.registerViewProcessor('combinedview', {
      process: (view, group, context, parentEnabled) => {
        (view as CombinedViewDefinition).views?.forEach((v) =>
          this.formProcessorService.processView(v, group, context, parentEnabled),
        );
      },
    });

    this.registry.registerViewProcessor('fieldview', {
      process: (view, group, context, parentEnabled) => {
        (view as FieldViewDefinition).fields?.forEach((f) =>
          this.formProcessorService.processField(f, group, context, parentEnabled),
        );
      },
    });

    this.registry.registerViewProcessor('subpropertygridview', {
      process: (view, group, context, parentEnabled) => {
        const gridView = view as SubPropertyGridViewDefinition;
        const formArray = new FormArray<FormGroup>([]);
        group.addControl(gridView.subPropertyName, formArray);

        let gridEnabled = parentEnabled;
        if (gridView.canEdit?.$type === 'constant' && !gridView.canEdit.value)
          gridEnabled = of(false);
        else if (gridView.canEdit?.$type === 'property') {
          const control = context.getOrAddControl(gridView.canEdit.value, group);
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
      process: (field: FieldDefinition, group: FormGroup, context: FormContext) => {
        if (field.type === FieldType.Button) return;
        context.getOrAddControl(field.property, group);
      },
    };

    Object.values(FieldType).forEach((type) => {
      this.registry.registerFieldProcessor(type, standardFieldProcessor);
    });
  }

  private registerMetadataProcessors() {
    this.registry.registerMetadataProcessor(MetadataType.Required, {
      process: (metadata, field, group, context) => {
        const control = context.getOrAddControl(field.property, group);
        this.applyPoc(metadata.value, group, context, (value: unknown) => {
          if (value) control.addValidators(Validators.required);
          else control.removeValidators(Validators.required);
          control.updateValueAndValidity({ emitEvent: false });
        });
      },
    });

    this.registry.registerMetadataProcessor(MetadataType.Enabled, {
      process: (metadata, field, group, context, parentEnabled) => {
        const control = context.getOrAddControl(field.property, group);
        const fieldEnabled = this.getPocObservable(metadata.value, group, context);

        context.untilDestroyed(combineLatest([parentEnabled, fieldEnabled])).subscribe(([p, f]) => {
          const shouldEnable = p && (f as boolean);
          if (shouldEnable && control.disabled) control.enable({ emitEvent: false });
          else if (!shouldEnable && control.enabled) control.disable({ emitEvent: false });
        });
      },
    });

    this.registry.registerMetadataProcessor(MetadataType.MinValue, {
      process: (metadata, field, group, context) => {
        const control = context.getOrAddControl(field.property, group);
        const rangeValidator = this.getOrAddValidator(control, 'range', createRangeValidator);
        this.applyPoc(metadata.value, group, context, (v: unknown) =>
          rangeValidator.setMin(v as string | number),
        );
      },
    });

    this.registry.registerMetadataProcessor(MetadataType.MaxValue, {
      process: (metadata, field, group, context) => {
        const control = context.getOrAddControl(field.property, group);
        const rangeValidator = this.getOrAddValidator(control, 'range', createRangeValidator);
        this.applyPoc(metadata.value, group, context, (v: unknown) =>
          rangeValidator.setMax(v as string | number),
        );
      },
    });

    this.registry.registerMetadataProcessor(MetadataType.MaxLength, {
      process: (metadata, field, group, context) => {
        const control = context.getOrAddControl(field.property, group);
        const validator = this.getOrAddValidator(control, 'maxLength', createMaxLengthValidator);
        this.applyPoc(metadata.value, group, context, (v: unknown) =>
          validator.setMaxLength(v as number),
        );
      },
    });

    this.registry.registerMetadataProcessor(MetadataType.Precision, {
      process: (metadata, field, group, context) => {
        const control = context.getOrAddControl(field.property, group);
        const validator = this.getOrAddValidator(
          control,
          'precisionScale',
          createPrecisionScaleValidator,
        );
        this.applyPoc(metadata.value, group, context, (v: unknown) =>
          validator.setPrecision(v as number),
        );
      },
    });

    this.registry.registerMetadataProcessor(MetadataType.Scale, {
      process: (metadata, field, group, context) => {
        const control = context.getOrAddControl(field.property, group);
        const validator = this.getOrAddValidator(
          control,
          'precisionScale',
          createPrecisionScaleValidator,
        );
        this.applyPoc(metadata.value, group, context, (v: unknown) =>
          validator.setScale(v as number),
        );
      },
    });
  }

  private applyPoc<T>(
    poc: PropertyOrConstant,
    group: FormGroup,
    context: FormContext,
    callback: (value: T) => void,
  ) {
    if (poc.$type === 'constant') {
      callback(poc.value);
    } else {
      const control = context.getOrAddControl(poc.value, group);
      context
        .untilDestroyed(control.valueChanges.pipe(startWith(control.value)))
        .subscribe(callback);
    }
  }

  private getPocObservable(
    poc: PropertyOrConstant,
    group: FormGroup,
    context: FormContext,
  ): Observable<unknown> {
    if (poc.$type === 'constant') return of(poc.value);
    const control = context.getOrAddControl(poc.value, group);
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
