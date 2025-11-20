import { WorkItem } from "~/models/WorkItem";
import apiClient from "./apiClient";

export const workItems = {
  async delete(id: string): Promise<void> {
    await apiClient.delete(`/work-items/${id}`);
  },

  async save(workItem: Partial<WorkItem>): Promise<WorkItem> {
    if (workItem.id) {
      const { data } = await apiClient.put(
        `/work-items/${workItem.id}`,
        workItem,
      );
      return data;
    }

    const { data } = await apiClient.post("/work-items", workItem);

    return data;
  },

  async setComplete(id: string, isComplete: boolean): Promise<WorkItem> {
    const { data } = await apiClient.put(
      `/work-items/${id}/complete/${isComplete}`,
    );

    return data;
  },
};

export default workItems;
