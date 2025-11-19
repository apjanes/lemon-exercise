import React from "react";
import { createRoot } from "react-dom/client";
import { createBrowserRouter, RouterProvider } from "react-router-dom";
import { routes } from "~/routing";
import { PrimeReactProvider } from "primereact/api";
import "primereact/resources/themes/bootstrap4-light-blue/theme.css";
import "primereact/resources/primereact.css";
import "primeicons/primeicons.css";
import "~/css/styles.scss";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { AuthProvider } from "./providers/AuthProvider";

const container = document.getElementById("root");

if (!container) {
  throw new Error(
    'The root for the React application cannot be found. Ensure an element with `id="root"` exists.',
  );
}

const root = createRoot(container);
const router = createBrowserRouter(routes);
root.render(
  <AuthProvider>
    <PrimeReactProvider>
      <QueryClientProvider client={new QueryClient()}>
        <RouterProvider router={router} />
      </QueryClientProvider>
    </PrimeReactProvider>
  </AuthProvider>,
);
