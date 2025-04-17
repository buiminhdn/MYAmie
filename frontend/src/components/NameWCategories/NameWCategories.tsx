import CategoryItem from "@/components/CategoryItem/CategoryItem";
import { CategoryVM } from "@/models/viewmodels/category.vm";

interface NameWCategoriesProps {
  name: string;
  shortDescription: string;
  categories: CategoryVM[];
}

function NameWCategories({
  name,
  shortDescription,
  categories,
}: NameWCategoriesProps) {
  return (
    <div className="text-center lg:text-left">
      <p className="text-2xl font-semibold">{name}</p>
      <p className="mt-1 text-gray-500">{shortDescription}</p>
      {!!categories.length && (
        <div className="flex flex-wrap justify-center lg:justify-start gap-2 mt-5">
          {categories.map((category) => (
            <CategoryItem key={category.id} category={category} />
          ))}
        </div>
      )}
    </div>
  );
}

export default NameWCategories;
