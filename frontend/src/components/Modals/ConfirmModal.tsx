import ReactModal from "react-modal";
import clsx from "clsx";
import "./modal.css";

interface ConfirmModalProps {
  isOpen: boolean;
  onClose: () => void;
  children: React.ReactNode;
  className?: string;
}

function ConfirmModal({
  isOpen,
  onClose,
  children,
  className,
}: ConfirmModalProps) {
  return (
    <ReactModal
      isOpen={isOpen}
      onRequestClose={onClose}
      className={clsx("confirm_modal", className)}
      overlayClassName="filter_modal_overlay"
      ariaHideApp={false} // Prevent accessibility issues
      shouldCloseOnOverlayClick={true} // Allow closing on clicking outside
      shouldCloseOnEsc={true} // Allow closing with Escape key
    >
      <div className="p-8 relative overflow-y-auto">
        <button
          onClick={onClose}
          className="absolute top-2 right-2 size-6 bg-gray-200 rounded-full flex items-center justify-center hover:cursor-pointer"
          aria-label="Đóng modal"
        >
          <i className="fa fa-times"></i>
        </button>
        {children}
      </div>
    </ReactModal>
  );
}

export default ConfirmModal;
