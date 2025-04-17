import FeedbackItem from "@/components/FeedbackItems/FeedbackItem";
import Loader from "@/components/Loader/Loader";
import Pagination from "@/components/Pagination/Pagination";
import Rate from "@/components/Rate/Rate";
import { useGetFeedbacks } from "@/services/feedback.service";
import { FilterFeedbackParams } from "@/models/params/feedback.param";
import SendFeedbackForm from "./SendFeedbackForm";
import { useState } from "react";

interface FeedbackTabProps {
  id: number;
}

function FeedbackTab({ id }: FeedbackTabProps) {
  const [params, setParams] = useState<FilterFeedbackParams>({
    pageNumber: 1,
    id,
  });

  const { data, isLoading, isError } = useGetFeedbacks(params);

  const pagination = data?.data?.pagination;
  const feedbacks = data?.data?.feedbacks;
  const totalFeedback = data?.data?.totalFeedback ?? 0;
  const averageRating = data?.data?.averageRating ?? 0;

  return (
    <>
      <div className="p-6 border-2 border-gray-200 rounded-xl bg-white flex flex-wrap gap-4 justify-between">
        <div className="w-full md:w-fit flex flex-wrap justify-between gap-7">
          <div>
            <p className="text-base text-gray-500 font-medium">
              Tổng lượt đánh giá
            </p>
            <p className="text-4xl font-medium mt-1">{totalFeedback}</p>
          </div>
          <div>
            <p className="text-base text-gray-500 font-medium">
              Tỷ lệ đánh giá
            </p>
            <div className="mt-1 flex items-center gap-2">
              <p className="text-4xl font-medium">{averageRating}</p>
              <div>
                <Rate rate={averageRating} />
                <p className="text-xs font-medium text-gray-500 mt-1">
                  Trên tổng lượt đánh giá
                </p>
              </div>
            </div>
          </div>
        </div>
        <div className="h-fit">
          <SendFeedbackForm id={id} />
        </div>
      </div>

      <div className="py-6 md:p-6 space-y-10">
        {isLoading ? (
          <Loader />
        ) : isError ? (
          <p className="error">Lỗi, vui lòng thử lại</p>
        ) : feedbacks && feedbacks.length > 0 ? (
          feedbacks.map((feedback) => (
            <FeedbackItem key={feedback.id} feedback={feedback} />
          ))
        ) : (
          <p>Chưa có đánh giá nào</p>
        )}
      </div>

      <Pagination
        currentPage={pagination?.currentPage ?? 1}
        totalPage={pagination?.totalPages ?? 1}
        onPageChange={(newPage) =>
          setParams((prev) => ({ ...prev, pageNumber: newPage }))
        }
      />
    </>
  );
}

export default FeedbackTab;
