import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Expense } from 'src/app/models/Expense';
import { ExpenseService } from 'src/app/services/expense.service';

@Component({
  selector: 'app-update-expense',
  templateUrl: './update-expense.component.html',
  styleUrls: ['./update-expense.component.css']
})
export class UpdateExpenseComponent implements OnInit {
  expense: Expense | null = null;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private adminService: ExpenseService
  ) { }

  ngOnInit(): void {
    const expenseId = +this.route.snapshot.paramMap.get('expenseId')!;
    this.loadExpense(expenseId);
  }

  loadExpense(expenseId: number): void {
    this.adminService.getExpenseById(expenseId).subscribe(
      data => this.expense = data,
      error => console.error(error)
    );
  }

  updateExpense(): void {
    if (this.expense) {
      this.adminService.updateExpense(this.expense.expenseId, this.expense).subscribe(
        data => this.router.navigate(['/expense-list']),
        error => console.error(error)
      );
    }
  }

  cancelUpdate(): void {
    this.router.navigate(['/expense-list']);
  }
}