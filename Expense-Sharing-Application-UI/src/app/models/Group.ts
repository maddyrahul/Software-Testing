import { UserWithBalance } from "./UserWithBalance";

export interface Group {
  groupId: number;  // Make groupId optional
  name: string;
  description: string;
  createdDate: Date;
  members?: UserWithBalance[]; 
}

export interface GroupCreation {
  name: string;
  description: string;
  createdDate: Date;
}
