import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GroupsEditPageComponent } from './groups-edit-page.component';

describe('GroupsEditPageComponent', () => {
  let component: GroupsEditPageComponent;
  let fixture: ComponentFixture<GroupsEditPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GroupsEditPageComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GroupsEditPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
