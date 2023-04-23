import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {Config} from "../../models/config";

@Component({
  selector: 'app-configs-table',
  templateUrl: './configs-table.component.html',
  styleUrls: ['./configs-table.component.scss']
})
export class ConfigsTableComponent implements OnInit {

  @Input()
  public configs: Config[];

  @Output()
  deleted: EventEmitter<Config> = new EventEmitter<Config>();

  @Output()
  edited: EventEmitter<Config> = new EventEmitter<Config>();

  public constructor() {

  }

  public ngOnInit(): void {

  }
}
