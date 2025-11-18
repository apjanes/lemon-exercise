import React from "react";
import HomePage from "./pages/HomePage";
export interface IRoute {
  path: string;
  isDefault?: boolean;
  element: React.ReactElement;
}

export const routes: IRoute[] = [
  {
    path: "/",
    isDefault: true,
    element: <HomePage />,
  },
];
