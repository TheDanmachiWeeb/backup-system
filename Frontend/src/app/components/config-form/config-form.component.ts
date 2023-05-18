import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormBuilder, FormGroup, FormArray, Validators } from '@angular/forms';
import { Config } from '../../models/config';
import { Station } from '../../models/station';
import { Group } from '../../models/group';

@Component({
  selector: 'app-config-form',
  templateUrl: './config-form.component.html',
  styleUrls: ['./config-form.component.scss'],
})
export class ConfigFormComponent {
  @Input()
  form: FormGroup;

  @Input()
  stations: Station[];

  @Input()
  groups: Group[];

  @Output()
  saved: EventEmitter<any> = new EventEmitter<any>();

  @Output()
  deleted: EventEmitter<any> = new EventEmitter<any>();

  constructor(private fb: FormBuilder) {}

  getSourceControls() {
    return (this.form.get('sources') as FormArray).controls;
  }

  getDestinationControls() {
    return (this.form.get('destinations') as FormArray).controls;
  }

  public static createForm(fb: FormBuilder, config: Config): FormGroup {
    return fb.group({
      configId: config.configId,
      configName: [config.configName, Validators.required],
      backupType: [config.backupType, Validators.required],
      retention: [config.retention, Validators.required],
      packageSize: [config.packageSize, Validators.required],
      periodCron: [config.periodCron, Validators.required],
      stations: fb.array(
        config.stations.map((station) =>
          fb.group({
            stationId: station.stationId,
            stationName: station.stationName,
          })
        )
      ),
      groups: fb.array(
        config.groups.map((group) =>
          fb.group({
            groupId: group.groupId,
            groupName: group.groupName,
          })
        )
      ),
      sources: fb.array(
        config.sources.map((source) =>
          fb.group({
            path: source.path,
          })
        )
      ),
      sourceInput: '',
      destinations: fb.array(
        config.destinations.map((destination) =>
          fb.group({
            path: destination.path,
            type: destination.type,
          })
        )
      ),
      destinationInput: fb.group({
        path: '',
        type: 'local',
      }),
    });
  }

  public save(): void {
    const req = { ...this.form.value };
    req.stations = req.stations.map((s: { stationId: number }) => s.stationId);
    req.groups = req.groups.map((g: { groupId: number }) => g.groupId);
    delete req.destinationInput;
    delete req.sourceInput;

    this.saved.emit(req);
  }

  public delete(): void {
    this.deleted.emit(Number(this.form.value.configId));
  }

  public destinations(): FormArray {
    return this.form.get('destinations') as FormArray;
  }
  public sources(): FormArray {
    return this.form.get('sources') as FormArray;
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

  public addGroup(item: Group): void {
    const groups = this.form.get('groups') as FormArray;
    const group = this.fb.group({
      groupId: [item.groupId],
      groupName: [item.groupName],
    });
    groups.push(group);
    const index = this.groups.findIndex((g) => g.groupId == item.groupId);
    if (index !== -1) {
      this.groups.splice(index, 1);
    }
  }

  public deleteGroup(item: Group): void {
    const groups = this.form.get('groups') as FormArray;
    const index = groups.controls.findIndex(
      (group) => (group as FormGroup).value.groupName == item.groupName
    );
    if (index !== -1) {
      groups.removeAt(index);
    }

    this.groups.push(item);
    this.groups.sort((a, b) => a.groupName.localeCompare(b.groupName));
  }

  public deleteSource(index: number): void {
    this.sources().removeAt(index);
  }

  public deleteDestination(index: number): void {
    this.destinations().removeAt(index);
  }

  addSource() {
    const sources = this.form.get('sources') as FormArray;
    sources.push(
      this.fb.group({
        path: this.form.get('sourceInput')?.value,
      })
    );
    this.form.get('sourceInput')?.reset();
  }

  public addDestination(): void {
    const destinations = this.form.get('destinations') as FormArray;
    const destinationInput = this.form.get('destinationInput') as FormGroup;

    const destination = this.fb.group({
      path: destinationInput.get('path')?.value,
      type: destinationInput.get('type')?.value,
    });

    destinations.push(destination);
  }
}
