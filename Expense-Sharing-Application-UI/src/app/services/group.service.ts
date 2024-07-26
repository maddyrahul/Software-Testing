import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Group, GroupCreation } from '../models/Group';
import { Observable } from 'rxjs/internal/Observable';
import { UserWithBalance } from '../models/UserWithBalance';

@Injectable({
  providedIn: 'root'
})
export class GroupService {
  private groupsUrl = "https://localhost:7141/api/Groups";
  constructor(private http: HttpClient) {}

  private url="https://localhost:7141/api/GroupMembers";
  private memberUrl= "https://localhost:7141/api/GroupMembers/add"
  getAllGroups() {
    return this.http.get<Group[]>(this.groupsUrl);
  }

  createGroup(group: GroupCreation) {
    return this.http.post<Group>(this.groupsUrl, group);
  }

  deleteGroup(groupId: number) {
    return this.http.delete(`${this.groupsUrl}/${groupId}`);
  }

  addMember(groupId: number, email: string) {
    return this.http.post(`${this.memberUrl}`, {groupId, email });
  }

  getGroupsByUserId(userId: number): Observable<Group[]> {
    return this.http.get<Group[]>(`${this.groupsUrl}/user/${userId}`);
  }

  getGroupMembersWithBalances(groupId: number): Observable<UserWithBalance[]> {
    return this.http.get<UserWithBalance[]>(`${this.url}/WithBalances/${groupId}`);
  }
}