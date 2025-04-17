import { forwardRef, useEffect, useImperativeHandle, useState } from "react";
import { useForm } from "react-hook-form";
import { z } from "zod";
import { zodResolver } from "@hookform/resolvers/zod";
import { useGetAllCities } from "@/services/city.service";
import { useGetAllCategories } from "@/services/category.service";
import { useUpdatePlace } from "@/services/place.service";
import Input from "@/components/Input/Input";
import Dropdown from "@/components/Dropdown/Dropdown";
import Textarea from "@/components/Input/Textarea";
import ImageUpload from "@/pages/ProfilePages/SettingPage/components/ImagesUpload";
import {
  MAX_ADDRESS_LENGTH,
  MAX_CATEGORIES_LENGTH,
  MAX_IMAGES_LENGTH,
  MAX_NAME_LENGTH,
  MAX_SHORT_DESCRIPTION_LENGTH,
} from "@/utils/constants";
import { PlaceDetailVM } from "@/models/viewmodels/place.vm";
import { resizeImage } from "@/utils/imageUtils";
import toast from "react-hot-toast";

interface UpdatePlaceProps {
  setIsPending: (value: boolean) => void;
  onCloseModal: () => void;
  detail: PlaceDetailVM;
}

const schema = z.object({
  id: z.number(),
  name: z
    .string()
    .min(1, "Tên không được để trống")
    .max(MAX_NAME_LENGTH, `Tên không được dài quá ${MAX_NAME_LENGTH} ký tự`),
  shortDescription: z
    .string()
    .min(1, "Mô tả ngắn không được để trống")
    .max(
      MAX_SHORT_DESCRIPTION_LENGTH,
      `Mô tả ngắn không được dài quá ${MAX_SHORT_DESCRIPTION_LENGTH} ký tự`
    ),
  cityId: z.number().min(1, "Vui lòng chọn một thành phố"),
  categoryIds: z
    .array(z.number())
    .min(1, "Chọn ít nhất một thể loại")
    .max(MAX_CATEGORIES_LENGTH, `Tối đa ${MAX_CATEGORIES_LENGTH} sở thích`),
  address: z
    .string()
    .min(1, "Địa chỉ không được để trống")
    .max(
      MAX_ADDRESS_LENGTH,
      `Địa chỉ không được dài quá ${MAX_ADDRESS_LENGTH} ký tự`
    ),
  description: z.string().min(1, "Mô tả chi tiết không được để trống"),
  imageFiles: z
    .array(z.instanceof(File))
    .max(MAX_IMAGES_LENGTH, `Tối đa ${MAX_IMAGES_LENGTH} hình ảnh`)
    .optional(),
});

type FormUpdatePlaceFields = z.infer<typeof schema>;

const UpdatePlace = forwardRef(
  ({ setIsPending, onCloseModal, detail }: UpdatePlaceProps, ref) => {
    const {
      register,
      handleSubmit,
      control,
      formState: { errors },
      reset,
    } = useForm<FormUpdatePlaceFields>({
      mode: "onBlur",
      resolver: zodResolver(schema),
    });

    const citiesQuery = useGetAllCities();
    const categoriesQuery = useGetAllCategories();
    const { isPending, mutateAsync } = useUpdatePlace();

    const [displayImages, setDisplayImages] = useState<string[]>([]);

    // Sync isPending with parent
    useEffect(() => {
      setIsPending(isPending);
    }, [isPending, setIsPending]);

    // Populate form with detail data
    useEffect(() => {
      if (detail) {
        reset({
          id: detail.id,
          name: detail.name,
          shortDescription: detail.shortDescription,
          cityId: detail.city.id,
          categoryIds: detail.categories.map((cat) => cat.id),
          address: detail.address,
          description: detail.description,
        });
        setDisplayImages(detail.images);
      }
    }, [detail, reset]);

    // Handle image removal
    const handleRemoveAllImages = () => {
      setDisplayImages([]);
    };

    // Form submission
    const handleFormSubmit = async (data: FormUpdatePlaceFields) => {
      const totalImages = (data.imageFiles?.length || 0) + displayImages.length;
      if (totalImages > MAX_IMAGES_LENGTH || totalImages < 1) {
        toast.error(`Tối đa ${MAX_IMAGES_LENGTH} hình ảnh`);
        return;
      }

      // Assuming resizeImage is defined elsewhere; otherwise, remove this part
      const resizedImages = data.imageFiles
        ? await Promise.all(
            data.imageFiles.map((file) => resizeImage(file, 2000, 1000))
          )
        : [];
      const validImages = resizedImages.filter(
        (file) => file !== null
      ) as File[];

      const finalData = {
        ...data,
        imageFiles: validImages,
        images: detail.images.join(";"),
      };

      try {
        await mutateAsync(finalData, {
          onSuccess: () => {
            onCloseModal();
          },
          onError: () => {
            onCloseModal();
          },
        });
      } catch (error) {}
    };

    // Expose submit method via ref
    useImperativeHandle(ref, () => ({
      submit: () => handleSubmit(handleFormSubmit)(),
    }));

    return (
      <form className="space-y-4" onSubmit={handleSubmit(handleFormSubmit)}>
        <Input
          type="number"
          {...register("id", { valueAsNumber: true })}
          hidden
        />
        <Input
          label="Tên địa điểm"
          placeholder="Nhập tên địa điểm"
          {...register("name")}
          errorMessage={errors.name?.message}
          maxLength={MAX_NAME_LENGTH}
        />
        <Input
          label="Mô tả ngắn"
          placeholder="Nhập mô tả ngắn"
          {...register("shortDescription")}
          errorMessage={errors.shortDescription?.message}
          maxLength={MAX_SHORT_DESCRIPTION_LENGTH}
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
          label={`Thể loại (${MAX_CATEGORIES_LENGTH})`}
          options={categoriesQuery.data?.data ?? undefined}
          isMulti={true}
          isClearable={true}
          maxSelectItems={MAX_CATEGORIES_LENGTH}
          isLoading={categoriesQuery.isLoading}
          isError={categoriesQuery.isError}
          placeHolder="Chọn thể loại"
          name="categoryIds"
          control={control}
        />
        <Input
          label="Địa chỉ"
          placeholder="Nhập địa chỉ"
          {...register("address")}
          errorMessage={errors.address?.message}
          maxLength={MAX_ADDRESS_LENGTH}
        />
        <Textarea
          label="Mô tả chi tiết"
          placeholder="Nhập mô tả chi tiết"
          {...register("description")}
          errorMessage={errors.description?.message}
        />
        <ImageUpload
          existingImages={displayImages}
          onRemoveAllExisting={handleRemoveAllImages}
          control={control}
          name="imageFiles"
          limit={MAX_IMAGES_LENGTH - displayImages.length}
        />
      </form>
    );
  }
);

UpdatePlace.displayName = "UpdatePlace";

export default UpdatePlace;
