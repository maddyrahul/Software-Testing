import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { FooterComponent } from './footer/footer.component';
import { HeaderComponent } from './header/header.component';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { LoginComponent } from './login/login.component';
import { JwtModule } from '@auth0/angular-jwt';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AddExpenseComponent } from './expenses/add-expense/add-expense.component';
import { ExpenseListComponent } from './expenses/expense-list/expense-list.component';

import { CreateGroupComponent } from './groups/create-group/create-group.component';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatTableModule } from '@angular/material/table';
import { EditUserComponent } from './edit-user/edit-user.component';
import { UserManagementComponent } from './user-management/user-management.component';
import { UpdateExpenseComponent } from './expenses/update-expense/update-expense.component';
import { GroupBalanceComponent } from './group-balance/group-balance.component';
import { GroupDetailsComponent } from './group-details/group-details.component';
import { GroupListComponent } from './group-list/group-list.component';
@NgModule({
  declarations: [
    AppComponent,
    FooterComponent,
    HeaderComponent,
    LoginComponent,
    AddExpenseComponent,
    ExpenseListComponent,
    CreateGroupComponent,
    EditUserComponent,
    UserManagementComponent,
    UpdateExpenseComponent,
    GroupBalanceComponent,
    GroupDetailsComponent,
    GroupListComponent,
  ],
  imports: [
    MatDatepickerModule,
    MatNativeDateModule,
    MatSnackBarModule,
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    AppRoutingModule,
    ReactiveFormsModule,
    MatCardModule,
    MatTableModule,
    HttpClientModule,
    RouterModule,
    JwtModule.forRoot({
      config: {
        tokenGetter: () => {
          return localStorage.getItem('access_token');
        },
      },
    }),
    BrowserAnimationsModule,
    MatIconModule,
    MatInputModule,
    MatFormFieldModule,
    MatButtonModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatTableModule,
  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
