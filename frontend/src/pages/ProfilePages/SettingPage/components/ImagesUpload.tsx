import { useEffect, useRef, useState } from "react";
import { useController } from "react-hook-form";
import Button from "@/components/Button/Button";
import IconBtn from "@/components/Button/IconBtn";
import getImageUrl from "@/utils/imageUtils";

interface ImageUploadProps {
  existingImages?: string[];
  onRemoveAllExisting?: () => void;
  control: any;
  name: string;
  limit?: number;
}

function ImageUpload({
  existingImages = [],
  onRemoveAllExisting,
  control,
  name,
  limit = 10,
}: ImageUploadProps) {
  const {
    field: { value: newFiles, onChange },
    fieldState: { error },
  } = useController({ name, control, defaultValue: [] });

  const fileInputRef = useRef<HTMLInputElement | null>(null);
  const [previewUrls, setPreviewUrls] = useState<string[]>([]);

  // Generate object URLs when files change
  useEffect(() => {
    const urls = newFiles.map((file: File) => URL.createObjectURL(file));
    setPreviewUrls(urls);

    return () => {
      urls.forEach((url: string) => URL.revokeObjectURL(url));
    };
  }, [newFiles]);

  const handleImageChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    if (e.target.files) {
      const selectedFiles = Array.from(e.target.files).slice(
        0,
        limit - newFiles.length
      );
      onChange([...newFiles, ...selectedFiles]);
    }
    if (fileInputRef.current) fileInputRef.current.value = "";
  };

  const handleRemoveAll = () => {
    previewUrls.forEach((url) => URL.revokeObjectURL(url));

    if (onRemoveAllExisting) {
      onRemoveAllExisting(); // Remove existing images
    }
    onChange([]); // Clear new file list
    setPreviewUrls([]); // Clear stored URLs
  };

  return (
    <div>
      <label className="mb-2 block font-medium">Hình ảnh (10)</label>

      <div className="grid grid-cols-4 gap-3">
        {/* Existing images */}
        {existingImages.map((image, index) => (
          <div key={`existing-${index}`} className="w-full">
            <img
              src={getImageUrl(image, "cover")}
              alt={`Existing ${index + 1}`}
              className="object-cover w-full h-36 rounded-lg"
            />
          </div>
        ))}

        {/* New uploaded image previews */}
        {newFiles.map((_: File, index: number) => (
          <div key={`new-${index}`} className="w-full">
            <img
              src={previewUrls[index]}
              alt={`Preview ${index + 1}`}
              className="object-cover w-full h-36 rounded-lg"
            />
          </div>
        ))}
      </div>

      <div className="mt-3 flex gap-3">
        <Button
          variant="outline"
          onClick={() => fileInputRef.current?.click()}
          disabled={existingImages.length > 0 || newFiles.length >= limit}
          className="text-xs font-medium"
        >
          Tải ảnh ({newFiles.length}/{limit})
        </Button>

        {existingImages.length + newFiles.length > 0 && (
          <IconBtn
            icon="fa-trash"
            className="hover:text-red-500 hover:cursor-pointer"
            onClick={handleRemoveAll}
          />
        )}
      </div>

      <input
        type="file"
        ref={fileInputRef}
        accept="image/*"
        multiple
        className="hidden"
        onChange={handleImageChange}
        disabled={existingImages.length > 0 || newFiles.length >= limit}
      />

      {error && <p className="text-xs text-red-500 mt-1.5">{error.message}</p>}
    </div>
  );
}

export default ImageUpload;
