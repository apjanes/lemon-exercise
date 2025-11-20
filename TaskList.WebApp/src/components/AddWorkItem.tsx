import React, { useState } from "react";
import { SubmitHandler, useForm } from "react-hook-form";
import { Dialog } from "primereact/dialog";
import { Button } from "primereact/button";
import { InputText } from "primereact/inputtext";
import { InputTextarea } from "primereact/inputtextarea";
import { WorkItem } from "~/models/WorkItem";
import workItems from "~/api/workItems";
import "~/css/AddWorkItem.scss";

interface Props {
  onAdded: () => void;
}

function AddWorkItem({ onAdded }: Props): React.ReactElement<Props> {
  const [dialogVisible, setDialogVisible] = useState(false);
  const [isSubmiting, setIsSubmitting] = useState(false);
  const {
    formState: { errors },
    handleSubmit,
    register,
    reset,
  } = useForm<WorkItem>();

  const handleCancelClick = () => {
    if (dialogVisible) {
      reset();
      setDialogVisible(false);
    }
  };

  const handleIconClick = () => {
    setDialogVisible(true);
  };

  const onSubmit: SubmitHandler<WorkItem> = async (data) => {
    setIsSubmitting(true);
    try {
      await workItems.save(data);
      reset();
      setDialogVisible(false);
      onAdded();
    } finally {
      setIsSubmitting(false);
    }
  };

  const footer = (
    <div>
      <div className="add-work-item__dialog__error-messages">
        {errors.summary && <p>{errors.summary.message}</p>}
      </div>
      <Button
        label="Cancel"
        onClick={handleCancelClick}
        className="p-button-outlined"
      />
      <Button
        className="add-work-item__dialog__add-button"
        form="add-work-item-form"
        label={!isSubmiting ? "Add" : ""}
        type="submit"
        disabled={isSubmiting}
        icon={isSubmiting ? "pi pi-spin pi-spinner" : undefined}
        autoFocus
      />
    </div>
  );

  return (
    <div className="add-work-item">
      <Button icon="pi pi-plus" onClick={handleIconClick} />
      <Dialog
        className="add-work-item__dialog"
        footer={footer}
        header="Add Work Item"
        visible={dialogVisible}
        onHide={handleCancelClick}
      >
        <form id="add-work-item-form" onSubmit={handleSubmit(onSubmit)}>
          <div className="add-work-item__dialog__row">
            <label htmlFor="summary">
              Summary: <i className="required">*</i>
            </label>{" "}
            <InputText
              className="add-work-item__dialog__input"
              id="summary"
              {...register("summary", { required: "summary is required" })}
            />
          </div>
          <div className="add-work-item__dialog__row">
            <label htmlFor="description">Description:</label>{" "}
            <InputTextarea
              className="add-work-item__dialog__input"
              id="description"
              {...register("description")}
            />
          </div>
        </form>
      </Dialog>
    </div>
  );
}

export default AddWorkItem;
