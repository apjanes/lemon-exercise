import React from "react";
interface IProps {
  children: React.ReactNode;
}

function AppLayout({ children }: IProps): React.ReactElement<IProps> {
  return (
    <div className="layout">
      <div className="layout__header-bar">
        <div className="layout__heading">
          <i className="layout__heading-icon pi pi-pen-to-square" />
          Task List
        </div>
      </div>

      <div className="layout__content">{children}</div>
    </div>
  );
}

export default AppLayout;
