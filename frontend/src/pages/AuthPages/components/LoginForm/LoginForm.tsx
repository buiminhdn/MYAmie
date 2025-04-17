import { memo, useCallback } from "react";
import { useForm } from "react-hook-form";
import { z } from "zod";
import { zodResolver } from "@hookform/resolvers/zod";
import Input from "@/components/Input/Input";
import { useSignIn } from "@/services/auth.service";
import Button from "@/components/Button/Button";

// Constants
const PASSWORD_MIN_LENGTH = 6;

// Schema definition outside component to prevent recreation on every render
const loginSchema = z.object({
  email: z
    .string()
    .min(1, { message: "Email không được để trống" })
    .email({ message: "Email không đúng định dạng" }),
  password: z
    .string()
    .min(1, { message: "Mật khẩu không được để trống" })
    .min(PASSWORD_MIN_LENGTH, {
      message: `Mật khẩu phải dài hơn ${PASSWORD_MIN_LENGTH} ký tự`,
    }),
});

export type FormLoginFields = z.infer<typeof loginSchema>;

function LoginForm() {
  const {
    register,
    formState: { errors },
    handleSubmit,
  } = useForm<FormLoginFields>({
    mode: "onBlur",
    resolver: zodResolver(loginSchema),
  });

  const { isPending, mutateAsync } = useSignIn();

  const onSubmit = useCallback(
    async (data: FormLoginFields) => {
      try {
        await mutateAsync(data);
      } catch (error) {}
    },
    [mutateAsync]
  );

  return (
    <form
      onSubmit={handleSubmit(onSubmit)}
      className="mt-3 space-y-4" // Increased space-y for better spacing
      noValidate // Disable browser validation
    >
      <Input
        {...register("email")}
        errorMessage={errors.email?.message}
        label="Email"
        type="email"
        placeholder="Nhập tài khoản Email..."
        id="login-email"
        autoComplete="username" // Better autocomplete support
      />

      <Input
        {...register("password")}
        errorMessage={errors.password?.message}
        label="Mật khẩu"
        type="password"
        placeholder="Nhập mật khẩu..."
        id="login-password"
        autoComplete="current-password" // Better autocomplete support
      />

      <Button
        disabled={isPending} // Disable if form is invalid
        type="submit"
        variant="outline"
        shape="rounded"
        className="w-full mt-2 hover:cursor-pointer" // Added margin-top
        id="login-button"
        aria-busy={isPending} // Accessibility improvement
      >
        {isPending ? "Đang đăng nhập..." : "Đăng nhập"}
      </Button>
    </form>
  );
}

export default memo(LoginForm);
