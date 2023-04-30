import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormBuilder, FormGroup, FormControl } from '@angular/forms';
import { Group } from '../../models/group';

@Component({
  selector: 'app-group-form',
  templateUrl: './group-form.component.html',
  styleUrls: ['./group-form.component.scss']
})
export class GroupFormComponent {
  @Input()
  form: FormGroup;

  @Input()
  group: Group;

  @Output()
  saved: EventEmitter<any> = new EventEmitter<any>();

  @Output()
  deleted: EventEmitter<any> = new EventEmitter<any>();

  public static createForm(fb: FormBuilder, group: Group): FormGroup {
    return fb.group({
      groupName: group.groupName,
      stations: group.stations,
      configs: group.configs,
      id: group.groupId
    });
  }

  public save(): void {
    this.saved.emit(this.form.value);
  }

  public delete(): void {
    this.deleted.emit(this.form.value);
  }

  public deleteStation(id: number): void {
    this.group.stations = this.group.stations.filter(
      (s) => s.stationId != id
    );
  }

  public deleteConfig(id: number): void {
    this.group.configs = this.group.configs.filter((s) => s.configId != id);
  }
}

