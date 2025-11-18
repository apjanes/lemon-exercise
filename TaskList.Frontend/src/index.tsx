import React from "react";
import { createRoot } from "react-dom/client";
import { createBrowserRouter, RouterProvider } from "react-router-dom";
import { routes } from "~/routing";
import { PrimeReactProvider } from "primereact/api";
import "primereact/resources/themes/bootstrap4-light-blue/theme.css";
import "primereact/resources/primereact.min.css";
import "primeicons/primeicons.css";
import "~/css/styles.css";

const container = document.getElementById("root");

if (!container) {
  throw new Error(
    'The root for the React application cannot be found. Ensure an element with `id="root"` exists.',
  );
}

const root = createRoot(container);
const router = createBrowserRouter(routes);
root.render(
  <PrimeReactProvider>
    <RouterProvider router={router} />
  </PrimeReactProvider>,
);
