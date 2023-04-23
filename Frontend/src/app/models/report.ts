
export class Report
{
  public reportId: number;

  public stationId: number;

  public stationName: string;

  public configId: number;

  public configName: string;

  public reportTime: string;

  public backupSize: number;

  public success: boolean;

  public constructor(reportId: number, stationId: number, stationName: string, configId: number, configName: string, reportTime: string, backupSize: number, success: boolean) {
    this.reportId = reportId;
    this.stationId = stationId;
    this.configId = configId;
    this.reportTime = reportTime;
    this.backupSize = backupSize;
    this.success = success;
  }

}
