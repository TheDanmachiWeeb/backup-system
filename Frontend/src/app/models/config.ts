
export class Config
{
  public configId: number;

  public configName: string;

  public backupType: string;

  public retention: number;

  public packageSize: number;

  public periodCron: string;

  public constructor(id: number, name: string, backupType: string, retention: number, packageSize: Int16Array, periodCron: string) {
    this.configId = id;
    this.configName = name;
    this.backupType = backupType;
    this.retention = retention;
    this.packageSize = this.packageSize;
    this.periodCron = this.periodCron;
  }

}
