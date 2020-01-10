import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { PageNotFoundComponent } from './page-not-found.component';
import { AdminComponent } from './admin/admin.component';


const routes: Routes = [
  { path: '', loadChildren: 'src/app/consumer/consumer.module#ConsumerModule' },
  { path: 'admin', component: AdminComponent, loadChildren: 'src/app/admin/admin.module#AdminModule' },
  { path: '**', component: PageNotFoundComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
