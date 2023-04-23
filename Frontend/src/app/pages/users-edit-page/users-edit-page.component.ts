import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { UsersService } from '../../services/users.service';
import { ActivatedRoute, Router } from '@angular/router';
import { User } from '../../models/user';
import { UserFormComponent } from '../../components/user-form/user-form.component';

@Component({
  selector: 'app-users-edit-page',
  templateUrl: './users-edit-page.component.html',
  styleUrls: ['./users-edit-page.component.scss'],
})
export class UsersEditPageComponent implements OnInit {
  form: FormGroup;

  user: User;

  public constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private service: UsersService
  ) {}

  public ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');

    this.service.findById(Number(id)).subscribe((user) => {
      this.user = user;
      this.form = UserFormComponent.createForm(this.fb, user);
    });
  }

  public saveUser(values: any): void {
    /*this.user.name = values.name;
    this.user.surname = values.surname;
    this.user.email = values.email;*/

    Object.assign(this.user, values);
    this.service
      .update(this.user)
      .subscribe(() => this.router.navigate(['/users']));
  }

  public deleteUser(): void {
    this.service
      .delete(this.user)
      .subscribe(() => this.router.navigate(['/users']));
  }
}
