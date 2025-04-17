import { CategoryVM } from "@/models/viewmodels/category.vm";
import cx from "classnames";

interface CategoryItemProps {
  category: CategoryVM;
  onClick?: () => void;
  isActive?: boolean;
}

function CategoryItem({
  category,
  onClick = () => {},
  isActive = false,
}: CategoryItemProps) {
  return (
    <div
      onClick={onClick}
      className={cx(
        "flex items-center gap-2 border-2 border-primary rounded-full py-1 px-3 w-fit hover:bg-primary-light hover:cursor-pointer transition-colors",
        { "bg-primary-light": isActive }
      )}
    >
      <input type="hidden" value={category.id} />
      <i className={cx("fa-sm fa-regular text-primary", category.icon)}></i>
      <p>{category.name}</p>
    </div>
  );
}

export default CategoryItem;
