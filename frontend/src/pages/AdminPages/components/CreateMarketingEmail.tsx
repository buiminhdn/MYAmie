import Button from "@/components/Button/Button";
import Input from "@/components/Input/Input";
import Textarea from "@/components/Input/Textarea";
import CustomModal from "@/components/Modals/CustomModal";
import { useAddMarketingEmail } from "@/services/email.service";
import { MAX_NAME_LENGTH } from "@/utils/constants";
import { zodResolver } from "@hookform/resolvers/zod";
import { useEffect, useState } from "react";
import { useForm } from "react-hook-form";
import { z } from "zod";

const schema = z.object({
  subject: z
    .string()
    .min(1, "Tiêu đề không được để trống")
    .max(
      MAX_NAME_LENGTH,
      `Tiêu đề không được dài quá ${MAX_NAME_LENGTH} ký tự`
    ),
  body: z.string().min(1, "Nội dung không được để trống"),
});

type FormCreateEmailFields = z.infer<typeof schema>;

function CreateMarketingEmail() {
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<FormCreateEmailFields>({
    mode: "onBlur",
    resolver: zodResolver(schema),
  });

  const [isOpen, setIsOpen] = useState(false);

  const { mutateAsync, isPending } = useAddMarketingEmail();

  const handleFormSubmit = async (data: FormCreateEmailFields) => {
    try {
      await mutateAsync(data, {
        onSuccess: () => {
          setIsOpen(false); // Close modal on success
        },
        onError: () => {
          setIsOpen(false); // Close modal on error
        },
      });
    } catch (error) {}
  };

  const closeModal = () => setIsOpen(false);
  const openModal = () => setIsOpen(true);

  useEffect(() => {
    document.body.style.overflow = isOpen ? "hidden" : "auto";
    return () => {
      document.body.style.overflow = "auto"; // Reset overflow when modal is closed
    };
  }, [isOpen]);

  return (
    <>
      <Button
        variant="outline"
        className="text-xs font-medium hover:cursor-pointer mb-7"
        onClick={openModal}
      >
        Tạo Email
      </Button>
      <CustomModal
        title="Tạo Email Marketing"
        isOpen={isOpen}
        onClose={closeModal}
        isPending={isPending}
        saveButtonTitle="Tạo"
        className="w-11/12 lg:w-1/2"
        onActiveClick={handleSubmit(handleFormSubmit)} // Trigger form submission
      >
        <form className="space-y-4">
          <Input
            label="Tiêu đề"
            placeholder="Nhập tiêu đề"
            {...register("subject")}
            errorMessage={errors.subject?.message}
            maxLength={MAX_NAME_LENGTH}
          />
          <Textarea
            label="Nội dung"
            placeholder="Nhập nội dung"
            {...register("body")}
            errorMessage={errors.body?.message}
          />
        </form>
      </CustomModal>
    </>
  );
}

export default CreateMarketingEmail;
