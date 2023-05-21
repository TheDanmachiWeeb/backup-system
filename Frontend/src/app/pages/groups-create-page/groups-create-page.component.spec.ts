import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GroupsCreatePageComponent } from './groups-create-page.component';

describe('GroupsCreatePageComponent', () => {
  let component: GroupsCreatePageComponent;
  let fixture: ComponentFixture<GroupsCreatePageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GroupsCreatePageComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GroupsCreatePageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
