import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Report } from '../models/report';

@Injectable({
  providedIn: 'root',
})
export class ReportsService {
  constructor(private http: HttpClient) {}

  public findAll(): Observable<Report[]> {
    return this.http.get<Report[]>('http://localhost:5666/api/reports');
  }

  public insert(report: Report): Observable<Report> {
    return this.http.post<Report>('http://localhost:5666/api/reports', report);
  }

  public delete(report: Report): Observable<Report> {
    return this.http.delete<Report>(
      'http://localhost:5666/api/reports/' + report.reportId
    );
  }
}
