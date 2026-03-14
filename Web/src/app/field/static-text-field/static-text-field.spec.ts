import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StaticTextField } from './static-text-field';

describe('StaticTextField', () => {
  let component: StaticTextField;
  let fixture: ComponentFixture<StaticTextField>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [StaticTextField],
    }).compileComponents();

    fixture = TestBed.createComponent(StaticTextField);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
