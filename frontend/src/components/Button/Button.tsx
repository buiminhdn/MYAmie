import cx from "classnames";
import { Link } from "react-router-dom";

interface ButtonProps {
  variant?: "solid" | "outline" | "ghost";
  shape?: "rectangle" | "rounded";
  id?: string;
  to?: string;
  className?: string;
  padding?: string;
  type?: "button" | "submit" | "reset";
  disabled?: boolean;
  onClick?: () => void;
  children: React.ReactNode;
}

const Button = ({
  variant = "solid",
  shape = "rectangle",
  type = "button",
  padding = "px-3 py-2",
  disabled = false,
  id,
  to,
  className,
  onClick,
  children,
}: ButtonProps) => {
  const classes = cx(
    "transition-colors duration-300 border-2 whitespace-nowrap",
    padding,
    {
      solid: "text-white bg-primary border-primary hover:opacity-90",
      outline: "text-primary border-primary bg-primary-lighter hover:bg-white",
      ghost: "bg-white border-gray-200 hover:border-gray-500",
    }[variant],
    {
      rectangle: "rounded-md",
      rounded: "rounded-full",
    }[shape],
    { "opacity-50 cursor-not-allowed": disabled },
    className
  );

  return to ? (
    <Link id={id} to={to} className={classes}>
      {children}
    </Link>
  ) : (
    <button
      id={id}
      type={type}
      className={classes}
      onClick={onClick}
      disabled={disabled}
    >
      {children}
    </button>
  );
};

export default Button;
