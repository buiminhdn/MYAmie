import Button from "@/components/Button/Button";
import CustomModal from "@/components/Modals/CustomModal";
import { accountIdSelector } from "@/store/auth/auth.selector";
import { useSelector } from "react-redux";
import CreatePlace from "./CreatePlace";
import { useEffect, useRef, useState } from "react";

interface CreatePlaceSectionProps {
  userId: number;
}

function CreatePlaceSection({ userId }: CreatePlaceSectionProps) {
  const currentUserId = useSelector(accountIdSelector);
  const [isOpen, setIsOpen] = useState(false);
  const [isPending, setIsPending] = useState(false);
  const createPlaceRef = useRef<{ submit: () => void } | null>(null); // Ref for CreatePlace

  const closeModal = () => setIsOpen(false);
  const openModal = () => setIsOpen(true);

  const handleCreateSubmit = () => {
    if (createPlaceRef.current?.submit) {
      createPlaceRef.current.submit(); // Trigger form submission
    }
  };

  useEffect(() => {
    document.body.style.overflow = isOpen ? "hidden" : "auto";
    return () => {
      document.body.style.overflow = "auto";
    };
  }, [isOpen]);

  if (currentUserId !== userId) return null;

  return (
    <>
      <Button
        variant="outline"
        className="text-xs font-medium hover:cursor-pointer"
        onClick={openModal}
      >
        Thêm địa điểm
      </Button>
      <CustomModal
        title="Thêm địa điểm mới"
        isOpen={isOpen}
        onClose={closeModal}
        isPending={isPending}
        saveButtonTitle="Thêm"
        className="w-11/12 lg:w-1/2"
        onActiveClick={handleCreateSubmit} // Handle save button click
      >
        <CreatePlace
          ref={createPlaceRef}
          setIsPending={setIsPending}
          onCloseModal={closeModal}
        />
      </CustomModal>
    </>
  );
}

export default CreatePlaceSection;
