import React, { useState } from "react";
import { Column } from "primereact/column";
import { DataTable } from "primereact/datatable";
import { workItems as workItemsApi } from "~/api/workItems";
import { DeleteIcon } from "~/components/icons/DeleteIcon";
import { EditIcon } from "~/components/icons/EditIcon";
import { useWorkItems } from "~/hooks/useWorkItems";
import { WorkItem } from "~/models/WorkItem";
import { AppLayout } from "~/components/AppLayout";
import { Button } from "primereact/button";
import { WorkItemDialog } from "~/components/workItems/WorkItemDialog";
import { ConfirmDeleteDialog } from "~/components/workItems/ConfirmDeleteDialog";
import "~/pages/HomePage.scss";
import { Checkbox } from "primereact/checkbox";
enum DialogType {
  None,
  Add,
  Edit,
  Delete,
}

function HomePage(): React.ReactElement {
  const { data: workItems, isPending, refetch } = useWorkItems();
  const [visibleDialogType, setVisibleDialogType] = useState<DialogType>(
    DialogType.None,
  );
  const [activeWorkItem, setActiveWorkItem] = useState<WorkItem | undefined>();

  const createActions = (rowData: WorkItem) => {
    return (
      <>
        <EditIcon
          className="mr-50"
          onClick={() => handleEditClicked(rowData)}
        />
        <DeleteIcon onClick={() => handleDeleteClicked(rowData)} />
      </>
    );
  };

  const createIsComplete = (rowData: WorkItem) => {
    return (
      <Checkbox
        checked={rowData.isComplete}
        onChange={(e) => handleCompletionChanged(e.checked || false, rowData)}
      />
    );
  };

  const handleAddClicked = () => {
    setVisibleDialogType(DialogType.Add);
  };

  const handleCanceled = () => {
    setActiveWorkItem(undefined);
    setVisibleDialogType(DialogType.None);
  };

  const handleCompletionChanged = async (
    isComplete: boolean,
    rowData: WorkItem,
  ) => {
    await workItemsApi.setComplete(rowData.id, isComplete);
    rowData.isComplete = isComplete;
    refetch();
  };

  const handleDeleteClicked = (rowData: WorkItem) => {
    setActiveWorkItem(rowData);
    setVisibleDialogType(DialogType.Delete);
  };

  const handleDeleteConfirmed = async (
    choice: boolean,
    toDelete: WorkItem | undefined,
  ) => {
    if (choice && toDelete) {
      await workItemsApi.delete(toDelete.id);
      refetch();
    }
    setActiveWorkItem(undefined);
    setVisibleDialogType(DialogType.None);
  };

  // DEBUG: on edit the item should be fetch from the API
  const handleEditClicked = (rowData: WorkItem) => {
    setActiveWorkItem(rowData);
    setVisibleDialogType(DialogType.Edit);
  };

  const handleSaving = async (toSave: WorkItem) => {
    await workItemsApi.save(toSave);
  };

  const handleSaved = () => {
    refetch();
    setVisibleDialogType(DialogType.None);
  };

  return (
    <AppLayout>
      <div className="home-page">
        <h1 className="home-page__title">
          Tasks{" "}
          <div className="home-page__title-content">
            <Button
              type="button"
              aria-label="Add new task"
              icon="pi pi-plus"
              onClick={handleAddClicked}
            />
          </div>
        </h1>
        <DataTable
          value={workItems}
          loading={isPending}
          emptyMessage="No task are found."
          removableSort
        >
          <Column
            field="isComplete"
            body={createIsComplete}
            style={{ width: "1%" }}
            sortable
          />
          <Column field="title" header="Title" sortable />
          <Column field="description" header="Description" sortable />
          <Column header="Actions" body={createActions} />
        </DataTable>
      </div>

      <WorkItemDialog
        isVisible={visibleDialogType === DialogType.Add}
        onCanceled={handleCanceled}
        onSaved={handleSaved}
        onSaving={handleSaving}
      />

      <WorkItemDialog
        editing={activeWorkItem}
        isVisible={visibleDialogType === DialogType.Edit}
        onCanceled={handleCanceled}
        onSaved={handleSaved}
        onSaving={handleSaving}
      />

      <ConfirmDeleteDialog
        deleting={activeWorkItem}
        isVisible={visibleDialogType === DialogType.Delete}
        onConfirmed={handleDeleteConfirmed}
      />
    </AppLayout>
  );
}

export default HomePage;
