import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ConfigsService } from '../../services/configs.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Config } from '../../models/config';
import { ConfigFormComponent } from '../../components/config-form/config-form.component';

@Component({
  selector: 'app-configs-edit-page',
  templateUrl: './configs-edit-page.component.html',
  styleUrls: ['./configs-edit-page.component.scss'],
})
export class ConfigsEditPageComponent implements OnInit {
  form: FormGroup;

  config: Config;

  public constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private service: ConfigsService
  ) {}

  public ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');

    this.service.findById(Number(id)).subscribe((config) => {
      this.config = config;
      this.form = ConfigFormComponent.createForm(this.fb, config);
    });
  }

  public saveConfig(values: any): void {
    Object.assign(this.config, values);
    this.service
      .update(this.config)
      .subscribe(() => this.router.navigate(['/configs']));
  }

  public deleteConfig(): void {
    this.service
      .delete(this.config)
      .subscribe(() => this.router.navigate(['/configs']));
  }
}
