import { Source } from './source';
import { Destination } from './destination';
import { Group } from './group';
import { Station } from './station';

export class Config {
  public configId: number;

  public configName: string;

  public backupType: string;

  public retention: number;

  public packageSize: number;

  public zip: boolean;

  public periodic: boolean;

  public finished: boolean;

  public periodCron: string;

  public sources: Source[];

  public destinations: Destination[];

  public stations: Station[];

  public groups: Group[];

  public constructor(
    id: number = 0,
    name: string = '',
    backupType: string = 'full',
    retention: number = 10,
    packageSize: number = 10,
    periodCron: string = '',
    zip: boolean = false,
    periodic: boolean = false,
    finished: boolean = false,
    sources: Source[] = [],
    destinations: Destination[] = [],
    stations: Station[] = [],
    groups: Group[] = []
  ) {
    this.configId = id;
    this.configName = name;
    this.backupType = backupType;
    this.retention = retention;
    this.packageSize = packageSize;
    this.periodCron = periodCron;
    this.zip = zip;
    this.periodic = periodic;
    this.finished = finished;
    this.sources = sources;
    this.destinations = destinations;
    this.stations = stations;
    this.groups = groups;
  }
}
