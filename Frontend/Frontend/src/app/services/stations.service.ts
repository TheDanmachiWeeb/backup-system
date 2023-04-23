import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {Station} from "../models/station";

@Injectable({
  providedIn: 'root'
})
export class StationsService {

  constructor(private http: HttpClient) { }

  public findAll(): Observable<Station[]> {
    return this.http.get<Station[]>('http://localhost:5666/api/stations');
  }

  public insert(station: Station): Observable<Station> {
    return this.http.post<Station>('http://localhost:5666/api/stations', station);
  }

  public delete(station: Station): Observable<Station> {
    return this.http.delete<Station>('http://localhost:5666/api/stations/' + station.stationId);
  }
}
