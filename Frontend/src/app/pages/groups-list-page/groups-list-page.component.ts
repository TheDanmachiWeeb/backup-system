import { Component, OnInit } from '@angular/core';
import { GroupsService } from '../../services/groups.service';
import { Group } from '../../models/group';
import { Router } from '@angular/router';

@Component({
  selector: 'app-groups-list-page',
  templateUrl: './groups-list-page.component.html',
  styleUrls: ['./groups-list-page.component.scss'],
})
export class GroupsListPageComponent implements OnInit {
  public data: Group[];
  public filteredData: Group[];

  public constructor(private service: GroupsService, private router: Router) {}

  public ngOnInit(): void {
    this.refresh();
  }

  public editGroup(group: Group): void {
    this.router.navigate(['/groups/edit', group.groupId]);
  }

  public deleteGroup(group: Group): void {
    this.service.delete(group).subscribe(() => this.refresh());
  }

  private refresh(): void {
    this.service.findAll().subscribe((result) => {
      this.data = result;
      this.filteredData = [...this.data];
    });
  }

  onFiltered(filteredOptions: Group[]) {
    this.filteredData = filteredOptions;
  }
}
