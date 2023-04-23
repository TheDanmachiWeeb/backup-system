import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Group } from '../models/group';

@Injectable({
  providedIn: 'root',
})
export class GroupsService {
  constructor(private http: HttpClient) {}

  public findAll(): Observable<Group[]> {
    return this.http.get<Group[]>('http://localhost:5666/api/groups');
  }

  public insert(group: Group): Observable<Group> {
    return this.http.post<Group>('http://localhost:5666/api/groups', group);
  }

  public delete(group: Group): Observable<Group> {
    return this.http.delete<Group>(
      'http://localhost:5666/api/groups/' + group.groupId
    );
  }
}
