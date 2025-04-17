import { useEffect, useState } from "react";
import { useSelector } from "react-redux";
import toast from "react-hot-toast";

import {
  useDeleteFeedback,
  useUpdateFeedback,
} from "@/services/feedback.service";
import { isLoginSelector } from "@/store/auth/auth.selector";
import Button from "../Button/Button";
import RatingModal from "../Modals/RatingModal";
import DeleteConfirmModal from "../Modals/DeleteConfirmModal";

interface ModifyFeedbackItemProps {
  id: number;
  ratingProp: number;
  contentProp: string;
}

function ModifyFeedbackItem({
  id,
  ratingProp,
  contentProp,
}: ModifyFeedbackItemProps) {
  const [activeModal, setActiveModal] = useState<"edit" | "delete" | null>(
    null
  );
  const [rating, setRating] = useState(ratingProp);
  const [comment, setComment] = useState(contentProp);
  const isLogin = useSelector(isLoginSelector);

  useEffect(() => {
    document.body.classList.toggle("overflow-hidden", !!activeModal);
    return () => document.body.classList.remove("overflow-hidden");
  }, [activeModal]);

  const { isPending: isUpdatePending, mutateAsync: updateFeedback } =
    useUpdateFeedback();
  const { isPending: isDeletePending, mutateAsync: deleteFeedback } =
    useDeleteFeedback();

  const openModal = (type: "edit" | "delete") => {
    if (!isLogin) {
      toast.error(
        `Vui lòng đăng nhập để ${type === "edit" ? "sửa" : "xoá"} đánh giá`
      );
      return;
    }
    setActiveModal(type);
  };

  const closeModal = () => {
    setActiveModal(null);
    setRating(ratingProp); // Reset rating to original value when closing
    setComment(contentProp); // Reset comment to original value when closing
  };

  const handleUpdateFeedback = async () => {
    if (!rating) {
      toast.error("Vui lòng đánh giá số sao");
      return;
    }

    try {
      await updateFeedback(
        { id, rating, content: comment },
        { onSuccess: closeModal }
      );
    } catch {
      toast.error("Có lỗi xảy ra, vui lòng thử lại!");
    }
  };

  const handleDeleteFeedback = async () => {
    try {
      await deleteFeedback(id, { onSuccess: closeModal });
    } catch {
      toast.error("Có lỗi xảy ra, vui lòng thử lại!");
    }
  };

  return (
    <>
      <Button
        variant="ghost"
        className="text-xs font-medium hover:cursor-pointer"
        onClick={() => openModal("edit")}
      >
        Chỉnh sửa
      </Button>
      <Button
        variant="ghost"
        className="text-xs font-medium hover:cursor-pointer"
        onClick={() => openModal("delete")}
      >
        Xoá
      </Button>

      {activeModal === "edit" && (
        <RatingModal
          isOpen
          onClose={closeModal}
          rating={rating}
          comment={comment}
          isPending={isUpdatePending}
          onRatingChange={setRating}
          onCommentChange={setComment}
          onSubmit={handleUpdateFeedback}
        />
      )}

      {activeModal === "delete" && (
        <DeleteConfirmModal
          isOpen
          onClose={closeModal}
          handleDelete={handleDeleteFeedback}
          isPending={isDeletePending}
        />
      )}
    </>
  );
}

export default ModifyFeedbackItem;
