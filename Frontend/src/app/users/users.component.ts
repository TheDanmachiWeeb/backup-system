import { Component } from '@angular/core';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.scss']
})
export class UsersComponent {
  users = [
    {
      username: 'User 1',
      email: 'user1@mail.cz',
      password: '***********',
      description: true,
    },
    {
      username: 'User 2',
      email: 'user2@mail.cz',
      password: '***********',
      description: true,
    },
    {
      username: 'User 3',
      email: 'user3@mail.cz',
      password: '***********',
      description: true,
    },
    {
      username: 'User 4',
      email: 'user4@mail.cz',
      password: '***********',
      description: true,
    },
    
  ];
}
