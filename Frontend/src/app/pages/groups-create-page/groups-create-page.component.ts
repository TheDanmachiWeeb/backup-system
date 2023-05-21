import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ConfigsService } from '../../services/configs.service';
import { StationsService } from '../../services/stations.service';
import { GroupsService } from '../../services/groups.service';
import { GroupFormComponent } from 'src/app/components/group-form/group-form.component';

import { ActivatedRoute, Router } from '@angular/router';
import { Config } from '../../models/config';
import { Station } from '../../models/station';
import { Group } from '../../models/group';

@Component({
  selector: 'app-groups-create-page',
  templateUrl: './groups-create-page.component.html',
  styleUrls: ['./groups-create-page.component.scss'],
})
export class GroupsCreatePageComponent implements OnInit {
  form: FormGroup;

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
    this.form = GroupFormComponent.createForm(this.fb, new Group());

    this.StationsService.findAll().subscribe((stations) => {
      this.stations = stations;
    });

    this.ConfigsService.findAll().subscribe((configs) => {
      this.configs = configs;
    });
  }

  public createGroup(values: any): void {
    this.GroupsService.insert(values).subscribe(() =>
      this.router.navigate(['/groups'])
    );
  }
}
