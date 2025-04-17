import ConfirmModal from "@/components/Modals/ConfirmModal";
import clsx from "clsx";
import Button from "../Button/Button";

interface DeleteConfirmModalProps {
  isOpen: boolean;
  onClose: () => void;
  handleDelete: () => void;
  isPending: boolean;
}

function DeleteConfirmModal({
  isOpen,
  onClose,
  handleDelete,
  isPending,
}: DeleteConfirmModalProps) {
  if (!isOpen) return null;

  return (
    <ConfirmModal
      isOpen={isOpen}
      onClose={onClose}
      className="w-11/12 sm:w-1/2 xl:w-1/3"
    >
      <div className="flex gap-5">
        <div className="flex items-center justify-center bg-gray-100 w-12 p-2 rounded-md">
          <i className="text-red-500 fa-xl fa-regular fa-circle-xmark"></i>
        </div>
        <div>
          <p className="text-lg font-semibold">Xác nhận xoá</p>
          <p className="text-gray-500">Hành động này không thể hoàn tác.</p>
        </div>
      </div>
      <div className="flex gap-3 mt-7">
        <Button
          className="w-full hover:cursor-pointer"
          variant="ghost"
          onClick={onClose}
          disabled={isPending}
        >
          Trở lại
        </Button>
        <Button
          className={clsx("w-full hover:cursor-pointer", {
            "opacity-70 cursor-not-allowed": isPending,
          })}
          onClick={handleDelete}
          disabled={isPending}
          aria-live="polite"
        >
          {isPending ? "Đang xoá..." : "Đồng ý"}
        </Button>
      </div>
    </ConfirmModal>
  );
}

export default DeleteConfirmModal;
