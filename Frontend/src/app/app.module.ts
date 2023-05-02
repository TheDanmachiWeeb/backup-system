import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AutocompleteLibModule } from 'angular-ng-autocomplete';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { SidebarComponent } from './components/sidebar/sidebar.component';
import { UsersListPageComponent } from './pages/users-list-page/users-list-page.component';
import { DashboardPageComponent } from './pages/dashboard-page/dashboard-page.component';
import { StationsListPageComponent } from './pages/stations-list-page/stations-list-page.component';
import { GroupsListPageComponent } from './pages/groups-list-page/groups-list-page.component';
import { ConfigsListPageComponent } from './pages/configs-list-page/configs-list-page.component';
import { ReportsListPageComponent } from './pages/reports-list-page/reports-list-page.component';
import { SettingsPageComponent } from './pages/settings-page/settings-page.component';
import { UsersTableComponent } from './components/users-table/users-table.component';
import { ButtonComponent } from './components/button/button.component';
import { UsersEditPageComponent } from './pages/users-edit-page/users-edit-page.component';
import { UsersCreatePageComponent } from './pages/users-create-page/users-create-page.component';
import { ConfigsTableComponent } from './components/configs-table/configs-table.component';
import { GroupsTableComponent } from './components/groups-table/groups-table.component';
import { StationsTableComponent } from './components/stations-table/stations-table.component';
import { ReportsTableComponent } from './components/reports-table/reports-table.component';
import { UserFormComponent } from './components/user-form/user-form.component';
import { ReactiveFormsModule } from '@angular/forms';
import { ConfigsEditPageComponent } from './pages/configs-edit-page/configs-edit-page.component';
import { ConfigFormComponent } from './components/config-form/config-form.component';
import { InputAutocompleteComponent } from './components/input-autocomplete/input-autocomplete.component';
import { GroupsEditPageComponent } from './pages/groups-edit-page/groups-edit-page.component';
import { GroupFormComponent } from './components/group-form/group-form.component';
import { ReportFormComponent } from './components/report-form/report-form.component';
import { FtpFormComponent } from './components/ftp-form/ftp-form.component';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { InterceptorService } from './services/interceptor.service';

@NgModule({
  declarations: [
    AppComponent,
    SidebarComponent,
    UsersListPageComponent,
    DashboardPageComponent,
    StationsListPageComponent,
    GroupsListPageComponent,
    ConfigsListPageComponent,
    ReportsListPageComponent,
    SettingsPageComponent,
    UsersTableComponent,
    ButtonComponent,
    UsersEditPageComponent,
    UsersCreatePageComponent,
    ConfigsTableComponent,
    GroupsTableComponent,
    StationsTableComponent,
    ReportsTableComponent,
    UserFormComponent,
    ConfigsEditPageComponent,
    ConfigFormComponent,
    InputAutocompleteComponent,
    GroupsEditPageComponent,
    GroupFormComponent,
    ReportFormComponent,
    FtpFormComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    AutocompleteLibModule,
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: InterceptorService, multi: true },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
