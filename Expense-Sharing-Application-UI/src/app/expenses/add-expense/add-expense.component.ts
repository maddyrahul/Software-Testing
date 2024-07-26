// src/app/expenses/add-expense/add-expense.component.ts

import { Component} from '@angular/core';
import { Router } from '@angular/router';
import { ExpenseDto } from 'src/app/models/ExpenseDto ';
import { ExpenseService } from 'src/app/services/expense.service';

@Component({
  selector: 'app-add-expense',
  templateUrl: './add-expense.component.html',
  styleUrls: ['./add-expense.component.css']
})
export class AddExpenseComponent {
  expenseDto: ExpenseDto = {
    email: '',
    groupName: '',
    description: '',
    amount: 0,
    date: new Date()
  };

  constructor(private expenseService: ExpenseService, private router: Router) { }

  onSubmit(): void {
    this.expenseService.addExpense(this.expenseDto).subscribe(
      response => {
        alert('Expense added successfully');
        this.router.navigate(['/group-balance']);
      },
      error => {
        alert('Failed to add expense');
        console.error(error);
      }
    );
  }
}
