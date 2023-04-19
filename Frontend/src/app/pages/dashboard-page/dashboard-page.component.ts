import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-dashboard-page',
  templateUrl: './dashboard-page.component.html',
  styleUrls: ['./dashboard-page.component.scss']
})

export class DashboardPageComponent implements OnInit {

  summaryStats = {
    totalStations: 25,
    totalGroups: 4,
    totalConfigs: 10,
    totalReports: 30
  };

  latestBackups = [
    {
      stationName: 'Station 1',
      backupTime: '2022-04-01 10:00:00',
      backupType: 'Full',
      backupSize: '100MB',
      status: 'Success'
    },
    {
      stationName: 'Station 2',
      backupTime: '2022-03-31 10:00:00',
      backupType: 'Incremental',
      backupSize: '50MB',
      status: 'Success'
    },
    {
      stationName: 'Station 3',
      backupTime: '2022-03-30 10:00:00',
      backupType: 'Full',
      backupSize: '200MB',
      status: 'Failed'
    }
  ];

  constructor() { }

  ngOnInit(): void {
    // You can add any additional code that you need to run when the component is initialized
  }

}