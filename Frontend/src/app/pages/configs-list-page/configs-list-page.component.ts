import { Component, OnInit } from '@angular/core';
import { ConfigsService } from '../../services/configs.service';
import { Config } from '../../models/config';
import { Router } from '@angular/router';

@Component({
  selector: 'app-configs-list-page',
  templateUrl: './configs-list-page.component.html',
  styleUrls: ['./configs-list-page.component.scss'],
})
export class ConfigsListPageComponent implements OnInit {
  public data: Config[];
  public filteredData: Config[];

  public constructor(private service: ConfigsService, private router: Router) {}

  public ngOnInit(): void {
    this.refresh();
  }

  public editConfig(config: Config): void {
    this.router.navigate(['/configs/edit', config.configId]);
  }

  public deleteConfig(config: Config): void {
    this.service.delete(config).subscribe(() => this.refresh());
  }

  private refresh(): void {
    this.service.findAll().subscribe((result) => {
      this.data = result;
      this.filteredData = [...this.data];
    });
  }

  onFiltered(filteredOptions: Config[]) {
    this.filteredData = filteredOptions;
  }
}
