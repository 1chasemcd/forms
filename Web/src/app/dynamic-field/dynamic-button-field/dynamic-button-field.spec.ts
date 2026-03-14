import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DynamicButtonField } from './dynamic-button-field';

describe('DynamicButtonField', () => {
  let component: DynamicButtonField;
  let fixture: ComponentFixture<DynamicButtonField>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DynamicButtonField],
    }).compileComponents();

    fixture = TestBed.createComponent(DynamicButtonField);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
