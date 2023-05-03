export class Destination {
  type: 'ftp' | 'local' | 'network';
  path: string;

  public constructor(path: string, type: 'ftp' | 'local' | 'network') {
    this.path = path;
    this.type = type;
  }
}
