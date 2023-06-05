import { Component } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { UsersService } from '../../services/users.service';

@Component({
  selector: 'app-settings-page',
  templateUrl: './settings-page.component.html',
  styleUrls: ['./settings-page.component.scss'],
})
export class SettingsPageComponent {
  form: FormGroup;

  constructor(private service: UsersService) {
    this.form = new FormGroup({
      emailCron: new FormControl(''),
    });
  }
}
