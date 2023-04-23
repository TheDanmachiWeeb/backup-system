import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ConfigsListPageComponent } from './configs-list-page.component';

describe('ConfigsListPageComponent', () => {
  let component: ConfigsListPageComponent;
  let fixture: ComponentFixture<ConfigsListPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ConfigsListPageComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(ConfigsListPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
