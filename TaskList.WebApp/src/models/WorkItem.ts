import { User } from "./User";

export interface WorkItem {
  createdAt: string;
  createdBy: User;
  description?: string;
  id: string;
  isComplete: boolean;
  title: string;
}
