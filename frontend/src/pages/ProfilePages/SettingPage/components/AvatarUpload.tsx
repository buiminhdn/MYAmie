import Button from "@/components/Button/Button";
import { ImageTypeParam } from "@/models/params/account.param";
import { useUpdateAvatarOrCover } from "@/services/account.service";
import { updateAvatar } from "@/store/auth/auth.slice";
import getImageUrl, { resizeImage } from "@/utils/imageUtils";
import { useEffect, useRef, useState } from "react";
import { useDispatch } from "react-redux";

interface AvatarUploadProps {
  image?: string;
}

function AvatarUpload({ image }: AvatarUploadProps) {
  const [avatarImg, setAvatarImg] = useState<string>(image || "");
  const [selectedFile, setSelectedFile] = useState<File | null>(null);
  const fileInputRef = useRef<HTMLInputElement | null>(null);
  const prevObjectUrl = useRef<string | null>(null);
  const dispatch = useDispatch();

  const { isPending, mutateAsync } = useUpdateAvatarOrCover();

  useEffect(() => {
    setAvatarImg(image || "");
  }, [image]);

  const handleFileChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const file = event.target.files?.[0];
    if (!file) return;

    if (prevObjectUrl.current) {
      URL.revokeObjectURL(prevObjectUrl.current);
    }

    const objectUrl = URL.createObjectURL(file);
    prevObjectUrl.current = objectUrl;
    setAvatarImg(objectUrl);
    setSelectedFile(file);

    fileInputRef.current && (fileInputRef.current.value = "");
  };

  const handleUndo = () => {
    if (prevObjectUrl.current) {
      URL.revokeObjectURL(prevObjectUrl.current);
      prevObjectUrl.current = null;
    }
    dispatch(updateAvatar(image || ""));
    setAvatarImg(image || "");
    setSelectedFile(null);
  };

  const handleSave = async () => {
    if (!selectedFile) return;
    try {
      const resizedFile = await resizeImage(selectedFile, 1000, 1000);
      if (!resizedFile) return;

      await mutateAsync({
        type: ImageTypeParam.Avatar,
        imageFile: resizedFile,
      });
      if (prevObjectUrl.current) {
        URL.revokeObjectURL(prevObjectUrl.current);
        prevObjectUrl.current = null;
      }
      setSelectedFile(null);
    } catch (error) {}
  };

  useEffect(() => {
    setAvatarImg(image || "");
    if (image) {
      dispatch(updateAvatar(image));
    }
  }, [image]);

  return (
    <div className="flex-none mx-auto md:mx-0 text-center">
      <label className="mb-2 block font-medium">Ảnh đại diện</label>
      <img
        src={selectedFile ? avatarImg : getImageUrl(avatarImg, "avatar")}
        alt="avatar image"
        className="object-cover size-40 rounded-full mx-auto"
      />
      <div className="flex flex-col gap-2 text-xs mx-auto mt-3">
        <Button
          disabled={isPending}
          onClick={() => fileInputRef.current?.click()}
          variant="outline"
          className="flex items-center justify-center gap-2.5 hover:cursor-pointer"
        >
          <i className="fa-solid fa-arrow-up-from-bracket"></i>
          <p className="font-semibold">Tải ảnh lên</p>
        </Button>
        {selectedFile && (
          <>
            <Button
              disabled={isPending}
              onClick={handleUndo}
              variant="outline"
              className="flex items-center justify-center gap-2.5 text-xs hover:cursor-pointer"
            >
              <i className="fa-solid fa-arrow-rotate-left"></i>
              <p className="font-semibold">Hoàn tác</p>
            </Button>
            <Button
              disabled={isPending}
              onClick={handleSave}
              variant="outline"
              className="text-xs font-semibold hover:cursor-pointer"
            >
              {isPending ? "Đang lưu..." : "Lưu thay đổi"}
            </Button>
          </>
        )}
      </div>
      <input
        type="file"
        ref={fileInputRef}
        accept="image/*"
        className="hidden"
        onChange={handleFileChange}
      />
    </div>
  );
}

export default AvatarUpload;
