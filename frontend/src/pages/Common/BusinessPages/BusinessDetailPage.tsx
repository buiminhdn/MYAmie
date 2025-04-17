import Avatar from "@/components/Avatar/Avatar";
import IconText from "@/components/IconText/IconText";
import { Tab, TabList, TabPanel, Tabs } from "react-tabs";
import { useParams } from "react-router-dom";
import Loader from "@/components/Loader/Loader";
import Cover from "@/components/Cover/Cover";
import NameWCategories from "@/components/NameWCategories/NameWCategories";
import getImageUrl from "@/utils/imageUtils";
import { useGetBusinessById } from "@/services/user.service";
import FeedbackTab from "@/components/FeedbackItems/FeedbackTab";
import { handleError } from "@/utils/errorUtils";

function BussinessDetailPage() {
  const { id } = useParams();
  const { data, isLoading, isError, error } = useGetBusinessById(Number(id));
  const business = data?.data;

  if (isLoading) return <Loader className="mt-10" />;
  if (isError) return <p className="error mt-10">{handleError(error)}</p>;
  if (!business) return <p className="error mt-10">Không tìm thấy dịch vụ</p>;

  return (
    <div>
      <Cover src={business.cover} alt={business.name} />
      <div className="mx-5 lg:mx-10">
        <Avatar
          src={business.avatar}
          alt={business.name}
          size="size-40"
          className="-mt-24 border-8 mx-auto lg:mx-0"
        />
        <div className="grid grid-cols-1 lg:grid-cols-3">
          <div className="col-span-1 mt-3 lg:mr-10">
            <NameWCategories
              name={business.name}
              shortDescription={business.shortDescription}
              categories={business.categories}
            />
            <div className="border-2 border-gray-200 rounded-xl p-5 flex flex-col gap-5 mt-7 bg-white">
              <div>
                <p className="font-medium text-gray-400">THÔNG TIN CHI TIẾT</p>
                <div className="mt-3 flex flex-col gap-2.5">
                  {business.phone && (
                    <IconText
                      icon="fa-phone"
                      text={business.phone}
                      className="font-medium"
                      iconClasses="w-5"
                    />
                  )}
                  {business.address && (
                    <IconText
                      icon="fa-location-arrow"
                      text={business.address}
                      className="font-medium"
                      iconClasses="w-5"
                    />
                  )}
                  {business.city && (
                    <IconText
                      icon="fa-location-dot"
                      text={business.city}
                      className="font-medium"
                      iconClasses="w-5"
                    />
                  )}
                  {business.operatingHours && (
                    <IconText
                      icon="fa-clock"
                      text={`${business.operatingHours}h`}
                      className="font-medium"
                      iconClasses="w-5"
                    />
                  )}
                </div>
              </div>
            </div>
          </div>
          <div className="mt-7 lg:mt-0 col-span-2">
            <Tabs>
              <TabList className="flex text-gray-600">
                <Tab className="cursor-pointer py-2 px-5 transition-colors duration-500">
                  Mô tả
                </Tab>
                <Tab className="cursor-pointer py-2 px-5 transition-colors duration-500">
                  Ảnh chi tiết
                </Tab>
                <Tab className="cursor-pointer py-2 px-5 transition-colors duration-500">
                  Đánh giá
                </Tab>
              </TabList>

              <TabPanel className="mt-4">
                {business.description ? (
                  <p className="whitespace-pre-line">{business.description}</p>
                ) : (
                  <p>Chưa có mô tả</p>
                )}
              </TabPanel>
              <TabPanel className="mt-4 grid grid-cols-1 sm:grid-cols-2 gap-3">
                {business.images.length !== 0 ? (
                  business.images.map((image, index) => (
                    <img
                      key={index}
                      src={getImageUrl(image, "cover")}
                      alt={business.name}
                      className="w-full h-64 object-cover rounded-lg"
                    />
                  ))
                ) : (
                  <p>Chưa có ảnh</p>
                )}
              </TabPanel>
              <TabPanel>
                <FeedbackTab id={business.id} />
              </TabPanel>
            </Tabs>
          </div>
        </div>
      </div>
    </div>
  );
}

export default BussinessDetailPage;
