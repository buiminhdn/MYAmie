import { useCallback, useMemo } from "react";
import { useForm } from "react-hook-form";
import { z } from "zod";
import { zodResolver } from "@hookform/resolvers/zod";
import { useNavigate } from "react-router-dom";
import Input from "@/components/Input/Input";
import Button from "@/components/Button/Button";
import { useSignUp } from "@/services/auth.service";

// Constants
const PASSWORD_MIN_LENGTH = 6;
const NAME_MAX_LENGTH = 50;

// Schema definition outside component to prevent recreation
const signupSchema = z.object({
  email: z
    .string()
    .min(1, { message: "Email không được để trống" })
    .email({ message: "Email không đúng định dạng" }),
  password: z.string().min(PASSWORD_MIN_LENGTH, {
    message: `Mật khẩu phải dài hơn ${PASSWORD_MIN_LENGTH} ký tự`,
  }),
  firstname: z
    .string()
    .min(1, { message: "Tên không được để trống" })
    .max(NAME_MAX_LENGTH, {
      message: `Tên không được dài quá ${NAME_MAX_LENGTH} ký tự`,
    }),
  lastname: z
    .string()
    .min(1, { message: "Họ không được để trống" })
    .max(NAME_MAX_LENGTH, {
      message: `Họ không được dài quá ${NAME_MAX_LENGTH} ký tự`,
    }),
});

type FormSignupFields = z.infer<typeof signupSchema>;

function SignupPage() {
  const navigate = useNavigate();
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<FormSignupFields>({
    mode: "onBlur",
    resolver: zodResolver(signupSchema),
  });

  const { isPending, mutateAsync } = useSignUp();

  // Memoized location fetch to prevent unnecessary recreations
  const getLocation = useCallback((): Promise<{
    latitude: number;
    longitude: number;
  }> => {
    return new Promise((resolve) => {
      if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(
          ({ coords: { latitude, longitude } }) =>
            resolve({ latitude, longitude }),
          () => resolve({ latitude: 0, longitude: 0 }),
          { enableHighAccuracy: false, maximumAge: 0, timeout: 5000 }
        );
      } else {
        resolve({ latitude: 0, longitude: 0 });
      }
    });
  }, []);

  // Memoized onSubmit handler
  const onSubmit = useCallback(
    async (data: FormSignupFields) => {
      try {
        const location = await getLocation();
        await mutateAsync({
          email: data.email,
          password: data.password,
          firstName: data.firstname,
          lastName: data.lastname,
          ...location,
        });
        navigate("/login");
      } catch (error) {}
    },
    [getLocation, mutateAsync, navigate]
  );

  // Memoized button text
  const buttonText = useMemo(
    () => (isPending ? "Đang đăng ký..." : "Đăng ký"),
    [isPending]
  );

  return (
    <div className="max-w-md mx-auto">
      <h1 className="text-lg font-semibold">Đăng ký</h1>
      <form onSubmit={handleSubmit(onSubmit)} className="space-y-4 mt-3">
        <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
          <Input
            id="signup-lastname"
            label="Họ"
            placeholder="Nhập họ..."
            {...register("lastname")}
            errorMessage={errors.lastname?.message}
            autoComplete="family-name"
          />
          <Input
            id="signup-firstname"
            label="Tên"
            placeholder="Nhập tên..."
            {...register("firstname")}
            errorMessage={errors.firstname?.message}
            autoComplete="given-name"
          />
        </div>

        <Input
          id="signup-email"
          label="Email"
          type="email"
          placeholder="Nhập email..."
          {...register("email")}
          errorMessage={errors.email?.message}
          autoComplete="email"
        />

        <Input
          id="signup-password"
          label="Mật khẩu"
          type="password"
          placeholder="Nhập mật khẩu..."
          {...register("password")}
          errorMessage={errors.password?.message}
          autoComplete="new-password"
          minLength={PASSWORD_MIN_LENGTH}
        />

        <Button
          id="signup-button"
          type="submit"
          variant="outline"
          shape="rounded"
          className="w-full mt-2 hover:cursor-pointer"
          disabled={isPending}
          aria-busy={isPending}
        >
          {buttonText}
        </Button>
      </form>
    </div>
  );
}

export default SignupPage;
