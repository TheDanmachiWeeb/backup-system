import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DashboardComponent } from './dashboard/dashboard.component';
import { StationsComponent } from './stations/stations.component';
import { AddStationComponent } from './stations/add-station/add-station.component';
import { GroupComponent } from './group/group.component'; 
import { AddUserComponent } from './users/add-user/add-user.component';
import { UsersComponent } from './users/users.component';
import { ConfigsComponent } from './configs/configs.component';



const routes: Routes = [
  { path: '', redirectTo: '/dashboard', pathMatch: 'full' }, // Redirect to the dashboard component
  { path: 'dashboard', component: DashboardComponent },
  { path: 'stations', component: StationsComponent },
  { path: 'addStation', component: AddStationComponent},
  { path: 'group', component: GroupComponent},
  { path: 'addUser', component: AddUserComponent},
  { path: 'Users', component: UsersComponent},
  { path: 'configs', component: ConfigsComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
