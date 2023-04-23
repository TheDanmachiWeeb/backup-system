import {Station} from "./station";
import { Config } from "./config";

export class Group
{
  public groupId: number;

  public groupName: string;

  public stations: Station[];

  public configs: Config[];

  public constructor(id: number, name: string) {
    this.groupId = id;
    this.groupName = name;
  }
}
