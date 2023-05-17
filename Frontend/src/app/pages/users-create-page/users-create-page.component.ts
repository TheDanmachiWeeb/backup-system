import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { UsersService } from '../../services/users.service';
import { User } from '../../models/user';
import { ActivatedRoute, Router } from '@angular/router';
import { UserFormComponent } from 'src/app/components/user-form/user-form.component';

@Component({
  selector: 'app-users-create-page',
  templateUrl: './users-create-page.component.html',
  styleUrls: ['./users-create-page.component.scss'],
})
export class UsersCreatePageComponent implements OnInit {
  form: FormGroup;

  public constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private UsersService: UsersService
  ) {}

  public ngOnInit(): void {
    this.form = UserFormComponent.createForm(this.fb, new User());
  }

  public createUser(values: any): void {
    console.log(values);
    this.UsersService.insert(values).subscribe(() =>
      this.router.navigate(['/users'])
    );
  }
}
