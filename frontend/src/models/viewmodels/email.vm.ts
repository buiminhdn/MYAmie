import { PaginationData } from "../app.interface";

export interface EmailMarketingVM {
  id: number;
  subject: string;
  body: string;
  status: string;
}

export interface PagedEmailMarketingVM {
  emails: EmailMarketingVM[];
  pagination: PaginationData;
}
