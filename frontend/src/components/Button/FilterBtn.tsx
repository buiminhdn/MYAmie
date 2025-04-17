import { useEffect, useState, useRef } from "react";
import Button from "./Button";
import CustomModal from "../Modals/CustomModal";

interface FilterBtnProps {
  onActiveClick?: () => void;
  onInactiveClick?: () => void;
  children: React.ReactNode;
}

function FilterBtn({
  children,
  onActiveClick,
  onInactiveClick,
}: FilterBtnProps) {
  const [modalIsOpen, setModalIsOpen] = useState(false);
  const buttonRef = useRef<HTMLButtonElement>(null);

  const openModal = () => setModalIsOpen(true);
  const closeModal = () => {
    setModalIsOpen(false);
    // Return focus to the button when modal closes
    setTimeout(() => buttonRef.current?.focus(), 0);
  };

  const handleActiveClick = () => {
    onActiveClick?.();
    closeModal();
  };

  const handleInactiveClick = () => {
    onInactiveClick?.();
    closeModal();
  };

  useEffect(() => {
    if (modalIsOpen) {
      document.documentElement.style.overflow = "hidden";
    } else {
      document.documentElement.style.overflow = "auto";
    }

    return () => {
      document.documentElement.style.overflow = "auto";
    };
  }, [modalIsOpen]);

  return (
    <>
      <Button
        id="filter-button"
        variant="ghost"
        className="flex items-center gap-2 hover:cursor-pointer"
        onClick={openModal}
        aria-label="Open filter modal"
        aria-haspopup="dialog" // Indicates this button opens a dialog
      >
        <i className="fa-solid fa-bars-filter"></i>
        <p>Bộ lọc</p>
      </Button>
      <CustomModal
        isOpen={modalIsOpen}
        onClose={closeModal}
        title="Tất cả bộ lọc"
        saveButtonTitle="Xem kết quả"
        cancelButtonTitle="Xoá bộ lọc"
        className="w-11/12 md:w-1/2"
        onActiveClick={handleActiveClick}
        onInactiveClick={handleInactiveClick}
      >
        {children}
      </CustomModal>
    </>
  );
}

export default FilterBtn;
