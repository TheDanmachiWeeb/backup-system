import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {Config} from "../models/config";

@Injectable({
  providedIn: 'root'
})
export class ConfigsService {

  constructor(private http: HttpClient) { }

  public findAll(): Observable<Config[]> {
    return this.http.get<Config[]>('http://localhost:5666/api/configurations');
  }

  public insert(config: Config): Observable<Config> {
    return this.http.post<Config>('http://localhost:5666/api/configurations', config);
  }

  public delete(config: Config): Observable<Config> {
    return this.http.delete<Config>('http://localhost:5666/api/configurations/' + config.configId);
  }
}