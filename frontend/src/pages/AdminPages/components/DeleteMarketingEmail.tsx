import { useState, useEffect } from "react";
import toast from "react-hot-toast";
import DeleteConfirmModal from "@/components/Modals/DeleteConfirmModal";
import { useDeleteMarketingEmail } from "@/services/email.service";

interface DeleteMarketingEmailProps {
  id: number;
}

function DeleteMarketingEmail({ id }: DeleteMarketingEmailProps) {
  const [isModalOpen, setIsModalOpen] = useState(false);

  const { isPending: isDeletePending, mutateAsync: deleteMarketingEmail } =
    useDeleteMarketingEmail();

  useEffect(() => {
    document.body.classList.toggle("overflow-hidden", isModalOpen);
    return () => document.body.classList.remove("overflow-hidden");
  }, [isModalOpen]);

  const openModal = () => setIsModalOpen(true);
  const closeModal = () => setIsModalOpen(false);

  const handleDelete = async () => {
    try {
      await deleteMarketingEmail(id, {
        onSuccess: () => {
          closeModal();
        },
      });
    } catch {
      toast.error("Có lỗi xảy ra, vui lòng thử lại!");
    }
  };

  return (
    <>
      <button
        onClick={openModal}
        className="text-xs w-fit hover:underline hover:text-red-700 text-red-500 font-medium hover:cursor-pointer"
      >
        Xoá
      </button>
      {isModalOpen && (
        <DeleteConfirmModal
          isOpen
          onClose={closeModal}
          handleDelete={handleDelete}
          isPending={isDeletePending}
        />
      )}
    </>
  );
}

export default DeleteMarketingEmail;
