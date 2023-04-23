import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Group } from '../../models/group';

@Component({
  selector: 'app-groups-table',
  templateUrl: './groups-table.component.html',
  styleUrls: ['./groups-table.component.scss'],
})
export class GroupsTableComponent implements OnInit {
  @Input()
  public groups: Group[];

  @Output()
  deleted: EventEmitter<Group> = new EventEmitter<Group>();

  @Output()
  edited: EventEmitter<Group> = new EventEmitter<Group>();

  public constructor() {}

  public ngOnInit(): void {}
}
