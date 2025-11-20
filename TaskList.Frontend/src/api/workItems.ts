import { WorkItem } from "~/models/WorkItem";
import apiClient from "./apiClient";

const workItems = {
  async save(workItem: Partial<WorkItem>): Promise<WorkItem> {
    const responst = await apiClient.put("/work-items", workItem);
    return responst.data;
  },
};

export default workItems;
