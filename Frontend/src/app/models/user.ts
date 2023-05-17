export class User {
  public userId: number;

  public email: string;

  public username: string;

  public passwordHash: string;

  public constructor(
    id: number = 0,
    username: string = '',
    email: string = '',
    passwordHash: string = ''
  ) {
    this.userId = id;
    this.username = username;
    this.email = email;
    this.passwordHash = passwordHash;
  }
}
