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

@Component({
  selector: 'app-configs-create-page',
  templateUrl: './configs-create-page.component.html',
  styleUrls: ['./configs-create-page.component.scss'],
})
export class ConfigsCreatePageComponent implements OnInit {
  form: FormGroup;

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
    this.form = ConfigFormComponent.createForm(this.fb, new Config());

    this.StationsService.findAll().subscribe((stations) => {
      this.stations = stations;
    });

    this.GroupsService.findAll().subscribe((groups) => {
      this.groups = groups;
    });
  }

  public createConfig(values: any): void {
    this.ConfigsService.insert(values).subscribe(() =>
      this.router.navigate(['/configs'])
    );
  }
}
