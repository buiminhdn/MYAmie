import cx from "classnames";

interface IconBtnProps {
  id?: string;
  onClick?: () => void;
  icon: string;
  className?: string;
  iconSize?: string;
  size?: string;
  effect?: "default" | "opacity" | "scale";
  hasBackground?: boolean;
}

function IconBtn({
  id,
  onClick,
  icon,
  className,
  iconSize = "fa-lg",
  size = "size-8",
  effect = "default",
  hasBackground = true, // Default to true for backward compatibility
}: IconBtnProps) {
  const effectClasses = {
    default: hasBackground ? "hover:bg-gray-200" : "",
    opacity: "bg-black bg-opacity-50 hover:bg-opacity-80 text-white",
    scale: "hover:scale-110",
  };

  return (
    <button
      id={id}
      type="button"
      className={cx(
        "rounded-full flex flex-none items-center justify-center transition-all duration-300",
        effectClasses[effect],
        !hasBackground && "bg-transparent", // Add this if you want no background
        size,
        className
      )}
      onClick={onClick}
      aria-label={icon.replace("fa-", "").replace("-", " ")}
    >
      <i className={cx("fa-regular", iconSize, icon)}></i>
    </button>
  );
}

export default IconBtn;
