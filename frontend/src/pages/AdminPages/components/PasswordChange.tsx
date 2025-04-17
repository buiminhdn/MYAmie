import Button from "@/components/Button/Button";
import Input from "@/components/Input/Input";
import ConfirmModal from "@/components/Modals/ConfirmModal";
import { useUpdateUserPassword } from "@/services/admin-user.service";
import { zodResolver } from "@hookform/resolvers/zod";
import { useState } from "react";
import { useForm } from "react-hook-form";
import { z } from "zod";

// Define props
interface PasswordChangeProps {
  id: number;
}

// Validation schema
const schema = z.object({
  id: z.number(),
  password: z.string().min(6, { message: "Mật khẩu phải có ít nhất 6 ký tự" }),
});

// Define form fields type
type FormPasswordChangeFields = z.infer<typeof schema>;

function PasswordChange({ id }: PasswordChangeProps) {
  const [isModalOpen, setIsModalOpen] = useState(false);

  // Initialize form with defaultValues to fix type error
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<FormPasswordChangeFields>({
    mode: "onBlur",
    resolver: zodResolver(schema),
    defaultValues: {
      id, // Ensure id is assigned at form initialization
    },
  });

  const { isPending, mutateAsync } = useUpdateUserPassword();

  const onSubmit = async (data: FormPasswordChangeFields) => {
    try {
      await mutateAsync({
        userId: data.id,
        password: data.password,
      });
      setIsModalOpen(false); // Close modal on success
    } catch (error) {
      setIsModalOpen(false); // Close modal even on error
    }
  };

  return (
    <>
      <button
        onClick={() => setIsModalOpen(true)}
        className="w-fit hover:underline hover:text-primary font-medium text-gray-500 hover:cursor-pointer"
      >
        Mật khẩu
      </button>
      <ConfirmModal
        isOpen={isModalOpen}
        onClose={() => setIsModalOpen(false)}
        className="w-11/12 md:w-96"
      >
        <form onSubmit={handleSubmit(onSubmit)}>
          {/* Hidden Input for ID */}
          <input type="hidden" {...register("id", { valueAsNumber: true })} />

          {/* Password Input */}
          <Input
            label="Mật khẩu mới"
            type="password"
            placeholder="Nhập mật khẩu mới"
            {...register("password")}
            errorMessage={errors.password?.message}
          />

          {/* Action Buttons */}
          <div className="flex flex-wrap sm:flex-nowrap gap-3 mt-4">
            <Button
              type="button"
              className="w-full"
              variant="ghost"
              onClick={() => setIsModalOpen(false)}
            >
              Trở lại
            </Button>
            <Button type="submit" className="w-full">
              {isPending ? "Đang cập nhật..." : "Cập nhật"}
            </Button>
          </div>
        </form>
      </ConfirmModal>
    </>
  );
}

export default PasswordChange;
