import React from "react";
import { Icon } from "~/components/icons/Icon";

interface Props {
  className?: string;
  onClick?: () => void;
}
export function DeleteIcon({
  className,
  onClick,
}: Props): React.ReactElement<Props> {
  return (
    <Icon
      className={className}
      iconName="trash"
      title="Delete"
      onClick={onClick}
    />
  );
}
