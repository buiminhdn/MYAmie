import { useEffect, useState } from "react";
import { zodResolver } from "@hookform/resolvers/zod";
import { useForm } from "react-hook-form";
import { z } from "zod";
import toast from "react-hot-toast";

import Input from "@/components/Input/Input";
import Textarea from "@/components/Input/Textarea";
import Dropdown from "@/components/Dropdown/Dropdown";
import { resizeImage } from "@/utils/imageUtils";
import { BusinessProfileVM } from "@/models/viewmodels/profile.vm";
import { useGetAllCities } from "@/services/city.service";
import { useGetAllCategories } from "@/services/category.service";
import { useUpdateBusinessProfile } from "@/services/account.service";
import { UpdateBusinessProfileParams } from "@/models/params/account.param";
import ImageUpload from "./ImagesUpload";
import Button from "@/components/Button/Button";

// Constants
const MAX_IMAGES = 10;
const MAX_CATEGORIES = 3;
const MAX_SHORT_DESCRIPTION_LENGTH = 250;
const MAX_NAME_LENGTH = 50;
const MAX_ADDRESS = 100;

interface DetailProfileBusinessUploadProps {
  detail: BusinessProfileVM;
}

const schema = z
  .object({
    name: z
      .string()
      .min(1, "Tên không được để trống")
      .max(MAX_NAME_LENGTH, `Tên không được dài quá ${MAX_NAME_LENGTH} ký tự`),
    cityId: z.number().min(1, "Vui lòng chọn một thành phố"),
    address: z
      .string()
      .max(MAX_ADDRESS, "Địa chỉ không được dài quá 250 ký tự")
      .optional()
      .nullable(),
    shortDescription: z
      .string()
      .min(1, "Mô tả ngắn không được để trống")
      .max(
        MAX_SHORT_DESCRIPTION_LENGTH,
        "Mô tả ngắn không được dài quá 250 ký tự"
      ),
    description: z.string(),
    phone: z
      .string()
      .regex(/^\d{1,15}$/, "Số điện thoại không hợp lệ")
      .optional()
      .nullable(),
    openHour: z
      .number()
      .min(0, "Giờ mở cửa không được nhỏ hơn 0")
      .max(24, "Giờ mở cửa không được lớn hơn 24"),
    closeHour: z
      .number()
      .min(1, "Giờ đóng cửa không được nhỏ hơn 1")
      .max(24, "Giờ đóng cửa không được lớn hơn 24"),
    categoryIds: z
      .array(z.number())
      .min(1, "Chọn ít nhất một thể loại")
      .max(MAX_CATEGORIES, `Chọn tối đa ${MAX_CATEGORIES} thể loại`),
    imageFiles: z
      .array(z.instanceof(File))
      .max(MAX_IMAGES, `Tối đa ${MAX_IMAGES} hình ảnh`),
  })
  .refine((data) => data.closeHour > data.openHour, {
    message: "Giờ đóng cửa phải lớn hơn giờ mở cửa",
    path: ["closeHour"],
  });

type FormUpdateProfileFields = z.infer<typeof schema>;

const getDefaultValues = (
  detail: BusinessProfileVM
): FormUpdateProfileFields => ({
  name: detail.name,
  cityId: detail.cityId || 0,
  address: detail.address,
  shortDescription: detail.shortDescription,
  description: detail.description || "",
  phone: detail.phone,
  openHour: detail.openHour,
  closeHour: detail.closeHour,
  categoryIds: detail.categoryIds || [],
  imageFiles: [],
});

function DetailProfileBusinessUpload({
  detail,
}: DetailProfileBusinessUploadProps) {
  const [displayImages, setDisplayImages] = useState<string[]>(detail.images);

  useEffect(() => {
    setDisplayImages(detail.images || []);
  }, [detail.images]);

  const {
    reset,
    register,
    handleSubmit,
    control,
    formState: { errors, isDirty },
  } = useForm<FormUpdateProfileFields>({
    mode: "onBlur",
    resolver: zodResolver(schema),
    defaultValues: getDefaultValues(detail),
  });

  const citiesQuery = useGetAllCities();
  const categoriesQuery = useGetAllCategories();
  const { isPending, mutateAsync } = useUpdateBusinessProfile();

  const onSubmit = async (data: FormUpdateProfileFields) => {
    const totalImages = displayImages.length;
    if (totalImages > MAX_IMAGES) {
      toast.error(`Tối đa ${MAX_IMAGES} hình ảnh`);
      return;
    }

    try {
      const resizedImages = await Promise.all(
        data.imageFiles.map((file) => resizeImage(file, 2000, 1000))
      );

      const validImages = resizedImages.filter(
        (file) => file !== null
      ) as File[];

      const finalData: UpdateBusinessProfileParams = {
        ...data,
        phone: data.phone || "",
        address: data.address || "",
        imageFiles: validImages,
        images: detail.images.join(";"),
      };

      await mutateAsync(finalData, {
        onSuccess: () => {
          reset({ ...data, imageFiles: [] });
        },
      });
    } catch (error) {}
  };

  const handleRemoveAllImages = () => {
    setDisplayImages([]);
  };

  return (
    <form
      onSubmit={handleSubmit(onSubmit)}
      className="flex flex-col w-full gap-4"
    >
      <div className="grid grid-cols-1 sm:grid-cols-2 gap-5">
        <Input
          label="Tên dịch vụ"
          placeholder="Nhập tên dịch vụ"
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

        <Input
          type="number"
          min={0}
          max={24}
          label="Giờ mở cửa"
          placeholder="Nhập giờ mở cửa"
          {...register("openHour", { valueAsNumber: true })}
          errorMessage={errors.openHour?.message}
        />

        <Input
          type="number"
          min={1}
          max={24}
          label="Giờ đóng cửa"
          placeholder="Nhập giờ đóng cửa"
          {...register("closeHour", { valueAsNumber: true })}
          errorMessage={errors.closeHour?.message}
        />
      </div>

      <Input
        label="Số điện thoại"
        placeholder="Nhập số điện thoại..."
        {...register("phone")}
        errorMessage={errors.phone?.message}
      />

      <Input
        label="Địa chỉ"
        placeholder="Nhập địa chỉ..."
        {...register("address")}
        errorMessage={errors.address?.message}
        maxLength={MAX_ADDRESS}
      />

      <Input
        label="Mô tả ngắn"
        placeholder="Nhập mô tả..."
        {...register("shortDescription")}
        errorMessage={errors.shortDescription?.message}
        maxLength={MAX_SHORT_DESCRIPTION_LENGTH}
      />

      <Textarea
        label="Mô tả chi tiết"
        placeholder="Nhập mô tả chi tiết..."
        {...register("description")}
        errorMessage={errors.description?.message}
      />

      <Dropdown
        label={`Thể loại (${MAX_CATEGORIES})`}
        options={categoriesQuery.data?.data ?? undefined}
        isMulti={true}
        isClearable={true}
        maxSelectItems={MAX_CATEGORIES}
        isLoading={categoriesQuery.isLoading}
        isError={categoriesQuery.isError}
        placeHolder="Chọn sở thích"
        name="categoryIds"
        control={control}
      />

      <ImageUpload
        existingImages={displayImages}
        onRemoveAllExisting={handleRemoveAllImages}
        control={control}
        name="imageFiles"
        limit={MAX_IMAGES - displayImages.length}
      />

      <Button
        disabled={isPending || !isDirty}
        type="submit"
        variant="outline"
        className="ml-auto w-fit text-xs font-medium"
        aria-label="Lưu thay đổi"
      >
        {isPending ? "Đang cập nhật..." : "Lưu thay đổi"}
      </Button>
    </form>
  );
}

export default DetailProfileBusinessUpload;
