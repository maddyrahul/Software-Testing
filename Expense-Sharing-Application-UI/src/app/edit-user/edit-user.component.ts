import { Component, Input, OnInit } from '@angular/core';
import { User } from '../models/User';
import { UserService } from '../services/user.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-edit-user',
  templateUrl: './edit-user.component.html',
  styleUrls: ['./edit-user.component.css']
})
export class EditUserComponent implements OnInit {
  @Input() user: User = { userId: 0, email: '', role: '' }; // Default values
  userId!: number;
  successMessage = '';

  constructor(
    private userService: UserService,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.userId = this.route.snapshot.params['id'];
    this.loadUserDetails();
  }

  loadUserDetails(): void {
    this.userService.getUser(this.userId).subscribe((data: User) => {
      this.user = data;
    });
  }

  updateUser(): void {
    this.userService.updateUser(this.user.userId, this.user).subscribe(() => {
      this.successMessage = 'User updated successfully.';
      setTimeout(() => {
        this.successMessage = ''; 
        this.router.navigate(['/user-management']);
      }, 1000); 
    });
  }

  cancel(): void {
    this.router.navigate(['/user-management']);
  }
}
