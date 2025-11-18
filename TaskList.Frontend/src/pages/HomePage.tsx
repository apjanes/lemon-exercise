import React from "react";
import AppLayout from "~/components/AppLayout";

function HomePage(): React.ReactElement {
  return (
    <AppLayout>
      <div className="home-page">
        <h1>Welcome</h1>
      </div>
    </AppLayout>
  );
}

export default HomePage;
