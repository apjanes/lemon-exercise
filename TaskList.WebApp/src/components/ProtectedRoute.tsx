import React from "react";
import { Navigate, Outlet } from "react-router-dom";
import { useAuth } from "~/providers/AuthProvider";

export default function ProtectedRoute(): React.ReactElement {
  const { isAuthenticated, isInitializing } = useAuth();

  if (isInitializing) {
    return <div>Loading...</div>;
  }

  if (!isAuthenticated()) {
    return <Navigate to="/login" replace />;
  }

  return <Outlet />;
}
