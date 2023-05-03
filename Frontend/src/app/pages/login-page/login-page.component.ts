import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup} from "@angular/forms";
import {SessionsService} from "../../services/sessions.service";
import {Router} from "@angular/router";
import {catchError, of} from "rxjs";

@Component({
  selector: 'app-login-page',
  templateUrl: './login-page.component.html',
  styleUrls: ['./login-page.component.scss'],
})
<<<<<<< HEAD
export class LoginPageComponent {}
=======
export class LoginPageComponent implements OnInit {

  public form: FormGroup;
  public error: boolean = false;

  public constructor(private fb: FormBuilder,
                     private router: Router,
                     private sessions: SessionsService) {
  }

  public ngOnInit(): void {
    this.form = this.fb.group({
      username: '',
      password: ''
    });
  }

  public login(): void {
    this.sessions.login(this.form.value).pipe(
      catchError(() => {
        this.error = true;
        return of(false);
      })
    ).subscribe(result => {
      if (result) {
        this.router.navigate([''])
      }
    });
  }
}
>>>>>>> b59baf86c90090a4ed1efef5199749788f291739
