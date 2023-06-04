import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { DatePipe } from '@angular/common';
import { Report } from '../../models/report';

@Component({
  selector: 'app-reports-table',
  templateUrl: './reports-table.component.html',
  styleUrls: ['./reports-table.component.scss'],
})
export class ReportsTableComponent implements OnInit {
  @Input()
  public reports: Report[];

  @Output()
  deleted: EventEmitter<Report> = new EventEmitter<Report>();

  public constructor() {}

  public ngOnInit(): void {}

  public formatDate(date: string): string {
    return new DatePipe('en-US').transform(new Date(date), 'short') || '';
  }

  public formatBytes(bytes: number, decimals: number = 2): string {
    if (bytes === 0) {
      return '0 Bytes';
    }

    const k = 1024;
    const sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    const formattedSize = parseFloat(
      (bytes / Math.pow(k, i)).toFixed(decimals)
    );

    return `${formattedSize} ${sizes[i]}`;
  }
}
