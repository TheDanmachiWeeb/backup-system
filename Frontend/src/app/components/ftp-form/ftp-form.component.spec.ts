import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FtpFormComponent } from './ftp-form.component';

describe('FtpFormComponent', () => {
  let component: FtpFormComponent;
  let fixture: ComponentFixture<FtpFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FtpFormComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FtpFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
