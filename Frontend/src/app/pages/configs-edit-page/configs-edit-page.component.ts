import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ConfigsService } from '../../services/configs.service';
import { StationsService } from '../../services/stations.service';
import { GroupsService } from '../../services/groups.service';

import { ActivatedRoute, Router } from '@angular/router';
import { Config } from '../../models/config';
import { Station } from '../../models/station';
import { Group } from '../../models/group';
import { ConfigFormComponent } from '../../components/config-form/config-form.component';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-configs-edit-page',
  templateUrl: './configs-edit-page.component.html',
  styleUrls: ['./configs-edit-page.component.scss'],
})
export class ConfigsEditPageComponent implements OnInit {
  form: FormGroup;

  config: Config;
  stations: Station[];
  groups: Group[];

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

    this.ConfigsService.findById(Number(id)).subscribe((config) => {
      this.config = config;
      this.form = ConfigFormComponent.createForm(this.fb, config);

      forkJoin([
        this.StationsService.findAll(),
        this.GroupsService.findAll(),
      ]).subscribe(([stations, groups]) => {
        this.stations = stations;
        this.config.stations.forEach((station) => {
          this.stations = this.stations.filter(
            (s) => s.stationId !== station.stationId
          );
        });
        this.stations.sort((a, b) =>
          a.stationName.localeCompare(b.stationName)
        );

        this.groups = groups;
        this.config.groups.forEach((group) => {
          this.groups = this.groups.filter((g) => g.groupId !== group.groupId);
        });
        this.groups.sort((a, b) => a.groupName.localeCompare(b.groupName));
      });
    });
  }

  public saveConfig(values: any): void {
    Object.assign(this.config, values);
    this.ConfigsService.update(this.config).subscribe(() =>
      this.router.navigate(['/configs'])
    );
  }

  public deleteConfig(): void {
    this.ConfigsService.delete(this.config).subscribe(() =>
      this.router.navigate(['/configs'])
    );
  }
}
