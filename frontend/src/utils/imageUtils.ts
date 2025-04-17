import { noAvatar, noCover } from "@/assets/images";

export default function getImageUrl(
  imagePath: string | null | undefined,
  type: "cover" | "avatar"
): string {
  const baseUrl = (import.meta.env.VITE_BASE_URL_IMAGE || "").replace(
    /\/$/,
    ""
  ); // Remove trailing slash
  const fallbackImage = type === "cover" ? noCover : noAvatar;

  return imagePath ? `${baseUrl}/${imagePath}` : fallbackImage;
}

export const resizeImage = (
  file: File,
  maxWidth: number,
  maxHeight: number
): Promise<File | null> => {
  return new Promise((resolve) => {
    const img = new Image();
    const reader = new FileReader();

    reader.onload = (e) => {
      if (!e.target?.result) return resolve(null);

      img.src = e.target.result as string;
      img.onload = () => {
        const canvas = document.createElement("canvas");
        let { width, height } = img;
        const aspectRatio = width / height;

        if (width > maxWidth || height > maxHeight) {
          if (width / height > maxWidth / maxHeight) {
            width = maxWidth;
            height = width / aspectRatio;
          } else {
            height = maxHeight;
            width = height * aspectRatio;
          }
        }

        canvas.width = width;
        canvas.height = height;
        const ctx = canvas.getContext("2d");

        if (!ctx) return resolve(null);

        ctx.drawImage(img, 0, 0, width, height);
        canvas.toBlob((blob) => {
          if (!blob) return resolve(null);
          resolve(new File([blob], file.name, { type: file.type }));
        }, file.type);
      };
    };

    reader.readAsDataURL(file);
  });
};
