import Textarea from "@/components/Input/Textarea";
import clsx from "clsx";
import Button from "../Button/Button";
import ConfirmModal from "./ConfirmModal";

interface RatingModalProps {
  isOpen: boolean;
  onClose: () => void;
  rating: number;
  comment: string;
  isPending: boolean;
  onRatingChange: (rating: number) => void;
  onCommentChange: (comment: string) => void;
  onSubmit: () => void;
}

const RatingModal = ({
  isOpen,
  onClose,
  rating,
  comment,
  isPending,
  onRatingChange,
  onCommentChange,
  onSubmit,
}: RatingModalProps) => {
  if (!isOpen) return null;

  return (
    <ConfirmModal
      isOpen={isOpen}
      onClose={onClose}
      className="w-11/12 sm:w-1/2 xl:w-1/3"
    >
      <form
        onSubmit={(e) => {
          e.preventDefault();
          onSubmit();
        }}
      >
        <div className="flex text-2xl gap-1 text-yellow-300 mb-5 justify-center">
          {[1, 2, 3, 4, 5].map((star) => (
            <i
              key={star}
              className={clsx("fa-star cursor-pointer", {
                "fa-solid": star <= rating,
                "fa-light": star > rating,
              })}
              onClick={() => onRatingChange(star)}
              title={`Đánh giá ${star} sao`}
              aria-label={`Đánh giá ${star} sao`}
            />
          ))}
        </div>
        <Textarea
          value={comment}
          onChange={(e) => onCommentChange(e.target.value)}
          placeholder="Nhập đánh giá của bạn"
          label="Bình luận"
        />
        <div className="flex gap-3 mt-5">
          <Button
            type="button"
            className="w-full hover:cursor-pointer"
            variant="ghost"
            onClick={onClose}
          >
            Trở lại
          </Button>
          <Button
            type="submit"
            className="w-full hover:cursor-pointer"
            disabled={isPending}
          >
            {isPending ? "Đang gửi..." : "Gửi đánh giá"}
          </Button>
        </div>
      </form>
    </ConfirmModal>
  );
};

export default RatingModal;
