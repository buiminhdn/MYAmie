import IconBtn from "@/components/Button/IconBtn";
import getImageUrl from "@/utils/imageUtils";

interface ImagesDisplayProps {
  images?: string[];
  onRemoveAll: () => void;
}

function ImagesDisplay({ images = [], onRemoveAll }: ImagesDisplayProps) {
  return (
    <div>
      <label className="mb-2 block font-medium">
        Ảnh hiện tại ({images.length})
      </label>
      <div className="grid grid-cols-4 gap-3">
        {images.map((image, index) => (
          <div key={index} className="w-full h-24">
            <img
              src={getImageUrl(image, "cover")}
              alt={`Displayed preview ${index + 1}`}
              className="object-cover w-full h-full rounded-lg"
            />
          </div>
        ))}
      </div>
      {images.length > 0 && (
        <IconBtn
          effect="opacity"
          icon="fa-trash"
          onClick={onRemoveAll}
          className="mt-3"
        />
      )}
    </div>
  );
}

export default ImagesDisplay;
