import eslint from "@eslint/js";
import prettier from "eslint-config-prettier";
import tslint from "typescript-eslint";

export default tslint.config({
  files: ["src/**/*.js", "src/**/*.ts", "src/**/*.tsx"],
  ignores: ["node_modules"],
  extends: [eslint.configs.recommended, tslint.configs.stylistic, prettier],
  rules: {
    "@typescript-eslint/naming-convention": [
      "error",
      {
        selector: "interface",
        format: ["PascalCase"],
        prefix: ["I"],
      },
    ],
  },
});
