import {Group} from "./group";
import { Config } from "./config";

export class Station
{
  public stationId: number;

  public stationName: string;

  public ipAddress: string;

  public macAddress: string;

  public active: boolean;

  public groups: Group[];

  public configs: Config[];

  public constructor(id: number, name: string, ip: string, mac: string, active: boolean) {
    this.stationId = id;
    this.stationName = name;
    this.ipAddress = ip;
    this.macAddress = mac;
    this.active = active;
  }
}
