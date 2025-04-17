import { ChangeEvent } from "react";

interface FilterByDistanceProps {
  currentDistance: number;
  onChange: (distance: number) => void;
}

function FilterByDistance({
  currentDistance,
  onChange,
}: FilterByDistanceProps) {
  const handleDistanceChange = (event: ChangeEvent<HTMLInputElement>) => {
    onChange(Number(event.target.value)); // Notify parent immediately
  };

  return (
    <div className="w-full">
      <p className="font-medium mb-2">Khoảng cách</p>
      <input
        type="range"
        value={currentDistance}
        onChange={handleDistanceChange}
        step={5}
        min={5}
        max={15}
        className="slider appearance-none bg-gray-200 w-full h-2 cursor-pointer"
      />
      <div className="flex justify-between mt-2 text-sm">
        <p>5km</p>
        <p>10km</p>
        <p>15km</p>
      </div>
    </div>
  );
}

export default FilterByDistance;
