import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { CommonModule } from '@angular/common';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';


import { SidebarComponent } from './components/sidebar/sidebar.component';
import { UsersListPageComponent } from './pages/users-list-page/users-list-page.component';
import {HttpClientModule} from "@angular/common/http";
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
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    CommonModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
