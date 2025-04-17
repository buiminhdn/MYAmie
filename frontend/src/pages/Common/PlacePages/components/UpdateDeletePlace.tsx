import Button from "@/components/Button/Button";
import IconBtn from "@/components/Button/IconBtn";
import DeleteConfirmModal from "@/components/Modals/DeleteConfirmModal";
import { PlaceDetailVM } from "@/models/viewmodels/place.vm";
import { useDeletePlace } from "@/services/place.service";
import { useEffect, useRef, useState } from "react";
import UpdatePlace from "./UpdatePlace";
import CustomModal from "@/components/Modals/CustomModal";
import { useNavigate } from "react-router-dom";

function UpdateDeletePlace({ place }: { place: PlaceDetailVM }) {
  const [isDeleteOpen, setIsDeleteOpen] = useState(false);
  const [isEditOpen, setIsEditOpen] = useState(false);
  const [isPending, setIsPending] = useState(false);
  const updatePlaceRef = useRef<{ submit: () => void } | null>(null); // Ref for UpdatePlace
  const navigate = useNavigate();

  const { isPending: isDeleting, mutateAsync } = useDeletePlace();

  const openDeleteModal = () => setIsDeleteOpen(true);
  const closeDeleteModal = () => setIsDeleteOpen(false);
  const openEditModal = () => setIsEditOpen(true);
  const closeEditModal = () => setIsEditOpen(false);

  // Manage body overflow for modals
  useEffect(() => {
    document.body.style.overflow =
      isEditOpen || isDeleteOpen ? "hidden" : "auto";

    // Clean up when the component unmounts
    return () => {
      document.body.style.overflow = "auto";
    };
  }, [isEditOpen, isDeleteOpen]);

  const handleDelete = async () => {
    try {
      await mutateAsync(place.id, {
        onSuccess: () => {
          closeDeleteModal();
          navigate(`/user/${place.ownerId}`);
        },
        onError: closeDeleteModal,
      });
    } catch (error) {}
  };

  const handleUpdateSubmit = () => {
    updatePlaceRef.current?.submit(); // Trigger form submission
  };

  return (
    <>
      <IconBtn
        onClick={openDeleteModal}
        icon="fa-trash"
        className="hover:text-red-500 hover:cursor-pointer bg-primary-lighter hover:bg-white border-2 border-primary"
      />
      <Button
        onClick={openEditModal}
        variant="outline"
        className="text-xs font-semibold hover:cursor-pointer"
      >
        Cập nhật
      </Button>

      <DeleteConfirmModal
        isOpen={isDeleteOpen}
        onClose={closeDeleteModal}
        handleDelete={handleDelete}
        isPending={isDeleting}
      />
      <CustomModal
        title="Cập nhật địa điểm"
        isOpen={isEditOpen}
        onClose={closeEditModal}
        saveButtonTitle="Cập nhật"
        className="w-11/12 lg:w-1/2"
        isPending={isPending}
        onActiveClick={handleUpdateSubmit}
        onInactiveClick={closeEditModal}
      >
        <UpdatePlace
          detail={place}
          onCloseModal={closeEditModal}
          ref={updatePlaceRef}
          setIsPending={setIsPending}
        />
      </CustomModal>
    </>
  );
}

export default UpdateDeletePlace;
