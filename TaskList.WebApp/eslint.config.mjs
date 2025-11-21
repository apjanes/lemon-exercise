import eslint from "@eslint/js";
import prettier from "eslint-config-prettier";
import tseslint from "typescript-eslint";

export default tseslint.config(
  {
    ignores: [
      "**/node_modules/**",
      "**/wwwroot/dist/**",
      "**/bin/**",
      "**/obj/**",
    ],
  },
  {
    files: ["src/**/*.{js,ts,tsx}"],
    extends: [
      eslint.configs.recommended,
      tseslint.configs.recommended,
      tseslint.configs.stylistic,
      prettier,
    ],
  },
);
