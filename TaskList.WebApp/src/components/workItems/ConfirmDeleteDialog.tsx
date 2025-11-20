import React, { useEffect, useState } from "react";
import { Dialog } from "primereact/dialog";
import { Button } from "primereact/button";
import { WorkItem } from "~/models/WorkItem";

interface Props {
  deleting: WorkItem | undefined;
  isVisible: boolean;
  onConfirmed: (choice: boolean, toDelete: WorkItem | undefined) => void;
}

export function ConfirmDeleteDialog({
  deleting,
  isVisible,
  onConfirmed,
}: Props): React.ReactElement<Props> {
  const handleConfirmClick = (
    choice: boolean,
    toDelete: WorkItem | undefined,
  ) => {
    onConfirmed(choice, toDelete);
  };

  return (
    <Dialog
      visible={isVisible}
      onHide={() => handleConfirmClick(false, deleting)}
      header="Are you sure?"
      footer={
        <>
          <Button
            label="No"
            icon="pi pi-times"
            onClick={() => handleConfirmClick(false, deleting)}
            autoFocus
          />
          <Button
            label="Yes"
            icon="pi pi-check"
            className="p-button-outlined"
            onClick={() => {
              handleConfirmClick(true, deleting);
            }}
          />
        </>
      }
    >
      Are you sure you want to delete this work item?
    </Dialog>
  );
}

export default ConfirmDeleteDialog;
