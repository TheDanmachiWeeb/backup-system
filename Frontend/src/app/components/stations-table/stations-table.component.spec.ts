import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StationsTableComponent } from './stations-table.component';

describe('StationsTableComponent', () => {
  let component: StationsTableComponent;
  let fixture: ComponentFixture<StationsTableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ StationsTableComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StationsTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
