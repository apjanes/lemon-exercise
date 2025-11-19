import { Column } from "primereact/column";
import { DataTable } from "primereact/datatable";
import React from "react";
import AppLayout from "~/components/AppLayout";
import { useWorkItems } from "~/hooks/useWorkItems";

function HomePage(): React.ReactElement {
  const { data: workItems, isPending, error } = useWorkItems();

  if (workItems) {
    debugger;
  }
  return (
    <AppLayout>
      <div className="home-page">
        <h1>Work Items</h1>
        {workItems && (
          <DataTable value={workItems} loading={isPending}>
            <Column field="summary" header="Summary" />
          </DataTable>
        )}
      </div>
    </AppLayout>
  );
}

export default HomePage;
