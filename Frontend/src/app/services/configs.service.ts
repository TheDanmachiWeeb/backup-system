import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { Config } from '../models/config';
import { Destination } from '../models/destination';
import { Source } from '../models/source';
import { Group } from '../models/group';
import { Station } from '../models/station';

@Injectable({
  providedIn: 'root',
})
export class ConfigsService {
  private apiUrl = 'http://localhost:5666/api/configurations';

  constructor(private http: HttpClient) {}

  public findAll(): Observable<Config[]> {
    return this.http.get<Config[]>(this.apiUrl);
  }

  public findById(id: number): Observable<Config> {
    return this.http.get<Config>(`${this.apiUrl}/${id}`).pipe(
      map((config: Config) => {
        config.groups = Array.from(
          new Set(config.groups.map((group) => JSON.stringify(group)))
        ).map((group) => JSON.parse(group));
        return config;
      })
    );
  }
  public insert(config: Config): Observable<Config> {
    return this.http.post<Config>(this.apiUrl, config);
  }

  public update(config: Config): Observable<Config> {
    return this.http.put<Config>(`${this.apiUrl}/${config.configId}`, config);
  }

  public delete(config: Config): Observable<Config> {
    return this.http.delete<Config>(`${this.apiUrl}/${config.configId}`);
  }
}
