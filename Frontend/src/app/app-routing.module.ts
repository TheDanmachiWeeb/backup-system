import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UsersListPageComponent } from './pages/users-list-page/users-list-page.component';
import { DashboardPageComponent } from './pages/dashboard-page/dashboard-page.component';
import { StationsListPageComponent } from './pages/stations-list-page/stations-list-page.component';
import { ConfigsListPageComponent } from './pages/configs-list-page/configs-list-page.component';
import { ConfigsEditPageComponent } from './pages/configs-edit-page/configs-edit-page.component';
import { GroupsListPageComponent } from './pages/groups-list-page/groups-list-page.component';
import { ReportsListPageComponent } from './pages/reports-list-page/reports-list-page.component';
import { SettingsPageComponent } from './pages/settings-page/settings-page.component';
import { UsersCreatePageComponent } from './pages/users-create-page/users-create-page.component';
import { UsersEditPageComponent } from './pages/users-edit-page/users-edit-page.component';
import { GroupsEditPageComponent } from './pages/groups-edit-page/groups-edit-page.component';
import { LoginPageComponent } from './pages/login-page/login-page.component';
import { AuthGuard } from './services/auth.guard';
import { ConfigsCreatePageComponent } from './pages/configs-create-page/configs-create-page.component';
import { GroupsCreatePageComponent } from './pages/groups-create-page/groups-create-page.component';
import { CatPageComponent } from './pages/cat-page/cat-page.component';

//d
const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    component: DashboardPageComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'stations',
    component: StationsListPageComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'users',
    component: UsersListPageComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'users/create',
    component: UsersCreatePageComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'users/edit/:id',
    component: UsersEditPageComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'configs',
    component: ConfigsListPageComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'configs/create',
    component: ConfigsCreatePageComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'configs/edit/:id',
    component: ConfigsEditPageComponent,
    canActivate: [AuthGuard],
  },

  {
    path: 'groups',
    component: GroupsListPageComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'groups/edit/:id',
    component: GroupsEditPageComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'groups/create',
    component: GroupsCreatePageComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'reports',
    component: ReportsListPageComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'settings',
    component: SettingsPageComponent,
    canActivate: [AuthGuard],
  },
  { path: 'cat', component: CatPageComponent },

  { path: 'login', component: LoginPageComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
