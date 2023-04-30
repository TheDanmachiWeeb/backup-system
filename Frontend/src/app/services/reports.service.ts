import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Report } from '../models/report';

@Injectable({
  providedIn: 'root',
})
export class ReportsService {
  constructor(private http: HttpClient) {}
  private apiUrl = 'http://localhost:5666/api/reports';
  public findAll(): Observable<Report[]> {
    return this.http.get<Report[]>(this.apiUrl);
  }

  public findById(id: number): Observable<Report> {
    return this.http.get<Report>(this.apiUrl + '/' + id);
  }

  public insert(report: Report): Observable<Report> {
    return this.http.post<Report>(this.apiUrl, report);
  }

  public update(report: Report): Observable<Report> {
    return this.http.put<Report>(`${this.apiUrl}/${report.reportId}`, report);
  }

  public delete(report: Report): Observable<Report> {
    return this.http.delete<Report>(
      this.apiUrl + report.reportId
    );
  }
}
