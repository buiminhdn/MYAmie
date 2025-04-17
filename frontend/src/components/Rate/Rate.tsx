interface RateProps {
  rate: number;
}

function Rate({ rate }: RateProps) {
  const fullStars = Math.floor(rate);
  const hasHalfStar = rate % 1 >= 0.5; // Check for half-star

  return (
    <div
      className="text-yellow-400 space-x-1 flex"
      aria-label={`Rating: ${rate} out of 5`}
    >
      {Array.from({ length: 5 }).map((_, index) => {
        if (index < fullStars) {
          return <i key={index} className="fa-solid fa-star"></i>; // Full star
        } else if (index === fullStars && hasHalfStar) {
          return <i key={index} className="fa-solid fa-star-half-alt"></i>; // Half star
        } else {
          return <i key={index} className="fa-regular fa-star"></i>; // Empty star
        }
      })}
    </div>
  );
}

export default Rate;
