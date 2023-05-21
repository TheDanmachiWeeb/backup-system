import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ConfigsService } from '../../services/configs.service';
import { StationsService } from '../../services/stations.service';
import { GroupsService } from '../../services/groups.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Config } from '../../models/config';
import { Station } from '../../models/station';
import { Group } from '../../models/group';
import { GroupFormComponent } from '../../components/group-form/group-form.component';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-groups-edit-page',
  templateUrl: './groups-edit-page.component.html',
  styleUrls: ['./groups-edit-page.component.scss'],
})
export class GroupsEditPageComponent implements OnInit {
  form: FormGroup;

  group: Group;
  stations: Station[];
  configs: Config[];

  public constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private ConfigsService: ConfigsService,
    private StationsService: StationsService,
    private GroupsService: GroupsService
  ) {}

  public ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');

    forkJoin([
      this.GroupsService.findById(Number(id)),
      this.StationsService.findAll(),
      this.ConfigsService.findAll(),
    ]).subscribe(([group, stations, configs]) => {
      this.group = group;
      this.form = GroupFormComponent.createForm(this.fb, group);

      this.stations = stations;
      this.group.stations.forEach((station) => {
        this.stations = this.stations.filter(
          (s) => s.stationId !== station.stationId
        );
      });
      this.stations.sort((a, b) => a.stationName.localeCompare(b.stationName));

      this.configs = configs;
      this.group.configs.forEach((config) => {
        this.configs = this.configs.filter(
          (c) => c.configId !== config.configId
        );
      });
      this.configs.sort((a, b) => a.configName.localeCompare(b.configName));
    });
  }

  public saveGroup(values: any): void {
    Object.assign(this.group, values);
    this.GroupsService.update(this.group).subscribe(() =>
      this.router.navigate(['/groups'])
    );
  }

  public deleteGroup(): void {
    this.GroupsService.delete(this.group).subscribe(() =>
      this.router.navigate(['/groups'])
    );
  }
}
