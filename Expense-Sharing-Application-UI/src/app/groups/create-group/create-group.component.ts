import { Component } from '@angular/core';
import { FormBuilder, FormGroup, FormArray, Validators } from '@angular/forms';
import { GroupCreation } from 'src/app/models/Group';
import { GroupService } from 'src/app/services/group.service';

@Component({
  selector: 'app-create-group',
  templateUrl: './create-group.component.html',
  styleUrls: ['./create-group.component.css']
})
export class CreateGroupComponent {
  groupForm: FormGroup;
  maxMembers = 10;
  groupId?: number;
  successMessage: string = '';

  constructor(private fb: FormBuilder, private groupService: GroupService) {
    this.groupForm = this.fb.group({
      name: ['', Validators.required],
      description: [''],
      createdDate: [new Date(), Validators.required],
      members: this.fb.array([])
    });
  }

  get members(): FormArray {
    return this.groupForm.get('members') as FormArray;
  }

  addMember() {
    if (this.members.length < this.maxMembers) {
      this.members.push(this.fb.group({ email: ['', [Validators.required, Validators.email]] }));
    }
  }

  removeMember(index: number) {
    this.members.removeAt(index);
  }

  onSubmit() {
    if (this.groupForm.valid) {
      const groupData: GroupCreation = {
        name: this.groupForm.value.name,
        description: this.groupForm.value.description,
        createdDate: this.groupForm.value.createdDate,
      };
      const members = this.groupForm.value.members.map((member: { email: string }) => member.email);

      // Create the group first
      this.groupService.createGroup(groupData).subscribe(response => {
        this.groupId = response.groupId;

        // Add members to the created group
        if (this.groupId !== undefined) {
          this.addMembersToGroup(this.groupId, members);
        }

        // Set success message
        this.successMessage = 'Group created successfully!';
        setTimeout(() => {
          this.successMessage = '';
        }, 1000);
        // Optionally reset the form or specific fields
        this.groupForm.reset();
        while (this.members.length) {
          this.members.removeAt(0);
        }

      }, error => {
        console.error('Error creating group:', error);
      });
    }
  }

  addMembersToGroup(groupId: number, members: string[]) {
    members.forEach(email => {
      this.groupService.addMember(groupId, email).subscribe(response => {
        console.log(`Member ${email} added successfully to group ${groupId}`);
      }, error => {
        console.error(`Error adding member ${email}:`, error);
      });
    });
  }
}
