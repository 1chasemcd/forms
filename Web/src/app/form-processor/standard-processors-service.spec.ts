import { TestBed } from '@angular/core/testing';
import { StandardProcessorsService } from './standard-processors-service';
import { FormRegistryService } from './form-registry-service';
import { FormProcessorService } from './form-processor-service';
import { GridRowFactory } from '../dynamic-form/grid-row-factory';
import { FormFieldEnablementService } from './form-field-enablement-service';
import { FormContext } from '../dynamic-form/form-context';
import { DestroyRef } from '@angular/core';
import { FormArray, FormControl, FormGroup, Validators } from '@angular/forms';
import {
  CombinedViewDefinition,
  FieldViewDefinition,
  MetadataType,
  PropertyOrConstant,
  SubPropertyGridViewDefinition,
  MetadataDefinition,
  FieldDefinition,
  FieldType,
  FormDefinition,
} from '../api/api.g';
import { of } from 'rxjs';

describe('StandardProcessorsService', () => {
  let service: StandardProcessorsService;
  let registrySpy: jasmine.SpyObj<FormRegistryService>;
  let formProcessorSpy: jasmine.SpyObj<FormProcessorService>;
  let gridRowFactorySpy: jasmine.SpyObj<GridRowFactory>;
  let enablementSpy: jasmine.SpyObj<FormFieldEnablementService>;
  let formContext: FormContext;
  let mockDestroyRef: jasmine.SpyObj<DestroyRef>;

  beforeEach(() => {
    registrySpy = jasmine.createSpyObj('FormRegistryService', [
      'registerViewProcessor',
      'registerMetadataProcessor',
    ]);
    formProcessorSpy = jasmine.createSpyObj('FormProcessorService', [
      'processView',
      'processField',
    ]);
    gridRowFactorySpy = jasmine.createSpyObj('GridRowFactory', ['register']);
    enablementSpy = jasmine.createSpyObj('FormFieldEnablementService', [
      'enabledForParent',
      'enabledFor',
    ]);
    mockDestroyRef = jasmine.createSpyObj('DestroyRef', ['onDestroy']);

    TestBed.configureTestingModule({
      providers: [
        StandardProcessorsService,
        { provide: FormRegistryService, useValue: registrySpy },
        { provide: FormProcessorService, useValue: formProcessorSpy },
        { provide: GridRowFactory, useValue: gridRowFactorySpy },
        { provide: FormFieldEnablementService, useValue: enablementSpy },
        { provide: DestroyRef, useValue: mockDestroyRef },
      ],
    });

    service = TestBed.inject(StandardProcessorsService);
    formContext = new FormContext(mockDestroyRef, new FormGroup({}));
  });

  it('should register view and metadata processors', () => {
    service.register();
    expect(registrySpy.registerViewProcessor).toHaveBeenCalledWith(
      'combinedview',
      jasmine.any(Object),
    );
    expect(registrySpy.registerViewProcessor).toHaveBeenCalledWith(
      'fieldview',
      jasmine.any(Object),
    );
    expect(registrySpy.registerViewProcessor).toHaveBeenCalledWith(
      'subpropertygridview',
      jasmine.any(Object),
    );
    expect(registrySpy.registerMetadataProcessor).toHaveBeenCalledWith(
      MetadataType.Required,
      jasmine.any(Object),
    );
    expect(registrySpy.registerMetadataProcessor).toHaveBeenCalledWith(
      MetadataType.Enabled,
      jasmine.any(Object),
    );
    expect(registrySpy.registerMetadataProcessor).toHaveBeenCalledWith(
      MetadataType.MinValue,
      jasmine.any(Object),
    );
    expect(registrySpy.registerMetadataProcessor).toHaveBeenCalledWith(
      MetadataType.MaxValue,
      jasmine.any(Object),
    );
    expect(registrySpy.registerMetadataProcessor).toHaveBeenCalledWith(
      MetadataType.MaxLength,
      jasmine.any(Object),
    );
    expect(registrySpy.registerMetadataProcessor).toHaveBeenCalledWith(
      MetadataType.Precision,
      jasmine.any(Object),
    );
    expect(registrySpy.registerMetadataProcessor).toHaveBeenCalledWith(
      MetadataType.Scale,
      jasmine.any(Object),
    );
  });

  describe('View Processors', () => {
    beforeEach(() => service.register());

    it('should process combinedview', () => {
      const processor = registrySpy.registerViewProcessor.calls.argsFor(0)[1];
      const view: CombinedViewDefinition = {
        $type: 'combinedview',
        views: [{ $type: 'fieldview', fields: [] }],
        unify: false,
      };

      processor.process(view, formContext);

      expect(enablementSpy.enabledForParent).toHaveBeenCalledWith(view.views[0], view);
      expect(formProcessorSpy.processView).toHaveBeenCalledWith(view.views[0], formContext);
    });

    it('should process fieldview', () => {
      const processor = registrySpy.registerViewProcessor.calls.argsFor(1)[1];
      const field = { property: 'test' } as FieldDefinition;
      const view: FieldViewDefinition = {
        $type: 'fieldview',
        fields: [field],
      };
      const mockControl = new FormControl();
      formProcessorSpy.processField.and.returnValue(mockControl);

      processor.process(view, formContext);

      expect(formProcessorSpy.processField).toHaveBeenCalledWith(field, formContext);
      expect(enablementSpy.enabledForParent).toHaveBeenCalledWith(field, view);
      expect(enablementSpy.enabledForParent).toHaveBeenCalledWith(mockControl, field);
    });

    it('should process subpropertygridview', () => {
      const processor = registrySpy.registerViewProcessor.calls.argsFor(2)[1];
      const view: SubPropertyGridViewDefinition = {
        $type: 'subpropertygridview',
        subPropertyName: 'grid',
        idProperty: 'id',
        fields: [],
        canEdit: { $type: 'constant', value: true } as PropertyOrConstant,
      };

      processor.process(view, formContext);

      expect(formContext.formGroup.get('grid')).toBeInstanceOf(FormArray);
      expect(enablementSpy.enabledFor).toHaveBeenCalledWith(
        view,
        jasmine.any(of(true).constructor),
      );
      expect(gridRowFactorySpy.register).toHaveBeenCalledWith('grid', view, formContext);
    });

    it('should disable subpropertygridview if editForm is present', () => {
      const processor = registrySpy.registerViewProcessor.calls.argsFor(2)[1];
      const view: SubPropertyGridViewDefinition = {
        $type: 'subpropertygridview',
        subPropertyName: 'gridEdit',
        idProperty: 'id',
        fields: [],
        editForm: { view: {} } as FormDefinition,
      };

      processor.process(view, formContext);

      expect(enablementSpy.enabledFor).toHaveBeenCalledWith(
        view,
        jasmine.any(of(false).constructor),
      );
    });
  });

  describe('Metadata Processors', () => {
    beforeEach(() => service.register());

    it('should process Required metadata (constant)', () => {
      const processor = registrySpy.registerMetadataProcessor.calls
        .all()
        .find((c) => c.args[0] === MetadataType.Required)?.args[1];
      if (!processor) return fail();

      const metadata: MetadataDefinition = {
        type: MetadataType.Required,
        value: { $type: 'constant', value: true } as PropertyOrConstant,
      };
      const field = { property: 'test' } as FieldDefinition;
      const control = formContext.getOrAddControl('test');

      processor.process(metadata, field, formContext);

      expect(control.hasValidator(Validators.required)).toBeTrue();

      // Test toggle off
      metadata.value.value = false;
      processor.process(metadata, field, formContext);
      expect(control.hasValidator(Validators.required)).toBeFalse();
    });

    it('should process Required metadata (property)', () => {
      const processor = registrySpy.registerMetadataProcessor.calls
        .all()
        .find((c) => c.args[0] === MetadataType.Required)?.args[1];
      if (!processor) return fail();
      const metadata: MetadataDefinition = {
        type: MetadataType.Required,
        value: { $type: 'property', value: 'reqTrigger' } as PropertyOrConstant,
      };
      const field = { property: 'test' } as FieldDefinition;
      const control = formContext.getOrAddControl('test');
      const trigger = formContext.getOrAddControl('reqTrigger');

      processor.process(metadata, field, formContext);

      trigger.setValue(true);
      expect(control.hasValidator(Validators.required)).toBeTrue();

      trigger.setValue(false);
      expect(control.hasValidator(Validators.required)).toBeFalse();
    });

    it('should process Enabled metadata', () => {
      const processor = registrySpy.registerMetadataProcessor.calls
        .all()
        .find((c) => c.args[0] === MetadataType.Enabled)?.args[1];
      if (!processor) return fail();

      const metadata: MetadataDefinition = {
        type: MetadataType.Enabled,
        value: { $type: 'constant', value: true } as PropertyOrConstant,
      };
      const field = { property: 'test' } as FieldDefinition;
      const control = formContext.getOrAddControl('test');

      processor.process(metadata, field, formContext);

      expect(enablementSpy.enabledFor).toHaveBeenCalledWith(control, jasmine.any(Object));
    });

    it('should process MinValue/MaxValue metadata', () => {
      const minProcessor = registrySpy.registerMetadataProcessor.calls
        .all()
        .find((c) => c.args[0] === MetadataType.MinValue)?.args[1];
      if (!minProcessor) return fail();

      const maxProcessor = registrySpy.registerMetadataProcessor.calls
        .all()
        .find((c) => c.args[0] === MetadataType.MaxValue)?.args[1];
      if (!maxProcessor) return fail();

      const field: FieldDefinition = { property: 'test', type: FieldType.Text };
      const control = formContext.getOrAddControl('test');

      minProcessor.process(
        { type: MetadataType.MinValue, value: { $type: 'constant', value: 10 } },
        field,
        formContext,
      );
      maxProcessor.process(
        { type: MetadataType.MaxValue, value: { $type: 'constant', value: 20 } },
        field,
        formContext,
      );

      if (!control.validator) return fail();
      const errors = control.validator(new FormControl(5));
      expect(errors?.['minValue']).toBeDefined();

      const errors2 = control.validator(new FormControl(25));
      expect(errors2?.['maxValue']).toBeDefined();

      const errors3 = control.validator(new FormControl(15));
      expect(errors3).toBeNull();
    });

    it('should process MaxLength metadata', () => {
      const processor = registrySpy.registerMetadataProcessor.calls
        .all()
        .find((c) => c.args[0] === MetadataType.MaxLength)?.args[1];
      if (!processor) return fail();

      const field = { property: 'test' } as FieldDefinition;
      const control = formContext.getOrAddControl('test');

      processor.process(
        { type: MetadataType.MaxLength, value: { $type: 'constant', value: 5 } },
        field,
        formContext,
      );

      if (!control.validator) return fail();
      expect(control.validator({ value: '123456' } as FormControl)).toBeDefined();
      expect(control.validator({ value: '12345' } as FormControl)).toBeNull();
    });

    it('should process Precision/Scale metadata', () => {
      const pProcessor = registrySpy.registerMetadataProcessor.calls
        .all()
        .find((c) => c.args[0] === MetadataType.Precision)?.args[1];
      if (!pProcessor) return fail();
      const sProcessor = registrySpy.registerMetadataProcessor.calls
        .all()
        .find((c) => c.args[0] === MetadataType.Scale)?.args[1];
      if (!sProcessor) return fail();

      const field = { property: 'test' } as FieldDefinition;
      const control = formContext.getOrAddControl('test');

      pProcessor.process(
        { type: MetadataType.Precision, value: { $type: 'constant', value: 5 } },
        field,
        formContext,
      );
      sProcessor.process(
        { type: MetadataType.Scale, value: { $type: 'constant', value: 2 } },
        field,
        formContext,
      );

      if (!control.validator) return fail();

      // 123.45 is precision 5, scale 2 -> valid
      expect(control.validator({ value: 123.45 } as FormControl)).toBeNull();
      // 1234.45 is precision 6 -> invalid
      expect(control.validator({ value: 1234.45 } as FormControl)?.['precision']).toBeDefined();
      // 123.456 is scale 3 -> invalid
      expect(control.validator({ value: 123.456 } as FormControl)?.['scale']).toBeDefined();
    });

    it('should reuse validators from cache', () => {
      const pProcessor = registrySpy.registerMetadataProcessor.calls
        .all()
        .find((c) => c.args[0] === MetadataType.Precision)?.args[1];
      if (!pProcessor) return fail();
      const sProcessor = registrySpy.registerMetadataProcessor.calls
        .all()
        .find((c) => c.args[0] === MetadataType.Scale)?.args[1];
      if (!sProcessor) return fail();

      const field = { property: 'test' } as FieldDefinition;
      const control = formContext.getOrAddControl('test');
      spyOn(control, 'addValidators').and.callThrough();

      pProcessor.process(
        { type: MetadataType.Precision, value: { $type: 'constant', value: 5 } },
        field,
        formContext,
      );
      sProcessor.process(
        { type: MetadataType.Scale, value: { $type: 'constant', value: 2 } },
        field,
        formContext,
      );

      // addValidators should only be called once for 'precisionScale' validator
      expect(control.addValidators).toHaveBeenCalledTimes(1);
    });
  });
});
