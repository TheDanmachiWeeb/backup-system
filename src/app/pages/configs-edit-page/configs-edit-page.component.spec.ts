import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ConfigsEditPageComponent } from './configs-edit-page.component';

describe('ConfigsEditPageComponent', () => {
  let component: ConfigsEditPageComponent;
  let fixture: ComponentFixture<ConfigsEditPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ConfigsEditPageComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(ConfigsEditPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
