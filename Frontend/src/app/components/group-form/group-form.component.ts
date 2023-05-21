import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormBuilder, FormGroup, FormControl, FormArray } from '@angular/forms';
import { Group } from '../../models/group';
import { Station } from 'src/app/models/station';
import { Config } from 'src/app/models/config';

@Component({
  selector: 'app-group-form',
  templateUrl: './group-form.component.html',
  styleUrls: ['./group-form.component.scss'],
})
export class GroupFormComponent {
  @Input()
  form: FormGroup;

  @Input()
  group: Group;

  @Input()
  stations: Station[];

  @Input()
  configs: Config[];

  @Output()
  saved: EventEmitter<any> = new EventEmitter<any>();

  @Output()
  deleted: EventEmitter<any> = new EventEmitter<any>();

  constructor(private fb: FormBuilder) {}

  public static createForm(fb: FormBuilder, group: Group): FormGroup {
    return fb.group({
      groupId: group.groupId,
      groupName: group.groupName,
      stations: fb.array(
        group.stations.map((station) =>
          fb.group({
            stationId: station.stationId,
            stationName: station.stationName,
          })
        )
      ),
      configs: fb.array(
        group.configs.map((config) =>
          fb.group({
            configId: config.configId,
            configName: config.configName,
          })
        )
      ),
    });
  }

  public save(): void {
    const req = { ...this.form.value };
    req.stations = req.stations.map((s: { stationId: number }) => s.stationId);
    req.configs = req.configs.map((c: { configId: number }) => c.configId);

    this.saved.emit(req);
  }

  public delete(): void {
    this.deleted.emit(Number(this.form.value.configId));
  }

  public addStation(item: Station): void {
    const stations = this.form.get('stations') as FormArray;
    const station = this.fb.group({
      stationId: [item.stationId],
      stationName: [item.stationName],
    });
    stations.push(station);
    const index = this.stations.findIndex((s) => s.stationId == item.stationId);
    if (index !== -1) {
      this.stations.splice(index, 1);
    }
  }

  public deleteStation(item: Station): void {
    const stations = this.form.get('stations') as FormArray;
    const index = stations.controls.findIndex(
      (control) => (control as FormGroup).value.stationId == item.stationId
    );
    if (index !== -1) {
      stations.removeAt(index);
    }
    this.stations.push(item);
    this.stations.sort((a, b) => a.stationName.localeCompare(b.stationName));
  }

  public addConfig(item: Config): void {
    const configs = this.form.get('configs') as FormArray;
    const config = this.fb.group({
      configId: [item.configId],
      configName: [item.configName],
    });
    configs.push(config);
    const index = this.configs.findIndex((s) => s.configId == item.configId);
    if (index !== -1) {
      this.configs.splice(index, 1);
    }
  }

  public deleteConfig(item: Config): void {
    const configs = this.form.get('configs') as FormArray;

    const index = configs.controls.findIndex(
      (control) => (control as FormGroup).value.configId == item.configId
    );
    if (index !== -1) {
      configs.removeAt(index);
    }
    this.configs.push(item);
    this.configs.sort((a, b) => a.configName.localeCompare(b.configName));
  }
}
