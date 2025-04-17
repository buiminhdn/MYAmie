import { PaginationParams } from "./app.param";

export enum VerificationTypeParam {
  AccountConfirmation = 1,
  PasswordReset = 2,
}

export interface RequestVerifyParams {
  email: string;
  type: VerificationTypeParam;
}

export interface ResetPasswordParams {
  email: string;
  code: string;
  newPassword: string;
}

export interface VerifyEmailParams {
  email: string;
  code: string;
}

export interface EmailMarketingParams extends PaginationParams {}

export interface AddMarketingEmailParams {
  subject: string;
  body: string;
}
