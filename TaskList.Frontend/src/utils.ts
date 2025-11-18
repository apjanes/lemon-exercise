import { trimCharsStart, trimCharsEnd } from "lodash/fp";

const prefix = trimCharsEnd("/", process.env.URI_PREFIX || "");

export const getUri = (relativeUri: string): string =>
  `${prefix}/${trimCharsStart("/")(relativeUri)}`;
