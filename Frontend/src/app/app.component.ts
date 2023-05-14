import { Component } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivate,
  Router,
  RouterStateSnapshot,
  UrlTree,
} from '@angular/router';
import { Observable } from 'rxjs';
import { SessionsService } from './services/sessions.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent {
  title = 'Frontend';

  public constructor(
    private session: SessionsService,
    private router: Router
  ) {}

  public logout(): void {
    this.session.logout();
    this.router.navigate(['/login']);
  }

  public authenticated(): boolean {
    return this.session.authenticated();
  }
}
