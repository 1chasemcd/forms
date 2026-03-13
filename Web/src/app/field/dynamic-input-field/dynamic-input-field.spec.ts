import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DynamicInputField } from './dynamic-input-field';

describe('DynamicInputField', () => {
  let component: DynamicInputField;
  let fixture: ComponentFixture<DynamicInputField>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DynamicInputField],
    }).compileComponents();

    fixture = TestBed.createComponent(DynamicInputField);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
