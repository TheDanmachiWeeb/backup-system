export class Destination {
  type: 'full' | 'inc' | 'diff';
  path: string;

  public constructor(path: string, type: 'full' | 'inc' | 'diff') {
    this.path = path;
    this.type = type;
  }
}
