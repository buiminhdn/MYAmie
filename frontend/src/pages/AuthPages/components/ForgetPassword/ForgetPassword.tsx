import { useEffect, useState, useCallback } from "react";
import toast from "react-hot-toast";
import Input from "@/components/Input/Input";
import ConfirmModal from "@/components/Modals/ConfirmModal";
import { useResetPassword } from "@/services/account.service";
import { useRequestVerification } from "@/services/email.service";
import Button from "@/components/Button/Button";
import { VerificationTypeParam } from "@/models/params/email.param";

const EMAIL_REGEX = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
const RESEND_DELAY = 60;
const PASSWORD_MIN_LENGTH = 6;

function ForgetPassword() {
  const [isOpen, setIsOpen] = useState(false);
  const [email, setEmail] = useState("");
  const [code, setCode] = useState("");
  const [newPassword, setNewPassword] = useState("");
  const [timeLeft, setTimeLeft] = useState(0);

  const { mutateAsync: requestEmailReset, isPending: isRequesting } =
    useRequestVerification();
  const { mutateAsync: resetPassword, isPending: isResetting } =
    useResetPassword();

  const closeModal = () => setIsOpen(false);
  const openModal = () => setIsOpen(true);

  // Validate email format
  const isEmailValid = useCallback(
    (email: string) => EMAIL_REGEX.test(email),
    []
  );

  // Handle email reset request
  const handleSendCode = async () => {
    if (!email) return toast.error("Vui lòng nhập email");
    if (!isEmailValid(email)) return toast.error("Vui lòng nhập email hợp lệ");

    try {
      await requestEmailReset({
        email: email,
        type: VerificationTypeParam.PasswordReset,
      });
      setTimeLeft(RESEND_DELAY);
    } catch (error) {
      toast.error("Gửi mã thất bại, vui lòng thử lại");
    }
  };

  // Countdown timer for resend button
  useEffect(() => {
    if (timeLeft > 0) {
      const timerId = setTimeout(() => setTimeLeft((prev) => prev - 1), 1000);
      return () => clearTimeout(timerId);
    }
  }, [timeLeft]);

  // Handle password reset
  const handleResetPassword = async () => {
    if (!email || !code || !newPassword) {
      return toast.error("Vui lòng nhập email, mã xác minh và mật khẩu");
    }
    if (newPassword.length < PASSWORD_MIN_LENGTH) {
      return toast.error(`Mật khẩu phải dài hơn ${PASSWORD_MIN_LENGTH} ký tự`);
    }

    try {
      await resetPassword({ email, code, newPassword });
      closeModal();
    } catch (error) {}
  };

  return (
    <>
      <p
        className="text-xs font-medium text-gray-700 hover:underline hover:cursor-pointer"
        onClick={openModal}
      >
        Quên mật khẩu
      </p>

      <ConfirmModal
        isOpen={isOpen}
        onClose={closeModal}
        className="w-11/12 sm:w-1/2 xl:w-1/3"
      >
        <p className="text-xl font-semibold text-center">THAY ĐỔI MẬT KHẨU</p>
        <p className="text-gray-700 text-center mt-2">
          Vui lòng xác nhận email để cập nhật mật khẩu mới
        </p>

        <Input
          placeholder="Nhập email tại đây..."
          type="email"
          className="mt-5"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          autoComplete="username"
        />
        <Button
          variant="outline"
          className="w-full mt-4"
          shape="rounded"
          onClick={handleSendCode}
          disabled={isRequesting || timeLeft > 0}
        >
          {isRequesting
            ? "Đang gửi mã..."
            : timeLeft > 0
            ? `Gửi lại sau ${timeLeft}s`
            : "Gửi mã"}
        </Button>

        <Input
          placeholder="Nhập mã tại đây..."
          type="number"
          className="mt-5"
          value={code}
          onChange={(e) => setCode(e.target.value)}
        />

        <Input
          placeholder="Nhập mật khẩu mới..."
          type="password"
          className="mt-3"
          value={newPassword}
          onChange={(e) => setNewPassword(e.target.value)}
        />

        <Button
          className="w-full mt-4"
          shape="rounded"
          onClick={handleResetPassword}
          disabled={isResetting}
        >
          {isResetting ? "Đang cập nhật..." : "Cập nhật mật khẩu"}
        </Button>
      </ConfirmModal>
    </>
  );
}

export default ForgetPassword;
