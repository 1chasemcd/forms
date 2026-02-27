import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AppDataView } from './data-view';

describe('AppDataView', () => {
  let component: AppDataView;
  let fixture: ComponentFixture<AppDataView>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DataView],
    }).compileComponents();

    fixture = TestBed.createComponent(AppDataView);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
