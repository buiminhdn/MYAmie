import { useState } from "react";
import FilterBtn from "../Button/FilterBtn";

interface SearchBarProps {
  children: React.ReactNode;
  onSearch: (searchTerm: string) => void;
  onActiveClick?: () => void;
  onInactiveClick?: () => void;
}

function SearchBar({
  onSearch,
  children,
  onInactiveClick,
  onActiveClick,
}: SearchBarProps) {
  const [searchTerm, setSearchTerm] = useState("");

  const handleSearchClick = () => {
    onSearch(searchTerm.trim());
  };

  const handleKeyDown = (e: React.KeyboardEvent<HTMLInputElement>) => {
    if (e.key === "Enter") {
      handleSearchClick();
    }
  };

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const value = e.target.value;
    setSearchTerm(value);
    if (value.trim() === "") onSearch("");
  };

  return (
    <div className="border border-gray-500 rounded-md p-2.5 flex items-center gap-2 md:w-1/2 bg-white mx-auto mt-8">
      <button
        onClick={handleSearchClick}
        className="px-2 text-gray-400 hover:text-gray-700 hover:cursor-pointer"
        aria-label="Search"
      >
        <i className="fa-xl fa-regular fa-magnifying-glass"></i>
      </button>
      <input
        type="text"
        placeholder="Nhập từ khoá tìm kiếm tại đây"
        value={searchTerm}
        onChange={handleInputChange}
        onKeyDown={handleKeyDown}
        className="w-full outline-none"
      />
      <FilterBtn
        onActiveClick={onActiveClick}
        onInactiveClick={onInactiveClick}
      >
        {children}
      </FilterBtn>
    </div>
  );
}

export default SearchBar;
