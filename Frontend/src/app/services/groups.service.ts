import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Group } from '../models/group';

@Injectable({
  providedIn: 'root',
})
export class GroupsService {
  private apiUrl = 'http://localhost:5666/api/groups';
  constructor(private http: HttpClient) {}

  public findAll(): Observable<Group[]> {
    return this.http.get<Group[]>(this.apiUrl);
  }

  public findById(id: number): Observable<Group> {
    return this.http.get<Group>('http://localhost:5666/api/groups/' + id);
  }

  public insert(group: Group): Observable<Group> {
    return this.http.post<Group>(this.apiUrl, group);
  }

  public update(group: Group): Observable<Group> {
    return this.http.put<Group>(`${this.apiUrl}/${group.groupId}`, group);
  }

  public delete(group: Group): Observable<Group> {
    return this.http.delete<Group>(`${this.apiUrl}/${group.groupId}`);
  }
}
