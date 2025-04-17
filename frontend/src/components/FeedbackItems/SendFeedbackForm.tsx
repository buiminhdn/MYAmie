import { useState, useLayoutEffect } from "react";
import { useSelector } from "react-redux";
import toast from "react-hot-toast";

import { useAddFeedback } from "@/services/feedback.service";
import {
  accountRoleSelector,
  isLoginSelector,
} from "@/store/auth/auth.selector";
import { FeedbackTargetType } from "@/models/params/feedback.param";
import { Role } from "@/models/app.interface";
import Button from "../Button/Button";
import RatingModal from "../Modals/RatingModal";

interface SendFeedbackFormProps {
  id: number;
}

function SendFeedbackForm({ id }: SendFeedbackFormProps) {
  const [isOpen, setIsOpen] = useState(false);
  const [rating, setRating] = useState(0);
  const [comment, setComment] = useState("");

  const isLogin = useSelector(isLoginSelector);
  const role = useSelector(accountRoleSelector);
  const { isPending, mutateAsync } = useAddFeedback();

  useLayoutEffect(() => {
    document.body.style.overflow = isOpen ? "hidden" : "auto";
    return () => {
      document.body.style.overflow = "auto";
    };
  }, [isOpen]);

  const openModal = () => {
    if (!isLogin) {
      toast.error("Vui lòng đăng nhập để viết đánh giá");
      return;
    }
    setIsOpen(true);
  };

  const closeModal = () => {
    setIsOpen(false);
    setRating(0);
    setComment("");
  };

  const handleSendFeedback = async () => {
    if (!rating) {
      toast.error("Vui lòng đánh giá số sao");
      return;
    }

    if (!comment) {
      toast.error("Vui lòng nhập bình luận");
      return;
    }

    try {
      await mutateAsync({
        targetId: id,
        targetType: FeedbackTargetType.BUSINESS,
        rating,
        content: comment,
      });
      closeModal();
    } catch {}
  };

  return (
    <>
      {role === Role.USER && (
        <Button
          variant="ghost"
          className="text-xs font-medium hover:cursor-pointer"
          onClick={openModal}
        >
          Viết đánh giá
        </Button>
      )}
      <RatingModal
        isOpen={isOpen}
        onClose={closeModal}
        rating={rating}
        comment={comment}
        isPending={isPending}
        onRatingChange={setRating}
        onCommentChange={setComment}
        onSubmit={handleSendFeedback}
      />
    </>
  );
}

export default SendFeedbackForm;
