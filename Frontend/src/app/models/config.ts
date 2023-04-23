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

  public periodCron: string;

  public sources: Source[];

  public destinations: Destination[];

  public stations: Station[];

  public groups: Group[];

  public constructor(
    id: number,
    name: string,
    backupType: string,
    retention: number,
    packageSize: number,
    periodCron: string,
    sources: Source[],
    destinations: Destination[],
    stations: Station[],
    groups: Group[]
  ) {
    this.configId = id;
    this.configName = name;
    this.backupType = backupType;
    this.retention = retention;
    this.packageSize = packageSize;
    this.periodCron = periodCron;
    this.sources = sources;
    this.destinations = destinations;
    this.stations = stations;
    this.groups = groups;
  }
}
