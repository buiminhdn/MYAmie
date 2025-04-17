import { forwardRef } from "react";
import cx from "classnames";

interface TextareaProps {
  id?: string;
  label?: string;
  placeholder?: string;
  value?: string;
  onChange?: (event: React.ChangeEvent<HTMLTextAreaElement>) => void;
  readOnly?: boolean;
  errorMessage?: string;
  className?: string;
}

const Textarea = forwardRef<HTMLTextAreaElement, TextareaProps>(
  (
    {
      id,
      label,
      placeholder,
      value,
      onChange,
      readOnly,
      errorMessage,
      className,
      ...rest
    },
    ref
  ) => {
    return (
      <div className="w-full">
        {label && (
          <label htmlFor={id} className="mb-2 block font-medium">
            {label}
          </label>
        )}
        <textarea
          ref={ref}
          id={id}
          rows={7}
          placeholder={placeholder}
          value={value}
          onChange={onChange}
          readOnly={readOnly}
          className={cx(
            "p-3 w-full border rounded-md outline-gray-300 border-gray-200",
            className,
            {
              "border-gray-200": !errorMessage,
              "border-red-600": errorMessage,
            }
          )}
          {...rest}
        />
        {errorMessage && (
          <p className="text-xs text-red-500 mt-1.5">{errorMessage}</p>
        )}
      </div>
    );
  }
);

Textarea.displayName = "Textarea"; // Cần thiết khi dùng `forwardRef`

export default Textarea;
