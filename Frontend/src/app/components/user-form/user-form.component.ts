import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { User } from '../../models/user';

@Component({
  selector: 'app-user-form',
  templateUrl: './user-form.component.html',
  styleUrls: ['./user-form.component.scss'],
})
export class UserFormComponent {
  @Input()
  form: FormGroup;

  @Output()
  saved: EventEmitter<any> = new EventEmitter<any>();

  @Output()
  deleted: EventEmitter<any> = new EventEmitter<any>();

  public static createForm(fb: FormBuilder, user: User): FormGroup {
    return fb.group({
      username: user.username,
      email: user.email,
    });
  }

  public save(): void {
    this.saved.emit(this.form.value);
  }

  public delete(): void {
    this.deleted.emit(this.form.value);
  }
}
