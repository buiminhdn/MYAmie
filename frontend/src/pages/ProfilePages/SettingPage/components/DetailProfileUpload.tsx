import { useEffect, useState } from "react";
import { zodResolver } from "@hookform/resolvers/zod";
import { Controller, useForm } from "react-hook-form";
import { z } from "zod";
import toast from "react-hot-toast";

import Input from "@/components/Input/Input";
import Textarea from "@/components/Input/Textarea";
import Dropdown from "@/components/Dropdown/Dropdown";
import Button from "@/components/Button/Button";
import CharacteristicSelect from "@/components/CharacteristicItem/CharacteristicSelect";
import { useGetAllCategories } from "@/services/category.service";
import { useGetAllCities } from "@/services/city.service";
import { useUpdateProfile } from "@/services/account.service";
import { UserProfileVM } from "@/models/viewmodels/profile.vm";
import ImageUpload from "./ImagesUpload";
import { UpdateProfileParams } from "@/models/params/account.param";
import { formatDateForInput } from "@/utils/dateTimeUtils";
import { resizeImage } from "@/utils/imageUtils";
import {
  MAX_CATEGORIES_LENGTH,
  MAX_CHARACTERISTICS_LENGTH,
  MAX_IMAGES_LENGTH,
  MAX_NAME_LENGTH,
  MAX_SHORT_DESCRIPTION_LENGTH,
} from "@/utils/constants";

interface DetailProfileUploadProps {
  detail: UserProfileVM;
}

const schema = z.object({
  lastName: z
    .string()
    .min(1, "Họ không được để trống")
    .max(MAX_NAME_LENGTH, `Họ không được dài quá ${MAX_NAME_LENGTH} ký tự`),
  firstName: z
    .string()
    .min(1, "Tên không được để trống")
    .max(MAX_NAME_LENGTH, `Tên không được dài quá ${MAX_NAME_LENGTH} ký tự`),
  dateOfBirth: z.string(),
  cityId: z.number().min(1, "Vui lòng chọn một thành phố"),
  shortDescription: z
    .string()
    .max(
      MAX_SHORT_DESCRIPTION_LENGTH,
      `Mô tả ngắn không được dài quá ${MAX_SHORT_DESCRIPTION_LENGTH} ký tự`
    ),
  description: z.string(),
  categoryIds: z
    .array(z.number())
    .max(MAX_CATEGORIES_LENGTH, `Tối đa ${MAX_CATEGORIES_LENGTH} sở thích`),
  characteristics: z
    .array(z.string())
    .max(
      MAX_CHARACTERISTICS_LENGTH,
      `Tối đa ${MAX_CHARACTERISTICS_LENGTH} đặc điểm`
    ),
  imageFiles: z
    .array(z.instanceof(File))
    .max(MAX_IMAGES_LENGTH, `Tối đa ${MAX_IMAGES_LENGTH} hình ảnh`),
});

type FormUpdateProfileFields = z.infer<typeof schema>;

const getDefaultValues = (detail: UserProfileVM): FormUpdateProfileFields => ({
  lastName: detail.lastName || "",
  firstName: detail.firstName || "",
  dateOfBirth: formatDateForInput(detail.dateOfBirth) || "",
  cityId: detail.cityId || 0,
  shortDescription: detail.shortDescription || "",
  description: detail.description || "",
  categoryIds: detail.categoryIds || [],
  characteristics: detail.characteristics || [],
  imageFiles: [],
});

function DetailProfileUpload({ detail }: DetailProfileUploadProps) {
  const [displayImages, setDisplayImages] = useState(detail.images || []);

  useEffect(() => {
    setDisplayImages(detail.images || []);
  }, [detail.images]);

  const {
    reset,
    register,
    handleSubmit,
    formState: { errors, isDirty },
    control,
  } = useForm<FormUpdateProfileFields>({
    mode: "onBlur",
    resolver: zodResolver(schema),
    defaultValues: getDefaultValues(detail),
  });

  const citiesQuery = useGetAllCities();
  const categoriesQuery = useGetAllCategories();
  const { isPending, mutateAsync } = useUpdateProfile();

  const onSubmit = async (data: FormUpdateProfileFields) => {
    const totalImages = displayImages.length;
    if (totalImages > MAX_IMAGES_LENGTH) {
      toast.error(`Tối đa ${MAX_IMAGES_LENGTH} hình ảnh`);
      return;
    }

    try {
      const resizedImages = await Promise.all(
        data.imageFiles.map((file) => resizeImage(file, 2000, 1000))
      );

      const validImages = resizedImages.filter(
        (file) => file !== null
      ) as File[];

      const finalData: UpdateProfileParams = {
        ...data,
        imageFiles: validImages,
        images: detail.images.join(";"),
      };

      await mutateAsync(finalData, {
        onSuccess: () => {
          reset({ ...data, imageFiles: [] });
        },
      });
    } catch {}
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
          label="Họ"
          placeholder="Nhập họ"
          {...register("lastName")}
          errorMessage={errors.lastName?.message}
          maxLength={MAX_NAME_LENGTH}
        />
        <Input
          label="Tên"
          placeholder="Nhập tên"
          {...register("firstName")}
          errorMessage={errors.firstName?.message}
          maxLength={MAX_NAME_LENGTH}
        />
        <Input
          type="date"
          label="Ngày sinh"
          {...register("dateOfBirth")}
          errorMessage={errors.dateOfBirth?.message}
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
      </div>

      <Input
        label="Mô tả ngắn"
        placeholder="Nhập mô tả..."
        {...register("shortDescription")}
        errorMessage={errors.shortDescription?.message}
        maxLength={MAX_SHORT_DESCRIPTION_LENGTH}
      />
      <Textarea
        label="Mô tả"
        placeholder="Nhập mô tả chi tiết..."
        {...register("description")}
        errorMessage={errors.description?.message}
      />

      <Dropdown
        label={`Sở thích (${MAX_CATEGORIES_LENGTH})`}
        options={categoriesQuery.data?.data ?? undefined}
        isMulti={true}
        isClearable={true}
        maxSelectItems={MAX_CATEGORIES_LENGTH}
        isLoading={categoriesQuery.isLoading}
        isError={categoriesQuery.isError}
        placeHolder="Chọn sở thích"
        name="categoryIds"
        control={control}
      />

      <Controller
        name="characteristics"
        control={control}
        render={({ field }) => (
          <CharacteristicSelect
            currentCharacteristics={field.value}
            onChange={field.onChange}
            errorMessage={errors.characteristics?.message}
          />
        )}
      />

      <ImageUpload
        existingImages={displayImages}
        onRemoveAllExisting={handleRemoveAllImages}
        control={control}
        name="imageFiles"
        limit={MAX_IMAGES_LENGTH - displayImages.length}
      />

      <Button
        disabled={isPending || !isDirty}
        type="submit"
        variant="outline"
        className="ml-auto font-medium"
      >
        {isPending ? "Đang cập nhật..." : "Lưu thay đổi"}
      </Button>
    </form>
  );
}

export default DetailProfileUpload;
