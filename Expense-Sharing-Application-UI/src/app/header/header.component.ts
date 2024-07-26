import { Component,OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { ExpenseService } from '../services/expense.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { GroupMember } from '../models/GroupMember';
import { GroupWithMembers } from '../models/GroupWithMembers';


@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {
  userId: number | null = null;
 
  constructor(
    private router: Router,
    private authService: AuthService,
    private snackBar: MatSnackBar,
    private expenseService: ExpenseService
  ) {}

  ngOnInit(): void {
    this.usercall();
  }

  usercall(): void {
    this.userId = this.authService.getUserId();
  }

  isLoggedIn(): boolean {
    return this.authService.isLoggedIn();
  }

  isAdmin(): boolean {
    const userRole = this.authService.getUserRole();
    return userRole === 'admin'; // Adjust according to your actual role structure
  }

  navigateToLogin(): void {
    this.router.navigate(['/login']);
  }

  navigateToUserManagement(): void {
    this.router.navigate(['/user-management']);
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }

  settleExpense(): void {
    const userId = this.authService.getUserId();

    if (userId !== null) {
      // Confirm settlement action
      const confirmation = confirm('Are you sure you want to settle expenses?');
      if (!confirmation) {
        return; // Do nothing if user cancels
      }

      const expense: GroupMember = {
        groupMemberId: 0,
        groupId: 0, // Set appropriate values for groupId if necessary
        userId: userId,
        isSettled: false,
      };

      const observer = {
        next: (response: any) => {
          console.log("response", response);
          this.snackBar.open(response.Message, 'Close', {
            duration: 3000,
            panelClass: 'snackbar-popup'
          });
        },
        error: (error: any) => {
          this.snackBar.open('Failed to settle expense.', 'Close', {
            duration: 3000,
            panelClass: 'snackbar-popup-error'
          });
        }
      };

      this.expenseService.settleExpense(userId, expense).subscribe(observer);



    } else {
      this.snackBar.open('User ID is null.', 'Close', {
        duration: 3000,
        panelClass: 'snackbar-popup-error'
      });
    }
  }
}
