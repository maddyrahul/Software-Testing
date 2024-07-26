import { Component, OnInit } from '@angular/core';
import { GroupService } from '../services/group.service';
import { AuthService } from '../services/auth.service';
import { Group } from '../models/Group';

@Component({
  selector: 'app-group-details',
  templateUrl: './group-details.component.html',
  styleUrls: ['./group-details.component.css']
})
export class GroupDetailsComponent implements OnInit {
  userId: number = 0;
  groups: Group[] = [];

  constructor(private groupService: GroupService, private authService: AuthService) { }

  ngOnInit(): void {
    this.userId = this.authService.getUserId(); // Adjust this to get the logged-in user's ID
    this.fetchGroupsByUserId();
  }

  fetchGroupsByUserId(): void {
    this.groupService.getGroupsByUserId(this.userId).subscribe(
      (groups) => {
        this.groups = groups;
        this.loadGroupMembers();
      },
      (error) => {
        console.error('Error fetching groups:', error);
      }
    );
  }

  loadGroupMembers(): void {
    this.groups.forEach(group => {
      this.groupService.getGroupMembersWithBalances(group.groupId).subscribe(
        (members) => {
          group.members = members;
        },
        (error) => {
          console.error('Error fetching group members:', error);
        }
      );
    });
  }
}
