import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StationsListPageComponent } from './stations-list-page.component';

describe('StationsListPageComponent', () => {
  let component: StationsListPageComponent;
  let fixture: ComponentFixture<StationsListPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [StationsListPageComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(StationsListPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
