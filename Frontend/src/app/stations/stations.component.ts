import { Component } from '@angular/core';

@Component({
  selector: 'app-stations',
  templateUrl: './stations.component.html',
  styleUrls: ['./stations.component.scss']
})
export class StationsComponent {
  stations = [
    {
      name: 'Station 1',
      ipAddress: '192.168.0.1',
      macAddress: '00:11:22:33:44:55',
      active: true,
      lastBackup: '2022-03-28 13:34:09',
      status: 'OK'
    },
    {
      name: 'Station 2',
      ipAddress: '192.168.0.2',
      macAddress: '00:11:22:33:44:56',
      active: true,
      lastBackup: '2022-03-27 16:45:22',
      status: 'OK'
    },
    {
      name: 'Station 3',
      ipAddress: '192.168.0.3',
      macAddress: '00:11:22:33:44:57',
      active: false,
      lastBackup: '2022-03-25 09:21:11',
      status: 'Error'
    },
    {
      name: 'Station 4',
      ipAddress: '192.168.0.4',
      macAddress: '00:11:22:33:44:58',
      active: true,
      lastBackup: '2022-03-22 18:11:54',
      status: 'OK'
    },
    {
      name: 'Station 5',
      ipAddress: '192.168.0.5',
      macAddress: '00:11:22:33:44:59',
      active: true,
      lastBackup: '2022-03-20 12:53:07',
      status: 'Warning'
    }
  ];
}
