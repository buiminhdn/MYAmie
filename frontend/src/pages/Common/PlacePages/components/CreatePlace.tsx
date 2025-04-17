import { forwardRef, useEffect, useImperativeHandle } from "react";
import { useForm } from "react-hook-form";
import { z } from "zod";
import { zodResolver } from "@hookform/resolvers/zod";
import { useGetAllCities } from "@/services/city.service";
import { useGetAllCategories } from "@/services/category.service";
import { useAddPlace } from "@/services/place.service";
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

interface CreatePlaceProps {
  setIsPending: (value: boolean) => void;
  onCloseModal: () => void;
}

const schema = z.object({
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
      `Địa chỉ ngắn không được dài quá ${MAX_ADDRESS_LENGTH} ký tự`
    ),
  description: z.string().min(1, "Mô tả chi tiết không được để trống"),
  imageFiles: z
    .array(z.instanceof(File), {
      required_error: "Hình ảnh không được để trống",
    })
    .min(1, "Tải lên ít nhất một hình ảnh")
    .max(MAX_IMAGES_LENGTH, `Tối đa ${MAX_IMAGES_LENGTH} hình ảnh`),
});

type FormCreatePlaceFields = z.infer<typeof schema>;

const CreatePlace = forwardRef(
  ({ setIsPending, onCloseModal }: CreatePlaceProps, ref) => {
    const {
      register,
      handleSubmit,
      control,
      formState: { errors },
    } = useForm<FormCreatePlaceFields>({
      mode: "onBlur",
      resolver: zodResolver(schema),
    });

    const citiesQuery = useGetAllCities();
    const categoriesQuery = useGetAllCategories();
    const { isPending, mutateAsync } = useAddPlace();

    useEffect(() => {
      setIsPending(isPending);
    }, [isPending, setIsPending]);

    const handleFormSubmit = async (data: FormCreatePlaceFields) => {
      try {
        await mutateAsync(data, {
          onSuccess: () => {
            onCloseModal();
          },
          onError: () => {
            onCloseModal();
          },
        });
      } catch (error) {}
    };

    useImperativeHandle(ref, () => ({
      submit: () => handleSubmit(handleFormSubmit)(),
    }));

    return (
      <form className="space-y-4" onSubmit={handleSubmit(handleFormSubmit)}>
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
          label={`Thể loại (${3})`}
          options={categoriesQuery.data?.data ?? undefined}
          isMulti={true}
          isClearable={true}
          maxSelectItems={3}
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
        <ImageUpload control={control} name="imageFiles" limit={10} />
      </form>
    );
  }
);

CreatePlace.displayName = "CreatePlace"; // Add display name for better debugging

export default CreatePlace;
