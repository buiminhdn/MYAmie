import { PaginationData } from "../app.interface";

export interface FeedbackVM {
  id: number;
  avatar: string;
  name: string;
  createdDate: string;
  rating: number;
  content: string;
  response?: string;
  senderId: number;
  ownerId: number;
}

export interface FeedbackInfoVM {
  feedbacks: FeedbackVM[];
  averageRating: number;
  totalFeedback: number;
}

export interface PagedFeedbacksVM {
  feedbacks: FeedbackVM[];
  averageRating: number;
  totalFeedback: number;
  pagination: PaginationData;
}
