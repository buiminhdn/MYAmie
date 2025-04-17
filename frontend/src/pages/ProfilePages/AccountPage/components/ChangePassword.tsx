import Button from "@/components/Button/Button";
import Input from "@/components/Input/Input";
import { useChangePassword } from "@/services/account.service";
import { zodResolver } from "@hookform/resolvers/zod";
import { useForm } from "react-hook-form";
import { z } from "zod";

const schema = z
  .object({
    oldPassword: z.string().min(6, "Mật khẩu phải dài hơn 6 ký tự"),
    newPassword: z.string().min(6, "Mật khẩu phải dài hơn 6 ký tự"),
    confirmPassword: z.string().min(6, "Mật khẩu phải dài hơn 6 ký tự"),
  })
  .refine((data) => data.newPassword === data.confirmPassword, {
    message: "Mật khẩu xác nhận không khớp",
    path: ["confirmPassword"],
  });

type FormChangePassFields = z.infer<typeof schema>;

function ChangePassword() {
  const {
    register,
    formState: { errors },
    handleSubmit,
    reset,
  } = useForm<FormChangePassFields>({
    mode: "onBlur",
    resolver: zodResolver(schema),
  });

  const { isPending, mutateAsync } = useChangePassword();

  const onSubmit = async (data: FormChangePassFields) => {
    try {
      await mutateAsync(
        { oldPassword: data.oldPassword, newPassword: data.newPassword },
        { onSuccess: () => reset() }
      );
    } catch (error) {}
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)}>
      <div className="flex flex-wrap md:flex-nowrap gap-6 mb-4">
        <Input
          type="password"
          label="Mật khẩu mới"
          placeholder="Nhập mật khẩu mới..."
          autoComplete="new-password"
          {...register("newPassword")}
          errorMessage={errors.newPassword?.message}
        />
        <Input
          type="password"
          label="Xác nhận mật khẩu"
          placeholder="Nhập lại mật khẩu mới..."
          autoComplete="new-password"
          {...register("confirmPassword")}
          errorMessage={errors.confirmPassword?.message}
        />
      </div>
      <Input
        type="password"
        label="Mật khẩu cũ"
        placeholder="Nhập mật khẩu cũ..."
        autoComplete="current-password"
        {...register("oldPassword")}
        errorMessage={errors.oldPassword?.message}
      />
      <div className="flex justify-end mt-4 text-xs">
        <Button disabled={isPending} type="submit" variant="outline">
          {isPending ? "Đang cập nhật..." : "Cập nhật"}
        </Button>
      </div>
    </form>
  );
}

export default ChangePassword;
