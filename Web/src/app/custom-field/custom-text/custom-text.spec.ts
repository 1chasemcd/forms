import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CustomText } from './custom-text';

describe('StaticTextField', () => {
  let component: CustomText;
  let fixture: ComponentFixture<CustomText>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CustomText],
    }).compileComponents();

    fixture = TestBed.createComponent(CustomText);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
