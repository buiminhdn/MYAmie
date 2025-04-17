import { useState } from "react";
import CategoryItem from "./CategoryItem";
import { useGetAllCategories } from "@/services/category.service";
import Loader from "../Loader/Loader";

interface CategoryContainerProps {
  onSelect: (categoryId: number) => void;
}

function CategoryContainer({ onSelect }: CategoryContainerProps) {
  const { data, isLoading, isError } = useGetAllCategories();
  const [selectedCategoryId, setSelectedCategoryId] = useState(0);

  const handleCategoryClick = (categoryId: number) => {
    setSelectedCategoryId(categoryId);
    onSelect(categoryId);
  };

  return (
    <div className="flex justify-center items-center gap-3 mt-7 flex-wrap md:w-4/5 mx-auto">
      <CategoryItem
        category={{ id: 0, name: "Tất cả", icon: "fa-icons" }}
        isActive={selectedCategoryId === 0}
        onClick={() => handleCategoryClick(0)}
      />

      {isLoading ? (
        <Loader />
      ) : isError ? (
        <p className="error">Lỗi, vui lòng thử lại</p>
      ) : (
        data?.data?.map((category) => (
          <CategoryItem
            key={category.id}
            category={category}
            onClick={() => handleCategoryClick(category.id)}
            isActive={category.id === selectedCategoryId}
          />
        ))
      )}
    </div>
  );
}

export default CategoryContainer;
