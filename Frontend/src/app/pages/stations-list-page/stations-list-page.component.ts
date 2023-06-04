import { Component, OnInit } from '@angular/core';
import { StationsService } from '../../services/stations.service';
import { Station } from '../../models/station';
import { Router } from '@angular/router';

@Component({
  selector: 'app-stations-list-page',
  templateUrl: './stations-list-page.component.html',
  styleUrls: ['./stations-list-page.component.scss'],
})
export class StationsListPageComponent implements OnInit {
  public data: Station[];
  public filteredData: Station[];

  public constructor(
    private service: StationsService,
    private router: Router
  ) {}

  public ngOnInit(): void {
    this.refresh();
  }

  public editStation(station: Station): void {
    this.router.navigate(['/stations/edit', station.stationId]);
  }

  public deleteStation(station: Station): void {
    this.service.delete(station).subscribe(() => this.refresh());
  }

  public approveStation(station: Station): void {
    this.service.approve(station).subscribe(() => this.refresh());
  }

  public rejectStation(station: Station): void {
    this.service.reject(station).subscribe(() => this.refresh());
  }

  private refresh(): void {
    this.service.findAll().subscribe((result) => {
      this.data = result;
      this.filteredData = [...this.data];
      this.filteredData.sort((a, b) =>
        a.status === 'waiting' && b.status !== 'waiting' ? -1 : 1
      );
    });
  }

  onFiltered(filteredOptions: Station[]) {
    this.filteredData = filteredOptions;
    this.filteredData.sort((a, b) =>
      a.status === 'waiting' && b.status !== 'waiting' ? -1 : 1
    );
  }
}
