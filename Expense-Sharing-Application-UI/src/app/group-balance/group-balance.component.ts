import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { GroupWithMembers } from '../models/GroupWithMembers';
import { ExpenseService } from '../services/expense.service';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-group-balance',
  templateUrl: './group-balance.component.html',
  styleUrls: ['./group-balance.component.css']
})
export class GroupBalanceComponent implements OnInit, OnDestroy {
  userId: number = 0;
  groupWithMembers: GroupWithMembers[] = [];
  error!: string;
  isLoading: boolean = false;
  private subscription: Subscription = new Subscription();

  constructor(
    private route: ActivatedRoute,
    private authService: AuthService,
    private expenseService: ExpenseService
  ) { }

  ngOnInit(): void {
    this.fetchBalances();
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

  fetchBalances() {
    this.userId = this.authService.getUserId();
    console.log("userid in group balnce",this.userId)
    this.isLoading = true;
    const sub = this.expenseService.getGroupsWithBalancesByUserId(this.userId).subscribe(
      (data) => {
        this.groupWithMembers = data;


        
        this.error = '';
        this.isLoading = false;
      },
      (err) => {
        this.error = 'An error occurred while fetching data.';
        console.error(err);
        this.isLoading = false;
      }
    );
    this.subscription.add(sub);
  }
}
