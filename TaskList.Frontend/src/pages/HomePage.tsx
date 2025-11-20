import { Column } from "primereact/column";
import { DataTable } from "primereact/datatable";
import React from "react";
import AddWorkItem from "~/components/AddWorkItem";
import AppLayout from "~/components/AppLayout";
import { useWorkItems } from "~/hooks/useWorkItems";
import { WorkItem } from "~/models/WorkItem";

function HomePage(): React.ReactElement {
  const { data: workItems, isPending, refetch } = useWorkItems();
  const handleWorkItemAdded = () => {
    refetch();
  };

  return (
    <AppLayout>
      <div className="home-page">
        <h1>
          Work Items <AddWorkItem onAdded={handleWorkItemAdded} />
        </h1>
        {/* {workItems && ( */}
        <DataTable
          value={workItems}
          loading={isPending}
          emptyMessage="No work items found."
        >
          <Column field="summary" header="Summary" />
        </DataTable>
        {/* )} */}
      </div>
    </AppLayout>
  );
}

export default HomePage;
