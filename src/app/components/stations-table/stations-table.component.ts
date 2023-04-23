import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Station } from '../../models/station';

@Component({
  selector: 'app-stations-table',
  templateUrl: './stations-table.component.html',
  styleUrls: ['./stations-table.component.scss'],
})
export class StationsTableComponent implements OnInit {
  @Input()
  public stations: Station[];

  @Output()
  deleted: EventEmitter<Station> = new EventEmitter<Station>();

  @Output()
  edited: EventEmitter<Station> = new EventEmitter<Station>();

  public constructor() {}

  public ngOnInit(): void {}
}
