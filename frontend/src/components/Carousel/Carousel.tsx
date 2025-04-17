import { useState, useCallback, memo } from "react";

interface CarouselProps {
  images: string[];
}

function Carousel({ images }: CarouselProps) {
  const [currentSlide, setCurrentSlide] = useState(0);
  const totalSlides = images.length;

  const goToNextSlide = useCallback(() => {
    setCurrentSlide((prev) => (prev === totalSlides - 1 ? 0 : prev + 1));
  }, [totalSlides]);

  const goToPrevSlide = useCallback(() => {
    setCurrentSlide((prev) => (prev === 0 ? totalSlides - 1 : prev - 1));
  }, [totalSlides]);

  const goToSlide = useCallback((index: number) => {
    setCurrentSlide(index);
  }, []);

  if (totalSlides === 0) return null;

  return (
    <div className="relative w-full h-full overflow-hidden">
      <div className="relative w-full h-full">
        {images.map((image, index) => (
          <img
            src={image}
            key={image} // Using image URL as key is better if URLs are unique
            alt={`Slide ${index + 1}`} // Added for accessibility
            className={`absolute inset-0 w-full h-full object-cover rounded-xl transition-opacity duration-700 ${
              currentSlide === index
                ? "opacity-100"
                : "opacity-0 pointer-events-none"
            }`}
            loading="lazy" // Improve performance
          />
        ))}
      </div>

      {totalSlides > 1 && (
        <div className="absolute bottom-8 left-0 right-0 flex items-center justify-center gap-4 md:gap-12">
          <button
            onClick={goToPrevSlide}
            className="text-white hover:scale-110 transition-transform hover:cursor-pointer"
            aria-label="Previous slide"
          >
            <i className="fa-2xl fa-regular fa-arrow-left-long"></i>
          </button>

          <div className="flex gap-1 md:gap-2">
            {images.map((_, index) => (
              <button
                key={index}
                onClick={() => goToSlide(index)}
                className={`${
                  currentSlide === index ? "bg-white w-12" : "bg-white/30 w-8"
                } h-1 rounded-full transition-all duration-300`}
                aria-label={`Go to slide ${index + 1}`}
              />
            ))}
          </div>

          <button
            onClick={goToNextSlide}
            className="text-white hover:scale-110 transition-transform hover:cursor-pointer"
            aria-label="Next slide"
          >
            <i className="fa-2xl fa-regular fa-arrow-right-long"></i>
          </button>
        </div>
      )}
    </div>
  );
}

export default memo(Carousel);
