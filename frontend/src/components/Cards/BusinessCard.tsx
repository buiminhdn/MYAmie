import { Link } from "react-router-dom";
import Avatar from "../Avatar/Avatar";
import Rate from "../Rate/Rate";
import IconText from "../IconText/IconText";
import { BusinessVM } from "@/models/viewmodels/user.vm";
import getImageUrl from "@/utils/imageUtils";
import { noCover } from "@/assets/images";

function BusinessCard({ business }: { business: BusinessVM }) {
  return (
    <Link
      to={`/service/${business.id}`}
      className="business-card flex flex-col border-2 border-gray-200 rounded-lg relative transition-shadow duration-300 hover:shadow-xl bg-white"
      aria-label={`Business card for ${business.name}`}
    >
      <img
        src={getImageUrl(business.cover, "cover")}
        alt={business.name}
        className="w-full min-h-40 h-40 object-cover rounded-t-md"
        loading="lazy"
        onError={(e) => (e.currentTarget.src = noCover)}
      />
      <div className="flex flex-col h-full justify-between p-4">
        <div>
          <Avatar
            src={business.avatar}
            alt={business.name}
            className="-mt-14"
          />
          <p className="text-lg font-semibold mt-1 truncate">{business.name}</p>
          <p className="line-clamp-2 text-gray-600 mt-1">
            {business.shortDescription || "Mô tả đang được cập nhật."}
          </p>
        </div>
        <div>
          <div className="flex items-center flex-wrap gap-2 mt-3">
            <Rate rate={business.averageRating} />
            <p className="text-gray-500">{business.totalFeedback} Đánh giá</p>
          </div>
          <div className="flex flex-wrap gap-3 justify-between border-t border-gray-200 mt-5 pt-4">
            {business.city && (
              <IconText
                icon="fa-location-dot"
                text={business.city}
                textClasses="text-xs"
              />
            )}
            {business.operatingHours ? (
              <IconText
                icon="fa-clock"
                text={business.operatingHours}
                textClasses="text-xs"
              />
            ) : (
              <IconText
                icon="fa-clock"
                text="Chưa cập nhật"
                textClasses="text-xs"
              />
            )}
          </div>
        </div>
      </div>
    </Link>
  );
}

export default BusinessCard;
