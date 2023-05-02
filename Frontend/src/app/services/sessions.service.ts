import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable, tap} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class SessionsService {

  constructor(private http: HttpClient) { }

  public login(model: any): Observable<any> {
    return this.http.post<any>('http://localhost:5666//api/sessions', model).pipe(
      tap(result => sessionStorage.setItem('token', result.token))
    );
  }

  public logout(): void {
    sessionStorage.removeItem('token');
  }

  public getToken(): string|null  {
    return sessionStorage.getItem('token');
  }

  public authenticated(): boolean {
    return !!this.getToken();
  }

}