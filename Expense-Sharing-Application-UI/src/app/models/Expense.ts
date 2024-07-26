import { Group } from "./Group";
import { User } from "./User";

export interface Expense {
  expenseId: number;
  description?: string;
  amount: number;
  date: Date;
  paidById: number;
  paidBy?: User;
  groupId: number;
  group?: Group;
 
  
}