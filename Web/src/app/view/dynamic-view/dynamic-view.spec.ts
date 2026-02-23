import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DynamicView } from './dynamic-view';

describe('DynamicView', () => {
  let component: DynamicView;
  let fixture: ComponentFixture<DynamicView>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DynamicView],
    }).compileComponents();

    fixture = TestBed.createComponent(DynamicView);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
