import { PaginationParams } from "./app.param";

export enum FeedbackTargetType {
  BUSINESS = 1,
  PLACE = 2,
  USER = 3,
}

export interface AddFeedbackParams {
  targetId: number;
  targetType: FeedbackTargetType;
  rating: number;
  content: string;
}

export interface DeleteFeedbackParams {
  id: number;
}

export interface FilterFeedbackParams extends PaginationParams {
  id: number;
  isResponded?: boolean;
  rate?: number;
  pageNumber?: number;
  pageSize?: number;
}

export interface ResponseFeedbackParams {
  id: number;
  message: string;
}

export interface UpdateFeedbackParams {
  id: number;
  rating: number;
  content: string;
}
