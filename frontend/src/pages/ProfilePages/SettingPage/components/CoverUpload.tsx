import Button from "@/components/Button/Button";
import { ImageTypeParam } from "@/models/params/account.param";
import { useUpdateAvatarOrCover } from "@/services/account.service";
import getImageUrl, { resizeImage } from "@/utils/imageUtils";
import { useEffect, useRef, useState } from "react";

interface CoverUploadProps {
  image?: string;
}

function CoverUpload({ image }: CoverUploadProps) {
  const [coverImg, setCoverImg] = useState<string>(image || "");
  const [selectedFile, setSelectedFile] = useState<File | null>(null);
  const fileInputRef = useRef<HTMLInputElement | null>(null);
  const prevObjectUrl = useRef<string | null>(null);

  const { isPending, mutateAsync } = useUpdateAvatarOrCover();

  useEffect(() => {
    setCoverImg(image || "");
  }, [image]);

  const handleFileChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const file = event.target.files?.[0];
    if (!file) return;

    if (prevObjectUrl.current) {
      URL.revokeObjectURL(prevObjectUrl.current);
    }

    const objectUrl = URL.createObjectURL(file);
    prevObjectUrl.current = objectUrl;
    setCoverImg(objectUrl);
    setSelectedFile(file);

    fileInputRef.current && (fileInputRef.current.value = "");
  };

  const handleUndo = () => {
    if (prevObjectUrl.current) {
      URL.revokeObjectURL(prevObjectUrl.current);
      prevObjectUrl.current = null;
    }
    setCoverImg(image || "");
    setSelectedFile(null);
  };

  const handleSave = async () => {
    if (!selectedFile) return;
    try {
      const resizedFile = await resizeImage(selectedFile, 4000, 2000);
      if (!resizedFile) return;

      await mutateAsync({
        type: ImageTypeParam.Cover,
        imageFile: resizedFile,
      });
      if (prevObjectUrl.current) {
        URL.revokeObjectURL(prevObjectUrl.current);
        prevObjectUrl.current = null;
      }
      setCoverImg(image || "");
      setSelectedFile(null);
    } catch (error) {}
  };

  return (
    <div className="relative">
      <label className="mb-2 block font-medium">Ảnh bìa (1400 x 320px)</label>
      <img
        src={selectedFile ? coverImg : getImageUrl(coverImg, "cover")}
        alt="cover image"
        className="object-cover h-48 sm:h-64 w-full rounded-lg"
      />
      <div className="absolute bottom-3 right-3 flex gap-2 justify-center flex-wrap">
        <Button
          disabled={isPending}
          onClick={() => fileInputRef.current?.click()}
          variant="outline"
          className="flex items-center gap-2.5 text-xs hover:cursor-pointer"
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
              className="flex items-center gap-2.5 text-xs hover:cursor-pointer"
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

export default CoverUpload;
