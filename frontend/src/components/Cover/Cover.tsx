import getImageUrl from "@/utils/imageUtils";

interface CoverProps {
  src: string;
  alt: string;
}

const Cover: React.FC<CoverProps> = ({ src, alt }) => (
  <img
    src={getImageUrl(src, "cover")}
    alt={alt}
    className="w-full h-64 md:h-80 object-cover rounded-b-2xl"
    loading="lazy"
  />
);

export default Cover;
