import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DynamicTextField } from './dynamic-text-field';

describe('DynamicTextField', () => {
  let component: DynamicTextField;
  let fixture: ComponentFixture<DynamicTextField>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DynamicTextField],
    }).compileComponents();

    fixture = TestBed.createComponent(DynamicTextField);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
