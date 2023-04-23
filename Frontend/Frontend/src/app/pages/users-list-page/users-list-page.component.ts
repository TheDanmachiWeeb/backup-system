import {Component, OnInit} from '@angular/core';
import {UsersService} from "../../services/users.service";
import {User} from "../../models/user";
import {Router} from "@angular/router";

@Component({
  selector: 'app-users-list-page',
  templateUrl: './users-list-page.component.html',
  styleUrls: ['./users-list-page.component.scss']
})
export class UsersListPageComponent implements OnInit {

  public data: User[];

  public constructor(private service: UsersService,
                     private router: Router) {
  }

  public ngOnInit(): void {
    this.refresh();
  }

  public editUser(user: User): void {
    this.router.navigate(['/users/edit', user.userId])
  }

  public deleteUser(user: User): void {
    this.service.delete(user).subscribe(() => this.refresh());
  }

  private refresh(): void {
    this.service.findAll().subscribe(result => this.data = result);
  }
}
