import { Component, OnInit, Input } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Destination } from 'src/app/models/destination';

@Component({
  selector: 'app-ftp-form',
  templateUrl: './ftp-form.component.html',
  styleUrls: ['./ftp-form.component.scss'],
})
export class FtpFormComponent {
  form: FormGroup;

  @Input()
  destination: Destination;

  constructor(private fb: FormBuilder) {}

  ngOnInit() {
    this.form = this.fb.group({
      hostname: ['', Validators.required],
      username: ['', Validators.required],
      password: ['', Validators.required],
      port: [21, Validators.required],
      encryption: ['none'],
      mode: ['active'],
    });
  }
}
