import cx from "classnames";
import { useState } from "react";

interface FilterSelectsProps<T extends Record<string, string | number>> {
  label: string;
  options: T;
  onFilterChange: (selectedValue: T[keyof T]) => void;
}

function FilterSelects<T extends Record<string, string | number>>({
  label,
  options,
  onFilterChange,
}: FilterSelectsProps<T>) {
  const optionValues = Object.values(options).filter(
    (v): v is number => typeof v === "number"
  );
  const [selected, setSelected] = useState<number>(optionValues[0] || 0);

  const handleSelect = (option: number) => {
    if (option !== selected) {
      setSelected(option);
      onFilterChange(option as T[keyof T]);
    }
  };

  return (
    <div className="flex gap-4 text-xs items-center">
      <p className="font-medium flex-none">{label}:</p>
      <div className="flex flex-wrap gap-3">
        {optionValues.map((option) => (
          <button
            key={option}
            className={cx(
              "px-2 py-1 border-2 rounded-full transition cursor-pointer",
              selected === option ? "border-primary" : "border-gray-300"
            )}
            onClick={() => handleSelect(option)}
          >
            {options[option]} {/* Convert numeric value to enum name */}
          </button>
        ))}
      </div>
    </div>
  );
}

export default FilterSelects;
