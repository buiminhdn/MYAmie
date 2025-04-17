import Avatar from "@/components/Avatar/Avatar";
import IconText from "@/components/IconText/IconText";
import { Tab, TabList, TabPanel, Tabs } from "react-tabs";
import { Link, useParams } from "react-router-dom";
import { useGetPlaceById } from "@/services/place.service";
import Loader from "@/components/Loader/Loader";
import Cover from "@/components/Cover/Cover";
import CategoryItem from "@/components/CategoryItem/CategoryItem";
import getImageUrl from "@/utils/imageUtils";
import { handleError } from "@/utils/errorUtils";
import UpdateDeletePlace from "./components/UpdateDeletePlace";
import { useSelector } from "react-redux";
import { accountIdSelector } from "@/store/auth/auth.selector";

function PlaceDetailPage() {
  const { id } = useParams();
  const curentUserId = useSelector(accountIdSelector);
  const { data, isLoading, isError, error } = useGetPlaceById(Number(id));
  const place = data?.data;

  if (isLoading) return <Loader className="mt-10" />;
  if (isError) return <p className="error mt-10">{handleError(error)}</p>;
  if (!place) return <p className="error mt-10">Không tìm thấy địa điểm</p>;

  return (
    <div>
      <div className="relative">
        <Cover src={place.images?.[0]} alt={place.name} />
        {place.ownerId === curentUserId && (
          <div className="absolute right-5 bottom-5 flex gap-2 items-center">
            <UpdateDeletePlace place={place} />
          </div>
        )}
      </div>
      <div className="mx-5 lg:mx-10 mt-7">
        <h1 className="text-3xl font-semibold text-center lg:text-left break-words">
          {place.name}
        </h1>
        <p className="mt-1 text-gray-500 text-center lg:text-left">
          {place.shortDescription}
        </p>

        <div className="grid grid-cols-1 lg:grid-cols-3 mt-4">
          {/* Left Section */}
          <aside className="col-span-1 mt-3 lg:mr-10">
            {/* Categories */}
            <div className="flex justify-center lg:justify-start flex-wrap gap-2">
              {place.categories.map((category) => (
                <CategoryItem key={category.id} category={category} />
              ))}
            </div>

            {/* Owner & Details */}
            <div className="border-2 border-gray-200 rounded-xl p-5 flex flex-col gap-5 mt-7 bg-white">
              {/* Owner Info */}
              <section>
                <h3 className="font-medium text-gray-400">NGƯỜI ĐĂNG</h3>
                <div className="flex items-center gap-2 truncate mt-3">
                  <Avatar
                    src={place.ownerAvatar}
                    alt={place.ownerName}
                    size="size-10"
                    hasBorder={false}
                  />
                  <div className="truncate">
                    <Link
                      to={`/user/${place.ownerId}`}
                      className="font-medium truncate hover:underline"
                    >
                      {place.ownerName}
                    </Link>
                    <p className="text-xs font-medium text-gray-400 mt-0.5">
                      {place.dateCreated}
                    </p>
                  </div>
                </div>
              </section>

              <div className="border-t w-full border-gray-300"></div>

              {/* Detailed Info */}
              <section>
                <h3 className="font-medium text-gray-400">
                  THÔNG TIN CHI TIẾT
                </h3>
                <div className="mt-3 flex flex-col gap-2.5">
                  {place.address && (
                    <IconText
                      icon="fa-location-dot"
                      text={place.address}
                      className="font-medium"
                      iconClasses="w-5"
                    />
                  )}
                  {place.city && (
                    <IconText
                      icon="fa-city"
                      text={place.city.name}
                      className="font-medium"
                      iconClasses="w-5"
                    />
                  )}
                </div>
              </section>
            </div>
          </aside>

          {/* Right Section (Tabs) */}
          <main className="mt-7 lg:mt-0 col-span-2">
            <Tabs>
              <TabList className="flex text-gray-600">
                <Tab className="cursor-pointer py-2 px-5 transition-colors duration-500">
                  Mô tả
                </Tab>
                <Tab className="cursor-pointer py-2 px-5 transition-colors duration-500">
                  Ảnh chi tiết
                </Tab>
              </TabList>

              {/* Description Tab */}
              <TabPanel className="mt-4">
                {place.description ? (
                  <p className="whitespace-pre-line">{place.description}</p>
                ) : (
                  <p>Chưa có mô tả</p>
                )}
              </TabPanel>

              {/* Images Tab */}
              <TabPanel className="mt-4 grid grid-cols-1 sm:grid-cols-2 gap-3">
                {place.images && place.images.length > 0 ? (
                  place.images.map((image, index) => (
                    <img
                      key={index}
                      src={getImageUrl(image, "cover")}
                      alt={`${place.name} - ${index + 1}`}
                      className="w-full h-64 object-cover rounded-lg"
                      loading="lazy"
                    />
                  ))
                ) : (
                  <p>Chưa có ảnh nào</p>
                )}
              </TabPanel>
            </Tabs>
          </main>
        </div>
      </div>
    </div>
  );
}

export default PlaceDetailPage;
