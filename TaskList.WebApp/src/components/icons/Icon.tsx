import React from "react";

interface Props {
  className?: string;
  iconName: string;
  title: string;
  onClick?: () => void;
}
export function Icon({
  className,
  iconName,
  title,
  onClick,
}: Props): React.ReactElement<Props> {
  return (
    <i
      className={`${className + " "}clickable pi pi-${iconName}`}
      title={title}
      onClick={onClick}
    ></i>
  );
}
