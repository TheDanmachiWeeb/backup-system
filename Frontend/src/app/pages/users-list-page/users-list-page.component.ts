import {Component, OnInit} from '@angular/core';
import {UsersService} from "../../services/users.service";
import {User} from "../../models/user";

@Component({
  selector: 'app-users-list-page',
  templateUrl: './users-list-page.component.html',
  styleUrls: ['./users-list-page.component.scss']
})
export class UsersListPageComponent implements OnInit {

  public data: User[];

  public constructor(private service: UsersService) {
  }

  public ngOnInit(): void {
    this.refresh();
  }

  public edit(user: User): void {

  }

  public delete(user: User): void {
    this.service.delete(user).subscribe(() => this.refresh());
  }

  private refresh(): void {
    this.service.findAll().subscribe(result => this.data = result);
  }

}
