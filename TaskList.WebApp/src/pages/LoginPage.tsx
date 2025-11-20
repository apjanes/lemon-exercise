import React, { useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { Button } from "primereact/button";
import { Card } from "primereact/card";
import { InputText } from "primereact/inputtext";
import AppLayout from "~/components/AppLayout";
import { useAuth } from "~/providers/AuthProvider";
import "~/pages/LoginPage.scss";

function LoginPage(): React.ReactElement {
  const [username, setUsername] = React.useState("");
  const [password, setPassword] = React.useState("");
  const { login: login, isAuthenticated } = useAuth();
  const navigate = useNavigate();
  const handleOnClick = async () => {
    await login(username, password);
  };

  useEffect(() => {
    if (isAuthenticated) {
      navigate("/", { replace: true });
    }
  }, [isAuthenticated]);

  return (
    <AppLayout>
      <div className="login-page">
        <h1>Login</h1>
        <Card>
          <div className="login-page__row">
            <label className="login-page__label" htmlFor="username">
              Username:
            </label>
            <InputText
              className="login-page__text"
              id="username"
              onChange={(e) => setUsername(e.target.value)}
            />
          </div>
          <div className="login-page__row">
            <label className="login-page__label" htmlFor="password">
              Password:
            </label>
            <InputText
              className="login-page__text"
              id="password"
              type="password"
              onChange={(e) => setPassword(e.target.value)}
            />
          </div>
          <div className="login-page__row">
            <Button
              label="Login"
              icon="pi pi-sign-in"
              onClick={handleOnClick}
            />
          </div>
        </Card>
      </div>
    </AppLayout>
  );
}

export default LoginPage;
