import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UsersListPageComponent } from './pages/users-list-page/users-list-page.component';
import { DashboardPageComponent } from './pages/dashboard-page/dashboard-page.component';
import { StationsListPageComponent } from './pages/stations-list-page/stations-list-page.component';
import { ConfigsListPageComponent } from './pages/configs-list-page/configs-list-page.component';
import { GroupsListPageComponent } from './pages/groups-list-page/groups-list-page.component';
import { ReportsListPageComponent } from './pages/reports-list-page/reports-list-page.component';
import { SettingsPageComponent } from './pages/settings-page/settings-page.component';
import { UsersCreatePageComponent } from './pages/users-create-page/users-create-page.component';
import { UsersEditPageComponent } from './pages/users-edit-page/users-edit-page.component';

//d
const routes: Routes = [
  { path: '', pathMatch: 'full', component: DashboardPageComponent }, // Redirect to the dashboard component
  { path: 'stations', component: StationsListPageComponent },
  { path: 'users', component: UsersListPageComponent },
  { path: 'users/create', component: UsersCreatePageComponent },
  { path: 'users/edit/:id', component: UsersEditPageComponent },
  { path: 'configs', component: ConfigsListPageComponent },
  { path: 'groups', component: GroupsListPageComponent },
  { path: 'reports', component: ReportsListPageComponent },
  { path: 'settings', component: SettingsPageComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
