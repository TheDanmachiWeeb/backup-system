import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { GroupsService } from '../../services/groups.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Group } from '../../models/group';
import { GroupFormComponent } from '../../components/group-form/group-form.component';

@Component({
  selector: 'app-groups-edit-page',
  templateUrl: './groups-edit-page.component.html',
  styleUrls: ['./groups-edit-page.component.scss']
})

export class GroupsEditPageComponent implements OnInit {
  form: FormGroup;

  group: Group;

  public constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private service: GroupsService
  ) {}

  public ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');

    this.service.findById(Number(id)).subscribe((group) => {
      this.group = group;
      this.form = GroupFormComponent.createForm(this.fb, group);
    });
  }

  public saveGroup(values: any): void {
    Object.assign(this.group, values);
    this.service
      .update(this.group)
      .subscribe(() => this.router.navigate(['/groups']));
  }

  public deleteGroup(): void {
    this.service
      .delete(this.group)
      .subscribe(() => this.router.navigate(['/groups']));
  }
}
