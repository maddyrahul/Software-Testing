import { UserWithBalance } from "./UserWithBalance";

export interface GroupWithMembers {
    groupId: number;
    members: UserWithBalance[];
  }