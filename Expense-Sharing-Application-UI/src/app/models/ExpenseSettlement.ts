import { Expense } from "./Expense";
import { User } from "./User";

export class ExpenseSettlement {
    expenseSettlementId?: number;
    amount?: number;
    date?: Date;
    expenseId?: number;
    expense?: Expense;
    paidById?: number;
    paidBy?: User;
    receivedById?: number;
    receivedBy?: User;
}