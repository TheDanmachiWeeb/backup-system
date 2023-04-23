import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {User} from "../models/user";

@Injectable({
  providedIn: 'root'
})
export class UsersService {

  constructor(private http: HttpClient) { }

  public findAll(): Observable<User[]> {
    return this.http.get<User[]>('http://localhost:5666/api/users');
  }

  public findById(id: number): Observable<User> {
    return this.http.get<User>('http://localhost:5666/api/users/' + id);
  }

  public insert(user: User): Observable<User> {
    return this.http.post<User>('http://localhost:5666/api/users', user);
  }

  public update(user: User): Observable<User> {
    return this.http.put<User>('http://localhost:5666/api/users/' + user.userId, user);
  }

  public delete(user: User): Observable<User> {
    return this.http.delete<User>('http://localhost:5666/api/users/' + user.userId);
  }
}
