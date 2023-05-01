import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormBuilder, FormGroup, FormControl } from '@angular/forms';
import { Config } from '../../models/config';
import { Station } from '../../models/station';
import { Group } from '../../models/group';
import { Source } from 'src/app/models/source';
import { Destination } from 'src/app/models/destination';

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

  @Input()
  stations: Station[];

  @Input()
  groups: Group[];

  @Output()
  saved: EventEmitter<any> = new EventEmitter<any>();

  @Output()
  deleted: EventEmitter<any> = new EventEmitter<any>();

  sourceInput: string = '';

  public static createForm(fb: FormBuilder, config: Config): FormGroup {
    return fb.group({
      configName: config.configName,
      backupType: config.backupType,
      retention: config.retention,
      packageSize: config.packageSize,
      periodCron: config.periodCron,
      sources: fb.array(
        config.sources.map((source) =>
          fb.group({
            path: [source.path],
          })
        )
      ),
      destinations: fb.array(
        config.destinations.map((destination) =>
          fb.group({
            path: [destination.path],
            type: [destination.type],
          })
        )
      ),
    });
  }

  public save(): void {
    this.saved.emit(this.form.value);
  }

  public delete(): void {
    this.deleted.emit(this.form.value);
  }

  public addStation(item: Station): void {
    this.config.stations.push(item);
    this.stations = this.stations.filter((s) => s.stationId != item.stationId);
  }

  public deleteStation(item: Station): void {
    this.config.stations = this.config.stations.filter(
      (s) => s.stationId != item.stationId
    );
    this.stations.push(item);
  }
  public addGroup(item: Group): void {
    this.config.groups.push(item);
    this.groups = this.groups.filter((s) => s.groupId != item.groupId);
  }

  public deleteGroup(item: Group): void {
    this.config.groups = this.config.groups.filter(
      (s) => s.groupId != item.groupId
    );
    this.groups.push(item);
  }

  public deleteSource(item: Source): void {
    this.config.sources.filter((s) => s.path != item.path);
  }

  public deleteDestination(item: Destination): void {
    this.config.destinations.filter((d) => d.path != item.path);
  }

  public addSource(): void {
    console.log(this.sourceInput);
    this.config.sources.push(new Source(this.sourceInput));
  }

  public addDestination(path: string, type: 'full' | 'diff' | 'inc'): void {
    this.config.sources.push(new Destination(path, type));
  }
}
