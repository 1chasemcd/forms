import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CustomStaticText } from './custom-static-text';

describe('CustomStaticText', () => {
  let component: CustomStaticText;
  let fixture: ComponentFixture<CustomStaticText>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CustomStaticText],
    }).compileComponents();

    fixture = TestBed.createComponent(CustomStaticText);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
