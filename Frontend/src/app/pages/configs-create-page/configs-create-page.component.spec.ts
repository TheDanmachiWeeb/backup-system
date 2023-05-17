import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ConfigsCreatePageComponent } from './configs-create-page.component';

describe('ConfigsCreatePageComponent', () => {
  let component: ConfigsCreatePageComponent;
  let fixture: ComponentFixture<ConfigsCreatePageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ConfigsCreatePageComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ConfigsCreatePageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
