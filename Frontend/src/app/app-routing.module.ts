import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {UsersListPageComponent} from "./pages/users-list-page/users-list-page.component";
import { DashboardPageComponent } from './pages/dashboard-page/dashboard-page.component';
import { StationsListPageComponent } from './pages/stations-list-page/stations-list-page.component';



const routes: Routes = [
  { path: '', pathMatch: 'full', component: DashboardPageComponent }, // Redirect to the dashboard component
  { path: 'stations', component: StationsListPageComponent},

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
