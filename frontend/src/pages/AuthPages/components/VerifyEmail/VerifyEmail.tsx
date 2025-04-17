import Button from "@/components/Button/Button";
import Input from "@/components/Input/Input";
import ConfirmModal from "@/components/Modals/ConfirmModal";
import { VerificationTypeParam } from "@/models/params/email.param";
import {
  useRequestVerification,
  useVerifyEmail,
} from "@/services/email.service";
import { useState, useEffect, useCallback } from "react";
import { toast } from "react-hot-toast";

const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
const RESEND_DELAY = 60;

function VerifyEmail() {
  const [isOpen, setIsOpen] = useState(false);
  const [formData, setFormData] = useState({ email: "", code: "" });
  const [timeLeft, setTimeLeft] = useState(0);

  const { mutateAsync: requestVerification, isPending: isRequesting } =
    useRequestVerification();
  const { mutateAsync: verify, isPending: isVerifying } = useVerifyEmail();

  const closeModal = useCallback(() => setIsOpen(false), []);
  const openModal = useCallback(() => setIsOpen(true), []);

  // Update form state
  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  // Timer logic for resend delay
  useEffect(() => {
    if (timeLeft > 0) {
      const timer = setTimeout(() => setTimeLeft((prev) => prev - 1), 1000);
      return () => clearTimeout(timer);
    }
  }, [timeLeft]);

  const handleRequestVerification = useCallback(async () => {
    if (!formData.email) return toast.error("Vui lòng nhập email");
    if (!emailRegex.test(formData.email))
      return toast.error("Email không hợp lệ");

    try {
      await requestVerification({
        email: formData.email,
        type: VerificationTypeParam.AccountConfirmation,
      });
      setTimeLeft(RESEND_DELAY);
    } catch (error) {}
  }, [formData.email, requestVerification]);

  const handleVerifyEmail = useCallback(async () => {
    const { email, code } = formData;
    if (!email || !code)
      return toast.error("Vui lòng nhập email và mã xác minh");

    try {
      await verify({ email, code });
      closeModal();
    } catch (error) {}
  }, [formData, verify, closeModal]);

  return (
    <>
      <p
        className="text-xs font-medium text-gray-700 hover:underline hover:cursor-pointer"
        onClick={openModal}
      >
        Xác minh email
      </p>

      <ConfirmModal
        isOpen={isOpen}
        onClose={closeModal}
        className="w-11/12 sm:w-1/2 xl:w-1/3"
      >
        <p className="text-xl font-semibold text-center">XÁC MINH EMAIL</p>
        <p className="text-gray-700 text-center mt-2">
          Vui lòng nhập email để xác minh tài khoản
        </p>

        <Input
          name="email"
          placeholder="Nhập email tại đây..."
          className="mt-5"
          value={formData.email}
          onChange={handleChange}
        />
        <Button
          variant="outline"
          className="w-full mt-4"
          shape="rounded"
          onClick={handleRequestVerification}
          disabled={isRequesting || timeLeft > 0}
        >
          {isRequesting
            ? "Đang gửi mã..."
            : timeLeft > 0
            ? `Gửi lại sau ${timeLeft}s`
            : "Gửi mã"}
        </Button>

        <Input
          name="code"
          placeholder="Nhập mã tại đây..."
          type="number"
          className="mt-5"
          value={formData.code}
          onChange={handleChange}
        />
        <Button
          className="w-full mt-4 hover:cursor-pointer"
          shape="rounded"
          onClick={handleVerifyEmail}
          disabled={isVerifying}
        >
          {isVerifying ? "Đang xác minh..." : "Xác minh"}
        </Button>
      </ConfirmModal>
    </>
  );
}

export default VerifyEmail;
