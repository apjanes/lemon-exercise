import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import { SubmitHandler, useForm } from "react-hook-form";
import { Button } from "primereact/button";
import { Card } from "primereact/card";
import { InputText } from "primereact/inputtext";
import { AppLayout } from "~/components/AppLayout";
import { useAuth } from "~/providers/AuthProvider";
import { AxiosError } from "axios";
import { LoginRequest } from "~/models/LoginRequest";

import "~/pages/LoginPage.scss";

function LoginPage(): React.ReactElement {
  const [username, setUsername] = React.useState("");
  const [password, setPassword] = React.useState("");
  const [errorMessage, setErrorMessage] = React.useState<string | null>(null);
  const [isSubmiting, setIsSubmitting] = useState(false);

  const { login, isAuthenticated } = useAuth();
  const {
    formState: { errors },
    handleSubmit,
    reset,
  } = useForm<LoginRequest>();

  const navigate = useNavigate();
  const onSubmit: SubmitHandler<LoginRequest> = async (data) => {
    reset();
    setErrorMessage(null);
    setIsSubmitting(true);
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
    } finally {
      setIsSubmitting(false);
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
          <form id="login-form" onSubmit={handleSubmit(onSubmit)}>
            {errorMessage && (
              <div className="login-page__error">{errorMessage}</div>
            )}
            <div className="login-page__row">
              <div className="login-page__error">
                {errors.username && <p>{errors.username.message}</p>}
              </div>
              <label className="login-page__label" htmlFor="username">
                Username:
              </label>
              <InputText
                className="login-page__text"
                id="username"
                {...register("username", { required: "username is required" })}
                onChange={(e) => setUsername(e.target.value)}
              />
            </div>
            <div className="login-page__row">
              <div className="login-page__error">
                {errors.password && <p>{errors.password.message}</p>}
              </div>
              <label className="login-page__label" htmlFor="password">
                Password:
              </label>
              <InputText
                className="login-page__text"
                id="password"
                type="password"
                {...register("password", { required: "password is required" })}
                onChange={(e) => setPassword(e.target.value)}
              />
            </div>
            <div className="login-page__row">
              <Button
                form="login-form"
                label="Login"
                icon={isSubmiting ? "pi pi-spin pi-spinner" : "pi pi-sign-in"}
                type="submit"
                disabled={isSubmiting}
                autoFocus
              />
            </div>
          </form>
        </Card>
      </div>
    </AppLayout>
  );
}

export default LoginPage;
