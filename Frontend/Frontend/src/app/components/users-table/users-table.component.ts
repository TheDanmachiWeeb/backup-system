import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {User} from "../../models/user";

@Component({
  selector: 'app-users-table',
  templateUrl: './users-table.component.html',
  styleUrls: ['./users-table.component.scss']
})
export class UsersTableComponent implements OnInit {

  @Input()
  public users: User[];

  @Output()
  deleted: EventEmitter<User> = new EventEmitter<User>();

  @Output()
  edited: EventEmitter<User> = new EventEmitter<User>();

  public constructor() {

  }

  public ngOnInit(): void {

  }
}
