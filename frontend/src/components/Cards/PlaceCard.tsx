import { Link } from "react-router-dom";
import Avatar from "../Avatar/Avatar";
import IconText from "../IconText/IconText";
import { PlaceVM } from "@/models/viewmodels/place.vm";
import getImageUrl from "@/utils/imageUtils";

interface PlaceCardProps {
  place: PlaceVM;
  isOwner?: boolean;
}

function PlaceCard({ place, isOwner = false }: PlaceCardProps) {
  return (
    <Link
      to={`/place/${place.id}`}
      className="place-card flex flex-col border-2 border-gray-200  rounded-lg relative transition-shadow duration-300 hover:shadow-xl bg-white"
    >
      <img
        src={getImageUrl(place.cover, "cover")}
        alt={place.name}
        className="w-full min-h-40 h-40 object-cover rounded-t-md"
      />
      <div className="p-4 h-full flex flex-col justify-between">
        <div>
          <p className="text-lg font-semibold truncate">{place.name}</p>
          <p className="line-clamp-3 text-gray-500 leading-snug mt-1">
            {place.shortDescription}
          </p>
        </div>
        <div className="flex flex-wrap justify-between items-end mt-5 gap-5">
          <div className="flex items-center gap-2 truncate">
            {!isOwner && (
              <Avatar
                hasBorder={false}
                src={place.ownerAvatar}
                alt={place.ownerName}
                size="size-11"
              />
            )}
            <div className="truncate">
              <p className="font-medium truncate">{place.ownerName}</p>
              <p className="text-xs font-medium text-gray-400 mt-0.5">
                {place.dateCreated}
              </p>
            </div>
          </div>
          <IconText
            icon="fa-location-dot"
            text={place.city}
            textClasses="text-xs"
          />
        </div>
      </div>
    </Link>
  );
}

export default PlaceCard;
