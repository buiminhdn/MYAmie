import { forwardRef, useState } from "react";
import cx from "classnames";

interface InputProps extends React.InputHTMLAttributes<HTMLInputElement> {
  id?: string;
  label?: string;
  errorMessage?: string;
  containerClassName?: string;
  showPasswordToggle?: boolean;
}

const Input = forwardRef<HTMLInputElement, InputProps>(
  (
    {
      id,
      label,
      type = "text",
      placeholder,
      readOnly,
      errorMessage,
      className,
      disabled = false,
      hidden = false,
      containerClassName,
      showPasswordToggle = type === "password",
      ...rest
    },
    ref
  ) => {
    const [showPassword, setShowPassword] = useState(false);

    const togglePasswordVisibility = () => {
      setShowPassword((prev) => !prev);
    };

    const inputType = showPassword && type === "password" ? "text" : type;

    return (
      <div
        className={cx("w-full", containerClassName, {
          hidden: hidden,
        })}
      >
        {label && (
          <label
            htmlFor={id}
            className={cx("mb-2 block font-medium", {
              "text-gray-400": disabled,
            })}
          >
            {label}
          </label>
        )}
        <div className="relative">
          <input
            ref={ref}
            id={id}
            type={inputType}
            placeholder={placeholder}
            readOnly={readOnly}
            disabled={disabled}
            aria-describedby={errorMessage ? `${id}-error` : undefined}
            aria-invalid={!!errorMessage}
            className={cx(
              "p-3 w-full border rounded-md transition-all",
              "outline-gray-300",
              {
                "border-gray-200": !errorMessage,
                "border-red-600": errorMessage,
                "bg-gray-100 cursor-not-allowed": disabled,
                "pr-10": showPasswordToggle,
              },
              className
            )}
            {...rest}
          />
          {showPasswordToggle && (
            <button
              type="button"
              onClick={togglePasswordVisibility}
              className="absolute right-3 bottom-3.5"
              aria-label={showPassword ? "Hide password" : "Show password"}
              disabled={disabled}
            >
              <i
                className={cx(
                  "text-gray-400 hover:text-gray-600",
                  "fa-lg fa-regular",
                  {
                    "fa-eye-slash": showPassword,
                    "fa-eye": !showPassword,
                    "opacity-50": disabled,
                  }
                )}
              />
            </button>
          )}
        </div>
        {errorMessage && (
          <p
            id={`${id}-error`}
            className="text-xs text-red-600 mt-1.5"
            aria-live="assertive"
          >
            {errorMessage}
          </p>
        )}
      </div>
    );
  }
);

Input.displayName = "Input";

export default Input;
