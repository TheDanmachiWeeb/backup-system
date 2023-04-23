import { Component, OnInit } from '@angular/core';
import { ReportsService } from '../../services/reports.service';
import { Report } from '../../models/report';
import { Router } from '@angular/router';

@Component({
  selector: 'app-reports-list-page',
  templateUrl: './reports-list-page.component.html',
  styleUrls: ['./reports-list-page.component.scss'],
})
export class ReportsListPageComponent implements OnInit {
  public data: Report[];

  public constructor(private service: ReportsService, private router: Router) {}

  public ngOnInit(): void {
    this.refresh();
  }

  public deleteReport(report: Report): void {
    this.service.delete(report).subscribe(() => this.refresh());
  }

  private refresh(): void {
    this.service.findAll().subscribe((result) => (this.data = result));
  }
}
