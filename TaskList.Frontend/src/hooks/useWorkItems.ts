import { useQuery } from "@tanstack/react-query";
import api from "~/api";
import { WorkItem } from "~/dtos/WorkItem";

export function useWorkItems() {
  return useQuery({
    queryKey: ["work-items"],
    queryFn: async (): Promise<WorkItem[]> => {
      const response = await api.get("/work-items");
      return response.data;
    },
  });
}
