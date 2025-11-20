import { Button } from "primereact/button";
import React from "react";
import { useAuth } from "~/providers/AuthProvider";
import "~/components/AppLayout.scss";
interface IProps {
  children: React.ReactNode;
}

export function AppLayout({ children }: IProps): React.ReactElement<IProps> {
  const { logout: userLogout, isAuthenticated } = useAuth();
  const handleLogoutClick = async () => {
    await userLogout();
  };

  return (
    <div className="layout">
      <div className="layout__header-bar">
        <div className="layout__header-left-content">
          <i className="layout__heading-icon pi pi-pen-to-square" />
          Task List
        </div>
        <div className="layout__header-right-content">
          {isAuthenticated && (
            <Button label="Logout" onClick={handleLogoutClick} />
          )}
        </div>
      </div>

      <div className="layout__content">{children}</div>
    </div>
  );
}

export default AppLayout;
