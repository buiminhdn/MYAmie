import { remove as removeDiacritics } from "diacritics";
import { useEffect, useMemo, useState, useCallback } from "react";
import Input from "../Input/Input";
import Loader from "../Loader/Loader";
import { useGetAllCities } from "@/services/city.service";
import { CityVM } from "@/models/viewmodels/city.vm";
import useDebounce from "@/hooks/useDebounce";
import IconBtn from "../Button/IconBtn";

const normalizeString = (str: string) => removeDiacritics(str.toLowerCase());

interface FilterByCitiesProps {
  onCitySelect: (cityId: number) => void;
  onClear?: () => void;
  currentCityId?: number | null;
}

function FilterByCities({
  onCitySelect,
  onClear,
  currentCityId,
}: FilterByCitiesProps) {
  const { data, isLoading, isError } = useGetAllCities();
  const [term, setTerm] = useState("");
  const [selectedCity, setSelectedCity] = useState<CityVM | null>(null);

  // Debounced search term
  const debouncedTerm = useDebounce(term, 300);
  const cities = data?.data;

  useEffect(() => {
    if (currentCityId && cities) {
      const foundCity = cities.find((city) => city.id === currentCityId);
      if (foundCity) {
        setSelectedCity(foundCity);
        setTerm(foundCity.name);
      }
    }
  }, [currentCityId, cities]);

  // Memoized event handlers
  const handleInputChange = useCallback(
    (e: React.ChangeEvent<HTMLInputElement>) => {
      setTerm(e.target.value);
      if (selectedCity) setSelectedCity(null);
    },
    [selectedCity]
  );

  const handleCitySelect = useCallback(
    (city: CityVM) => {
      setSelectedCity(city);
      setTerm(city.name);
      onCitySelect(city.id);
    },
    [onCitySelect]
  );

  const clearSelectedCity = useCallback(() => {
    setSelectedCity(null);
    setTerm("");
    onClear?.();
  }, [onClear]);

  // Filter city data
  const filteredCities = useMemo(() => {
    return cities?.filter((city) =>
      normalizeString(city.name).includes(normalizeString(debouncedTerm))
    );
  }, [cities, debouncedTerm]);

  let content;
  if (isLoading) {
    content = (
      <div className="mt-2" aria-live="polite">
        <Loader />
      </div>
    );
  } else if (isError) {
    content = (
      <p className="error" aria-live="assertive">
        Lỗi, vui lòng thử lại
      </p>
    );
  } else {
    content = (
      <ul
        className="text-gray-600 h-40 overflow-y-auto mt-2"
        aria-label="Danh sách thành phố"
        role="listbox"
      >
        {filteredCities?.map((city) => (
          <li
            key={city.id}
            onClick={() => handleCitySelect(city)}
            className="py-1.5 pl-3 mr-2 hover:bg-primary-lighter rounded-md cursor-pointer"
            role="option"
            aria-selected={selectedCity?.id === city.id}
          >
            {city.name}
          </li>
        ))}
      </ul>
    );
  }

  return (
    <div>
      <div className="relative">
        <Input
          placeholder="Chọn thành phố"
          label="Thành phố"
          value={term || undefined} // Ensure controlled/uncontrolled consistency
          onChange={handleInputChange}
          readOnly={!!selectedCity}
        />
        {selectedCity && (
          <IconBtn
            id="clear-city-filter"
            hasBackground={false}
            className="absolute right-3 bottom-2 hover:cursor-pointer"
            onClick={clearSelectedCity}
            icon="fa-circle-xmark"
          />
        )}
      </div>
      {!selectedCity && content}
    </div>
  );
}

export default FilterByCities;
