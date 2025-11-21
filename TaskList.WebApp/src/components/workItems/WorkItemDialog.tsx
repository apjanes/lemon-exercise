import React, { useEffect, useState } from "react";
import { SubmitHandler, useForm } from "react-hook-form";
import { Dialog } from "primereact/dialog";
import { Button } from "primereact/button";
import { InputText } from "primereact/inputtext";
import { InputTextarea } from "primereact/inputtextarea";
import { WorkItem } from "~/models/WorkItem";
import "~/components/workItems/WorkItemDialog.scss";

interface Props {
  editing?: WorkItem;
  isVisible: boolean;
  onCanceled: () => void;
  onSaving: (toSave: WorkItem) => Promise<void>;
  onSaved: () => void;
}

export function WorkItemDialog({
  editing,
  isVisible,
  onCanceled,
  onSaved,
  onSaving,
}: Props): React.ReactElement<Props> {
  const dialogTitle = editing ? "Edit Work Item" : "Add Work Item";
  const [isSubmiting, setIsSubmitting] = useState(false);
  const {
    formState: { errors },
    handleSubmit,
    register,
    reset,
  } = useForm<WorkItem>();

  useEffect(() => {
    if (editing) {
      reset(editing);
    }
  }, [editing]);

  const handleCancelClick = () => {
    reset();
    onCanceled();
  };

  const onSubmit: SubmitHandler<WorkItem> = async (data) => {
    setIsSubmitting(true);
    try {
      await onSaving(data);
      reset();
      onSaved();
    } finally {
      setIsSubmitting(false);
    }
  };

  const footer = (
    <div>
      <div className="work-item-dialog__error-messages">
        {errors.title && <p>{errors.title.message}</p>}
      </div>
      <Button
        label="Cancel"
        onClick={handleCancelClick}
        className="p-button-outlined"
      />
      <Button
        className="work-item-dialog__add-button"
        form="work-item-form"
        label={!isSubmiting ? "Save" : ""}
        type="submit"
        disabled={isSubmiting}
        icon={isSubmiting ? "pi pi-spin pi-spinner" : undefined}
        autoFocus
      />
    </div>
  );

  return (
    <Dialog
      className="work-item-dialog"
      footer={footer}
      header={dialogTitle}
      visible={isVisible}
      onHide={handleCancelClick}
    >
      <form id="work-item-form" onSubmit={handleSubmit(onSubmit)}>
        <div className="work-item-dialog__row">
          <label htmlFor="title-input">
            Title: <i className="required">*</i>
          </label>{" "}
          <InputText
            className="work-item-dialog__input"
            id="title-input"
            maxLength={500}
            {...register("title", { required: "title is required" })}
          />
        </div>
        <div className="work-item-dialog__row">
          <label htmlFor="description-input">Description:</label>{" "}
          <InputTextarea
            className="work-item-dialog__input"
            id="description-input"
            maxLength={1000}
            {...register("description")}
          />
        </div>
      </form>
    </Dialog>
  );
}

export default WorkItemDialog;
