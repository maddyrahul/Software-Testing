import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { CreateGroupComponent } from './groups/create-group/create-group.component';
import { AddExpenseComponent } from './expenses/add-expense/add-expense.component';
import { UserManagementComponent } from './user-management/user-management.component';
import { EditUserComponent } from './edit-user/edit-user.component';
import { ExpenseListComponent } from './expenses/expense-list/expense-list.component';
import { UpdateExpenseComponent } from './expenses/update-expense/update-expense.component';
import { AuthGuard } from './guards/auth.guard';
import { GroupBalanceComponent } from './group-balance/group-balance.component';
import { GroupDetailsComponent } from './group-details/group-details.component';
import { GroupListComponent } from './group-list/group-list.component';


const routes: Routes = [
 
  {path:'group-balance',component:GroupBalanceComponent},
  { path: 'login', component: LoginComponent },
  {path:'group-details',component:GroupDetailsComponent},
  { path: 'create-group', component: CreateGroupComponent, canActivate: [AuthGuard] , data: { role: 'admin' }},
  { path: 'group/:groupId/add-expense', component: AddExpenseComponent, canActivate: [AuthGuard],data: { role: 'normal' } },
  { path: 'user-management', component: UserManagementComponent, canActivate: [AuthGuard], data: { role: 'admin' } },
  { path: 'edit-user/:id', component: EditUserComponent, canActivate: [AuthGuard] , data: { role: 'admin' }},
  { path: 'add-expense', component: AddExpenseComponent, canActivate: [AuthGuard] , data: { role: 'normal' }},
  { path: 'expense-list', component: ExpenseListComponent, canActivate: [AuthGuard], data: { role: 'admin' } },
  {path :'group-list',component:GroupListComponent, canActivate: [AuthGuard], data: { role: 'admin' } },
  { path: 'update-expense/:expenseId', component: UpdateExpenseComponent, canActivate: [AuthGuard], data: { role: 'admin' } }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
