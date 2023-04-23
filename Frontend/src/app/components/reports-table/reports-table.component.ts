import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {Report} from "../../models/report";

@Component({
  selector: 'app-reports-table',
  templateUrl: './reports-table.component.html',
  styleUrls: ['./reports-table.component.scss']
})
export class ReportsTableComponent implements OnInit {

  @Input()
  public reports: Report[];

  @Output()
  deleted: EventEmitter<Report> = new EventEmitter<Report>();

  public constructor() {

  }

  public ngOnInit(): void {

  }
}
