import { useMemo } from "react";
import Select, { SingleValue, MultiValue } from "react-select";
import { Control, useController } from "react-hook-form";

interface OptionType {
  name: string;
  id: number;
}

interface DropdownProps<T extends OptionType> {
  label?: string;
  options?: T[];
  isClearable?: boolean;
  isMulti?: boolean;
  maxSelectItems?: number;
  placeHolder?: string;
  className?: string;
  height?: string;
  isLoading?: boolean;
  isError?: boolean;
  control: Control<any>;
  name: string;
  value?: number | number[];
}

const DEFAULT_HEIGHT = "2.95rem";

const getCustomStyles = (
  height: string = DEFAULT_HEIGHT,
  hasError: boolean
) => ({
  control: (base: any, state: { isFocused: boolean }) => ({
    ...base,
    height,
    borderColor: hasError ? "#EF4444" : state.isFocused ? "#d1d6dc" : "#E5E7EB",
    borderRadius: "0.375rem",
    boxShadow: state.isFocused && !hasError ? "0 0 0 1px #d1d6dc" : "none",
    ":hover": {
      cursor: "pointer",
      borderColor: hasError ? "#EF4444" : "#D1D5DB",
    },
  }),
  menu: (base: any) => ({
    ...base,
    paddingLeft: "0.25rem",
    marginTop: "0.3rem",
    backgroundColor: "white",
    borderRadius: "0.375rem",
    boxShadow: "0 4px 8px rgba(0, 0, 0, 0.1)",
    zIndex: 9999,
  }),
  option: (base: any, state: { isFocused: boolean; isSelected: boolean }) => ({
    ...base,
    padding: "0.5rem",
    borderRadius: "0.375rem",
    backgroundColor: state.isSelected
      ? "#3f6189"
      : state.isFocused
      ? "#ebf4fb"
      : "white",
    color: state.isSelected ? "white" : "#4B5563",
    cursor: "pointer",
    ":active": {
      backgroundColor: "#d5e9f9",
    },
  }),
  multiValue: (base: any) => ({
    ...base,
    marginRight: "0.25rem",
    paddingLeft: "0.2rem",
    backgroundColor: "#EBF4FB",
    borderRadius: "0.375rem",
  }),
  multiValueLabel: (base: any) => ({
    ...base,
    color: "#3F6189",
    fontWeight: "500",
  }),
  multiValueRemove: (base: any) => ({
    ...base,
    borderRadius: "0.375rem",
    color: "#2C5282",
    ":hover": {
      backgroundColor: "#E2E8F0",
      color: "#2C5282",
    },
  }),
  menuPortal: (base: any) => ({ ...base, zIndex: 9999 }),
  indicatorSeparator: (base: any) => ({
    ...base,
    backgroundColor: hasError ? "#EF4444" : "#E5E7EB",
  }),
  dropdownIndicator: (base: any) => ({
    ...base,
    color: hasError ? "#EF4444" : "#9CA3AF",
    ":hover": {
      color: hasError ? "#DC2626" : "#6B7280",
    },
  }),
});

function Dropdown<T extends OptionType>({
  label,
  options = [],
  isClearable = false,
  isMulti = false,
  maxSelectItems = 1,
  placeHolder = "Chọn một hoặc nhiều mục",
  className = "w-full",
  height = DEFAULT_HEIGHT,
  isLoading = false,
  isError = false,
  control,
  name,
  value,
}: DropdownProps<T>) {
  const { field, fieldState } = useController({
    control,
    name,
    defaultValue: isMulti ? value || [] : value || undefined,
  });

  const hasError = !!fieldState.error;

  const selectOptions = useMemo(
    () =>
      options.map((option) => ({
        label: option.name,
        value: option.id,
      })),
    [options]
  );

  const customStyles = useMemo(
    () => getCustomStyles(height, hasError),
    [height, hasError]
  );

  const currentValue = useMemo(() => {
    if (isMulti) {
      return selectOptions.filter((option) =>
        (field.value as number[])?.includes(option.value)
      );
    }
    return selectOptions.find((option) => option.value === field.value);
  }, [field.value, isMulti, selectOptions]);

  const currentPlaceholder = useMemo(
    () =>
      isLoading ? "Đang tải..." : isError ? "Lỗi khi tải dữ liệu" : placeHolder,
    [isLoading, isError, placeHolder]
  );

  const handleChange = (
    selected:
      | MultiValue<{ label: string; value: number }>
      | SingleValue<{ label: string; value: number }>
  ) => {
    if (isMulti) {
      const selectedValues = (
        selected as Array<{ label: string; value: number }>
      ).map((item) => item.value);
      field.onChange(selectedValues);
    } else {
      const selectedValue = (selected as { label: string; value: number })
        ?.value;
      field.onChange(selectedValue ?? undefined);
    }
  };

  return (
    <div className={className}>
      {label && (
        <label className="mb-2 block font-medium" htmlFor={`dropdown-${name}`}>
          {label}
        </label>
      )}
      <Select
        id={`dropdown-${name}`}
        ref={field.ref}
        onBlur={field.onBlur}
        options={selectOptions}
        placeholder={currentPlaceholder}
        styles={customStyles}
        isClearable={isClearable}
        isMulti={isMulti}
        maxMenuHeight={240}
        minMenuHeight={50}
        value={currentValue}
        menuPortalTarget={document.body}
        isDisabled={isLoading || isError}
        isOptionDisabled={() =>
          isMulti && maxSelectItems
            ? Array.isArray(field.value) && field.value.length >= maxSelectItems
            : false
        }
        onChange={handleChange}
        classNamePrefix="react-select"
      />
      {fieldState.error && (
        <p className="text-xs text-red-500 mt-1.5 inline-block">
          {fieldState.error.message}
        </p>
      )}
    </div>
  );
}

export default Dropdown;
