import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {UsersListPageComponent} from "./pages/users-list-page/users-list-page.component";
import { DashboardPageComponent } from './pages/dashboard-page/dashboard-page.component';



const routes: Routes = [
  { path: '', pathMatch: 'full', component: DashboardPageComponent }, // Redirect to the dashboard component

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
