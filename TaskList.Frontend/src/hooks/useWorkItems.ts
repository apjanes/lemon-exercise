import { useQuery } from "@tanstack/react-query";
import apiClient from "~/api/apiClient";
import { WorkItem } from "~/models/WorkItem";

export function useWorkItems() {
  return useQuery({
    queryKey: ["work-items"],
    queryFn: async (): Promise<WorkItem[]> => {
      const response = await apiClient.get("/work-items");
      return response.data;
    },
  });
}
