import { Component, OnInit } from '@angular/core';
import { GroupService } from '../services/group.service';
import { Group } from '../models/Group';

@Component({
  selector: 'app-group-list',
  templateUrl: './group-list.component.html',
  styleUrls: ['./group-list.component.css']
})
export class GroupListComponent implements OnInit {
  groups: Group[] = [];

  constructor(private groupService: GroupService) { }

  ngOnInit(): void {
    this.loadGroups();
  }

  loadGroups() {
    this.groupService.getAllGroups().subscribe(
      (data) => {
        this.groups = data;
        console.log("group-list", this.groups);
        this.loadGroupMembers();
      },
      (error) => {
        console.error('Error loading groups', error);
      }
    );
  }

  loadGroupMembers() {
    this.groups.forEach(group => {
      this.groupService.getGroupMembersWithBalances(group.groupId).subscribe(
        (members) => {
          group.members = members;
        },
        (error) => {
          console.error('Error loading group members', error);
        }
      );
    });
  }

  deleteGroup(groupId: number) {
    if (confirm('Are you sure you want to delete this group?')) {
      this.groupService.deleteGroup(groupId).subscribe(
        () => {
          this.groups = this.groups.filter(g => g.groupId !== groupId);
          console.log('Group deleted successfully.');
        },
        (error) => {
          console.error('Error deleting group', error);
        }
      );
    }
  }
}

