import path from "path";
import process from "process";
import yargs from "yargs";
import { fileURLToPath } from "url";

const __filename = fileURLToPath(import.meta.url);
const __dirname = path.dirname(__filename);
const args = yargs(process.argv).argv;
const env = args.env?.toLowerCase() || "development";
const mode = env == "development" ? "development" : "production";

process.env.NODE_ENV = env;

export default {
  mode,
  devtool: "inline-source-map",
  entry: "./src/index.tsx",
  module: {
    rules: [
      {
        test: /\.tsx?$/,
        use: [
          {
            loader: "ts-loader",
            options: {
              allowTsInNodeModules: true, // Enable compilation of .ts files in node_modules
            },
          },
          {
            loader: "source-map-loader",
          },
        ],
        exclude: /node_modules/,
      },
      {
        test: /\.css$/i,
        use: ["style-loader", "css-loader"],
      },
      {
        test: /\.scss$/i,
        use: ["style-loader", "css-loader", "sass-loader"],
      },
      {
        test: /\.(woff|woff2|eot|ttf|otf)$/i,
        type: "asset/resource",
      },
      {
        test: /\.m?js$/,
        resolve: {
          fullySpecified: false,
        },
      },
    ],
  },
  resolve: {
    alias: {
      "~": path.resolve(__dirname, "src"),
    },
    extensions: [".tsx", ".ts", ".js", ".css", ".scss"],
    mainFields: ["main", "module"],
  },
  output: {
    filename: "app.js",
    path: path.resolve(__dirname, "wwwroot/dist"),
  },
};
