import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Expense } from '../models/Expense';
import { GroupMember } from '../models/GroupMember';
import { Observable } from 'rxjs/internal/Observable';
import { ExpenseDto } from '../models/ExpenseDto ';
import { GroupWithMembers } from '../models/GroupWithMembers';

@Injectable({
  providedIn: 'root'
})
export class ExpenseService {

  private expensesUrl = "https://localhost:7141/api/Groups";
  private url="https://localhost:7141/api/GroupMembers/group";

  constructor(private http: HttpClient) {}

  getExpenses(groupId: number) {
    return this.http.get<Expense[]>(`${this.expensesUrl}/${groupId}/expenses`);
  }

  addExpense(expenseDto: ExpenseDto): Observable<any> {
    return this.http.post<any>(`${this.expensesUrl}/expenses`, expenseDto);
  }

  
  settleExpense(userId: number, expense: GroupMember): Observable<any> {
    return this.http.put(`${this.expensesUrl}/settle-expense/${userId}`, expense);
  }


  getGroupsWithBalancesByUserId(userId: number): Observable<GroupWithMembers[]> {
    return this.http.get<GroupWithMembers[]>(`${this.url}/user/${userId}/groups-with-balances`);
  }

  getGroupMembers(groupId: number): Observable<GroupMember[]> {
    return this.http.get<GroupMember[]>(`${this.url}/${groupId}`);
  }


  getAllExpenses(): Observable<Expense[]> {
    return this.http.get<Expense[]>(`${this.expensesUrl}/expenses`);
  }

  getExpenseById(expenseId: number): Observable<Expense> {
    return this.http.get<Expense>(`${this.expensesUrl}/expenses/${expenseId}`);
  }

  updateExpense(expenseId: number, expense: Expense): Observable<Expense> {
    return this.http.put<Expense>(`${this.expensesUrl}/expenses/${expenseId}`, expense);
  }
}
