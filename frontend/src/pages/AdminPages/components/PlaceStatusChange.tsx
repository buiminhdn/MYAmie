import Button from "@/components/Button/Button";
import Dropdown from "@/components/Dropdown/Dropdown";
import ConfirmModal from "@/components/Modals/ConfirmModal";
import { PlaceStatus } from "@/models/app.interface";
import { useUpdatePlaceStatus } from "@/services/admin-place.service";
import { zodResolver } from "@hookform/resolvers/zod";
import { useState, useMemo } from "react";
import { useForm } from "react-hook-form";
import { z } from "zod";

interface StatusChangeProps {
  id: number;
  status: PlaceStatus;
}

const schema = z.object({
  id: z.number(),
  status: z.number({ required_error: "Trạng thái không được để trống" }),
});

type FormStatusChangeFields = z.infer<typeof schema>;

function PlaceStatusChange({ id, status }: StatusChangeProps) {
  const [isModalOpen, setIsModalOpen] = useState(false);
  const { register, handleSubmit, control } = useForm<FormStatusChangeFields>({
    mode: "onBlur",
    resolver: zodResolver(schema),
    defaultValues: { status }, // Set initial value
  });

  const { isPending, mutateAsync } = useUpdatePlaceStatus();

  const dropdownOptions = useMemo(
    () => [
      { name: "Kích hoạt", id: PlaceStatus.ACTIVATED },
      { name: "Khóa", id: PlaceStatus.SUSPENDED },
    ],
    []
  );

  const handleSubmitForm = async (data: FormStatusChangeFields) => {
    try {
      mutateAsync({
        placeId: data.id,
        status: data.status,
      }).finally(() => setIsModalOpen(false));
    } catch (error) {}
  };

  return (
    <>
      <button
        onClick={() => setIsModalOpen((prev) => !prev)}
        className=" text-xs w-fit hover:underline hover:text-primary text-gray-500 font-medium hover:cursor-pointer"
      >
        Thay đổi
      </button>
      <ConfirmModal
        isOpen={isModalOpen}
        onClose={() => setIsModalOpen(false)}
        className="w-11/12 md:w-96"
      >
        <form onSubmit={handleSubmit(handleSubmitForm)}>
          <input
            type="hidden"
            value={id}
            {...register("id", { valueAsNumber: true })}
          />
          <Dropdown
            placeHolder="Chọn trạng thái"
            label="Trạng thái"
            options={dropdownOptions}
            name="status"
            control={control}
          />
          <div className="flex flex-wrap sm:flex-nowrap gap-3 mt-4">
            <Button
              type="button"
              className="w-full"
              variant="ghost"
              onClick={() => setIsModalOpen(false)}
            >
              Trở lại
            </Button>
            <Button type="submit" className="w-full" disabled={isPending}>
              {isPending ? "Đang thay đổi..." : "Thay đổi"}
            </Button>
          </div>
        </form>
      </ConfirmModal>
    </>
  );
}

export default PlaceStatusChange;
