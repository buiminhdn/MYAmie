import ReactModal from "react-modal";
import cx from "classnames";
import Button from "../Button/Button";
import IconBtn from "../Button/IconBtn";
import "./modal.css";

interface CustomModalProps {
  title: string;
  isOpen: boolean;
  onClose: () => void;
  onActiveClick?: () => void;
  onInactiveClick?: () => void;
  children: React.ReactNode;
  saveButtonTitle?: string;
  cancelButtonTitle?: string;
  className?: string;
  isPending?: boolean;
}

function CustomModal({
  title,
  isOpen,
  onClose,
  children,
  saveButtonTitle = "Lưu",
  cancelButtonTitle = "Huỷ bỏ",
  className = "w-1/2",
  onActiveClick,
  onInactiveClick = onClose, // Default to onClose if not provided
  isPending = false,
}: CustomModalProps) {
  return (
    <ReactModal
      isOpen={isOpen}
      onRequestClose={onClose}
      className={cx("filter_modal", className)}
      overlayClassName="filter_modal_overlay"
      aria={{
        labelledby: "modal-title",
        describedby: "modal-content",
      }}
    >
      {/* Header */}
      <div className="p-3.5 relative border-b border-gray-300">
        <p id="modal-title" className="text-center font-medium text-base">
          {title}
        </p>
        <IconBtn
          onClick={onClose}
          className="absolute top-2.5 right-2.5 hover:cursor-pointer"
          icon="fa-xmark"
        />
      </div>

      {/* Body */}
      <div
        id="modal-content"
        className="p-5 overflow-y-auto h-full flex flex-col gap-3"
      >
        {children}
      </div>

      {/* Footer */}
      <div className="p-3 flex justify-between border-t border-gray-300">
        <Button
          variant="ghost"
          className="hover:cursor-pointer"
          onClick={onInactiveClick}
          padding="py-1.5 px-2.5"
        >
          {cancelButtonTitle}
        </Button>
        <Button
          className="hover:cursor-pointer"
          id="filter-button-save"
          disabled={isPending}
          onClick={onActiveClick}
          padding="py-1.5 px-2.5"
        >
          {isPending ? `Đang ${saveButtonTitle}...` : saveButtonTitle}
        </Button>
      </div>
    </ReactModal>
  );
}

export default CustomModal;
