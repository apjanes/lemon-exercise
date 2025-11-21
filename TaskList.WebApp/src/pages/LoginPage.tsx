import React from "react";
import { useNavigate } from "react-router-dom";
import { Button } from "primereact/button";
import { Card } from "primereact/card";
import { InputText } from "primereact/inputtext";
import AppLayout from "~/components/AppLayout";
import { useAuth } from "~/providers/AuthProvider";
import "~/pages/LoginPage.scss";
import { AxiosError } from "axios";

function LoginPage(): React.ReactElement {
  const [username, setUsername] = React.useState("");
  const [password, setPassword] = React.useState("");
  const [errorMessage, setErrorMessage] = React.useState<string | null>(null);
  const { login, isAuthenticated } = useAuth();
  const navigate = useNavigate();
  const handleOnClick = async () => {
    try {
      await login(username, password);
      if (isAuthenticated()) {
        navigate("/", { replace: true });
      }
    } catch (error) {
      const axiosError = error as AxiosError;
      if (axiosError?.status === 401) {
        setErrorMessage("Invalid username or password.");
      } else {
        setErrorMessage("An unexpected error occurred. Please try again.");
      }
    }
  };

  if (isAuthenticated()) {
    navigate("/", { replace: true });
  }

  return (
    <AppLayout>
      <div className="login-page">
        <h1>Login</h1>
        <Card>
          {errorMessage && (
            <div className="login-page__error">{errorMessage}</div>
          )}
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
