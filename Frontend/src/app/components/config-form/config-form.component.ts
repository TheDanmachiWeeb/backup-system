import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormBuilder, FormGroup, FormControl } from '@angular/forms';
import { Config } from '../../models/config';

@Component({
  selector: 'app-config-form',
  templateUrl: './config-form.component.html',
  styleUrls: ['./config-form.component.scss'],
})
export class ConfigFormComponent {
  @Input()
  form: FormGroup;

  @Input()
  config: Config;

  @Output()
  saved: EventEmitter<any> = new EventEmitter<any>();

  @Output()
  deleted: EventEmitter<any> = new EventEmitter<any>();

  public static createForm(fb: FormBuilder, config: Config): FormGroup {
    return fb.group({
      configName: config.configName,
      backupType: config.backupType,
      retention: config.retention,
      packageSize: config.packageSize,
      periodCron: config.periodCron,
      sources: config.sources,
      destinations: config.destinations,
    });
  }

  public save(): void {
    this.saved.emit(this.form.value);
  }

  public delete(): void {
    this.deleted.emit(this.form.value);
  }

  public deleteStation(id: number): void {
    this.config.stations = this.config.stations.filter(
      (s) => s.stationId != id
    );
  }

  public deleteGroup(id: number): void {
    this.config.groups = this.config.groups.filter((s) => s.groupId != id);
  }
}
