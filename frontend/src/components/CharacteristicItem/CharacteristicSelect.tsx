import CharacteristicItem from "@/components/CharacteristicItem/CharacteristicItem";
import { useState } from "react";
import IconBtn from "../Button/IconBtn";

interface CharacteristicSelectProps {
  currentCharacteristics?: string[];
  onChange: (characteristics: string[]) => void;
  errorMessage?: string;
}

function CharacteristicSelect({
  currentCharacteristics = [],
  onChange,
  errorMessage,
}: CharacteristicSelectProps) {
  const [characteristics, setCharacteristics] = useState(
    currentCharacteristics
  );
  const [inputValue, setInputValue] = useState("");

  const handleAddCharacteristic = () => {
    const trimmedValue = inputValue.trim();
    if (
      trimmedValue &&
      trimmedValue.length <= 20 &&
      !characteristics.includes(trimmedValue) &&
      characteristics.length < 5
    ) {
      setCharacteristics((prev) => {
        const updated = [...prev, trimmedValue];
        onChange(updated);
        return updated;
      });
      setInputValue("");
    }
  };

  const handleRemoveAll = () => {
    setCharacteristics([]);
    onChange([]);
  };

  return (
    <div>
      <label className="mb-2 block font-medium">Tính cách (5)</label>
      <div className="border border-gray-300 rounded-full p-1 w-1/2 flex">
        <input
          type="text"
          className="w-full outline-none px-3"
          placeholder="Nhập tính cách"
          value={inputValue}
          onChange={(e) => setInputValue(e.target.value.slice(0, 20))}
          readOnly={characteristics.length >= 5}
        />
        <IconBtn
          icon="fa-plus"
          className="hover:cursor-pointer"
          onClick={handleAddCharacteristic}
        />
      </div>
      {characteristics.length > 0 && (
        <div className="flex flex-wrap gap-2 mt-3">
          {characteristics.map((characteristic, index) => (
            <CharacteristicItem key={index} text={characteristic} />
          ))}
          <IconBtn
            icon="fa-trash"
            className="hover:cursor-pointer hover:text-red-500"
            onClick={handleRemoveAll}
          />
        </div>
      )}
      {errorMessage && (
        <p className="text-xs text-red-500 mt-2">{errorMessage}</p>
      )}
    </div>
  );
}

export default CharacteristicSelect;
