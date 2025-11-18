import React from "react";
import Layout from "~/components/AppLayout";

function HomePage(): React.ReactElement {
  return (
    <Layout>
      <div className="example-page">
        <h1>Example</h1>
        <p>
          This is the default output for a CNU SPA project. Modify the contents
          of this page to implement your own requirements.
        </p>
        <h2>Developing</h2>
        <p>
          It is recommended to develop using two IDEs: Visual Studio, and
          VSCode. This is because each IDE has strength in different areas.
          Visual Studio is good for C# development which is needed for the API
          work and VSCode is best for web development using TypeScript and
          React.
        </p>
        <h3>Visual Studio</h3>
        <p>
          Open your newly created project using the Visual Studio Solution File.
          This will contain both the C# code (for API development) and the web
          code which will be opened in VSCode.
        </p>
        <p>
          Build the C# portion of the new project using{" "}
          <code>Build &gt; Build Solution</code> or using the keyboard shortcut:{" "}
          <code>Ctrl-Shift-B</code>.
        </p>
        <p>
          The code should run as usual using{" "}
          <code>Debug &gt; Start Debugging</code> or <code>F5</code>. Before
          doing that the web portion needs to be transpiled and running as
          described in the next section.
        </p>
        <h3>VSCode</h3>
        The web portion of your new application can be found at{" "}
        <code>/[ProjectDir]/UI</code>. Open this folder in VSCode.
        <h2>Code Structure</h2>
        The source code is laid out as described below. The location of{" "}
        <code>/</code> is the root of the project:
        <ul>
          <li>
            <code>/Controllers</code>: The API Controllers for the application.
          </li>
        </ul>
      </div>
    </Layout>
  );
}

export default HomePage;
