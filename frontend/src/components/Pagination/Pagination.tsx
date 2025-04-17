import Button from "../Button/Button";

interface PaginationProps {
  currentPage: number;
  totalPage?: number;
  onPageChange: (page: number) => void;
  className?: string;
}

function Pagination({
  currentPage,
  totalPage = 1,
  onPageChange,
  className = "mt-20",
}: PaginationProps) {
  const totalPages = Math.max(totalPage, 1); // Ensure totalPages is always >= 1

  const isPrevDisabled = currentPage <= 1;
  const isNextDisabled = currentPage >= totalPages;

  const handlePreviousPage = () => {
    if (!isPrevDisabled) {
      onPageChange(currentPage - 1);
    }
  };

  const handleNextPage = () => {
    if (!isNextDisabled) {
      onPageChange(currentPage + 1);
    }
  };

  return (
    <div className={`flex items-center gap-4 w-fit mx-auto ${className}`}>
      <Button
        id="pagination-previous"
        variant="ghost"
        padding="px-3 py-1.5"
        onClick={handlePreviousPage}
        disabled={isPrevDisabled}
        className={!isPrevDisabled ? "hover:cursor-pointer" : ""}
        aria-label="Trang trước"
      >
        <i className="fa-solid fa-chevron-left"></i>
      </Button>

      <p>
        {currentPage} / {totalPages}
      </p>

      <Button
        id="pagination-next"
        variant="ghost"
        padding="px-3 py-1.5"
        onClick={handleNextPage}
        disabled={isNextDisabled}
        className={!isNextDisabled ? "hover:cursor-pointer" : ""}
        aria-label="Trang sau"
      >
        <i className="fa-solid fa-chevron-right"></i>
      </Button>
    </div>
  );
}

export default Pagination;
