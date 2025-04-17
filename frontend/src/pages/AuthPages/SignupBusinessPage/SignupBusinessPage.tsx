import { useForm } from "react-hook-form";
import { z } from "zod";
import { zodResolver } from "@hookform/resolvers/zod";
import { useNavigate } from "react-router-dom";
import Dropdown from "@/components/Dropdown/Dropdown";
import IconText from "@/components/IconText/IconText";
import Input from "@/components/Input/Input";
import Button from "@/components/Button/Button";
import { useGetAllCities } from "@/services/city.service";
import { useGetAllCategories } from "@/services/category.service";
import { useSignUpBusiness } from "@/services/auth.service";

// Constants
const PASSWORD_MIN_LENGTH = 6;
const MAX_DESCRIPTION_LENGTH = 250;
const MAX_NAME_LENGTH = 50;
const MAX_CATEGORIES = 3;

// Schema definition
const schema = z.object({
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
  shortDescription: z
    .string()
    .min(1, { message: "Mô tả không được để trống" })
    .max(MAX_DESCRIPTION_LENGTH, {
      message: `Mô tả ngắn không được dài quá ${MAX_DESCRIPTION_LENGTH} ký tự`,
    }),
  name: z
    .string()
    .min(1, { message: "Tên không được để trống" })
    .max(MAX_NAME_LENGTH, {
      message: `Tên không được dài quá ${MAX_NAME_LENGTH} ký tự`,
    }),
  cityId: z
    .number({ required_error: "Thành phố không được để trống" })
    .min(1, { message: "Vui lòng chọn một thành phố" }),
  categoryIds: z
    .array(z.number(), { required_error: "Thể loại không được để trống" })
    .min(1, { message: "Chọn ít nhất một thể loại" })
    .max(MAX_CATEGORIES, { message: `Chọn tối đa ${MAX_CATEGORIES} thể loại` }),
});

export type FormSignupBusinessFields = z.infer<typeof schema>;

function SignupBusinessPage() {
  const navigate = useNavigate();
  const {
    register,
    handleSubmit,
    control,
    formState: { errors },
  } = useForm<FormSignupBusinessFields>({
    mode: "onBlur",
    resolver: zodResolver(schema),
  });

  // Data fetching
  const citiesQuery = useGetAllCities();
  const categoriesQuery = useGetAllCategories();
  const signupMutation = useSignUpBusiness();

  const onSubmit = handleSubmit(async (data: FormSignupBusinessFields) => {
    try {
      await signupMutation.mutateAsync(data, {
        onSuccess: () => navigate("/login"),
      });
    } catch (error) {}
  });

  const buttonText = signupMutation.isPending ? "Đang xử lý..." : "Đăng ký";

  return (
    <form
      onSubmit={onSubmit}
      className="mx-auto flex flex-col items-center py-10 sm:py-0 max-w-4xl"
    >
      <header className="text-center mb-8">
        <h1 className="text-xl font-medium">Đăng ký cho người làm dịch vụ</h1>
        <p className="mt-1 text-gray-600">
          Gia nhập cộng đồng MYAmie giúp nghiệp tiếp cận dễ dàng hơn đến các
          khách hàng tiềm năng
        </p>
      </header>

      <div className="flex flex-col sm:flex-row gap-7 w-full">
        {/* Account Information Section */}
        <section className="w-full sm:w-1/2">
          <IconText
            className="w-fit mx-auto"
            icon="fa-shield-keyhole"
            text="Thông tin tài khoản"
            textClasses="font-medium"
          />
          <div className="mt-5 space-y-4">
            <Input
              label="Email"
              placeholder="Nhập email của bạn"
              type="email"
              {...register("email")}
              errorMessage={errors.email?.message}
              autoComplete="email"
            />
            <Input
              label="Mật khẩu"
              placeholder="Nhập mật khẩu của bạn"
              type="password"
              {...register("password")}
              errorMessage={errors.password?.message}
              autoComplete="new-password"
              minLength={PASSWORD_MIN_LENGTH}
            />
            <Input
              label="Mô tả ngắn dịch vụ"
              placeholder="Nhập mô tả ngắn của bạn"
              type="text"
              {...register("shortDescription")}
              errorMessage={errors.shortDescription?.message}
              maxLength={MAX_DESCRIPTION_LENGTH}
            />
          </div>
        </section>

        <div className="border-l h-auto border-gray-200 hidden sm:block" />

        {/* Service Information Section */}
        <section className="w-full sm:w-1/2 mt-5 sm:mt-0">
          <IconText
            className="w-fit mx-auto"
            icon="fa-paper-plane"
            text="Miêu tả dịch vụ"
            textClasses="font-medium"
          />
          <div className="mt-5 space-y-4">
            <Input
              label="Tên dịch vụ"
              placeholder="Nhập tên dịch vụ..."
              {...register("name")}
              errorMessage={errors.name?.message}
              maxLength={MAX_NAME_LENGTH}
            />
            <Dropdown
              label="Thành phố"
              options={citiesQuery.data?.data ?? undefined}
              isLoading={citiesQuery.isLoading}
              isError={citiesQuery.isError}
              placeHolder="Chọn thành phố"
              name="cityId"
              control={control}
            />
            <Dropdown
              label={`Thể loại (tối đa ${MAX_CATEGORIES})`}
              options={categoriesQuery.data?.data ?? undefined}
              isMulti={true}
              isClearable={true}
              maxSelectItems={MAX_CATEGORIES}
              isLoading={categoriesQuery.isLoading}
              isError={categoriesQuery.isError}
              placeHolder="Chọn thể loại"
              name="categoryIds"
              control={control}
            />
          </div>
        </section>
      </div>

      <div className="w-full sm:w-1/2 mt-8">
        <Button
          type="submit"
          variant="outline"
          shape="rounded"
          disabled={signupMutation.isPending}
          aria-busy={signupMutation.isPending}
          className="w-full hover:cursor-pointer"
        >
          {buttonText}
        </Button>
        <p className="text-red-500 mt-3 text-xs font-medium text-center">
          (*) Sau khi đăng ký vui lòng cập nhật đầy đủ thông tin tại mục hồ sơ
        </p>
      </div>
    </form>
  );
}

export default SignupBusinessPage;
