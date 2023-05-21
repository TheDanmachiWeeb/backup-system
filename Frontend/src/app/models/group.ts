import { Station } from './station';
import { Config } from './config';

export class Group {
  public groupId: number;

  public groupName: string;

  public stations: Station[];

  public configs: Config[];

  public constructor(
    id: number = 0,
    name: string = '',
    stations: Station[] = [],
    configs: Config[] = []
  ) {
    this.groupId = id;
    this.groupName = name;
    this.stations = stations;
    this.configs = configs;
  }
}
