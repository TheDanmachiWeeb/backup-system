import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { User } from '../models/user';

@Injectable({
  providedIn: 'root',
})
export class SessionsService {
  constructor(private http: HttpClient) {}

  public login(user: any, savePassword: boolean): Observable<any> {
    return this.http.post<any>('http://localhost:5666/api/sessions', user).pipe(
      tap((result) => {
        savePassword
          ? localStorage.setItem('token', result.token)
          : sessionStorage.setItem('token', result.token);

        sessionStorage.setItem('user', user.username);
        sessionStorage.setItem('userId', user.userId);
      })
    );
  }

  public logout(): void {
    sessionStorage.getItem('token')
      ? sessionStorage.removeItem('token')
      : localStorage.removeItem('token');
  }

  public getToken(): string | null {
    return sessionStorage.getItem('token')
      ? sessionStorage.getItem('token')
      : localStorage.getItem('token');
  }

  public authenticated(): boolean {
    return !!this.getToken();
  }
}
