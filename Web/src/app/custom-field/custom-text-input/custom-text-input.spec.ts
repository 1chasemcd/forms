import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CustomTextInput } from './custom-text-input';

describe('CustomTextInput', () => {
  let component: CustomTextInput;
  let fixture: ComponentFixture<CustomTextInput>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CustomTextInput],
    }).compileComponents();

    fixture = TestBed.createComponent(CustomTextInput);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
