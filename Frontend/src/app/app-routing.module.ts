import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DashboardComponent } from './dashboard/dashboard.component';
import { StationsComponent } from './stations/stations.component';


const routes: Routes = [
  { path: '', redirectTo: '/dashboard', pathMatch: 'full' }, // Redirect to the dashboard component
  { path: 'dashboard', component: DashboardComponent },
  { path: 'stations', component: StationsComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
