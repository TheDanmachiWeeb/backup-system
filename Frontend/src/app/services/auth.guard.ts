import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivate,
  Router,
  RouterStateSnapshot,
  UrlTree,
} from '@angular/router';
import { Observable } from 'rxjs';
import { SessionsService } from './sessions.service';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate {
  public constructor(
    private session: SessionsService,
    private router: Router
  ) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ):
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree>
    | boolean
    | UrlTree {
    const auth = this.session.authenticated();

    if (!auth) {
      this.router.navigate(['/login']);
    }

    return auth;
  }
}
