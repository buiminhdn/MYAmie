import { useParams } from "react-router-dom";
import { useGetUserById } from "@/services/user.service";
import Loader from "@/components/Loader/Loader";
import { useSelector } from "react-redux";
import { accountIdSelector } from "@/store/auth/auth.selector";
import Avatar from "@/components/Avatar/Avatar";
import IconText from "@/components/IconText/IconText";
import { Tab, Tabs, TabList, TabPanel } from "react-tabs";
import Cover from "@/components/Cover/Cover";
import NameWCategories from "@/components/NameWCategories/NameWCategories";
import CharacteristicItem from "@/components/CharacteristicItem/CharacteristicItem";
import UserFriends from "./UserFriends";
import getImageUrl from "@/utils/imageUtils";
import CheckinPlaces from "./components/CheckinPlaces";
import { handleError } from "@/utils/errorUtils";
import { FriendshipButton } from "./components/FriendshipButton";

function UserDetailPage() {
  const { id } = useParams();
  const currentUserId = useSelector(accountIdSelector);
  const { data, isLoading, isError, error } = useGetUserById(Number(id));
  const user = data?.data;

  if (isLoading) return <Loader className="mt-10" />;
  if (isError) return <p className="error mt-10">{handleError(error)}</p>;
  if (!user) return <p className="error mt-10">Không tìm thấy người dùng</p>;

  return (
    <div>
      <div className="relative">
        <Cover src={user.cover} alt={user.name} />
        {user.id !== currentUserId && (
          <div className="absolute bottom-4 right-4 shadow-lg">
            <FriendshipButton otherUserId={user.id} />
          </div>
        )}
      </div>
      <div className="mx-5 lg:mx-10">
        <Avatar
          src={user.avatar}
          alt={user.name}
          size="size-40"
          className="-mt-24 border-8 mx-auto lg:mx-0"
        />
        <div className="grid grid-cols-1 lg:grid-cols-3">
          <div className="lg:col-span-1 mt-3 lg:mr-10">
            <NameWCategories
              name={user.name}
              shortDescription={user.shortDescription}
              categories={user.categories}
            />
            <div className="border-2  border-gray-200 rounded-xl p-5 mt-7 flex flex-col gap-5 bg-white">
              <div>
                <p className="font-medium text-gray-400">THÔNG TIN CHI TIẾT</p>
                <div className="mt-3 flex flex-col gap-2.5">
                  {user.city && (
                    <IconText
                      icon="fa-location-dot"
                      text={user.city}
                      className="font-medium"
                      iconClasses="w-5 justify-center"
                    />
                  )}
                </div>
              </div>
              {user.characteristics?.length > 0 && (
                <>
                  <div className="border-t border-gray-300 w-full" />
                  <div>
                    <p className="font-medium text-gray-400">TÍNH CÁCH</p>
                    <div className="mt-3 flex flex-wrap gap-2">
                      {user.characteristics.map((char, index) => (
                        <CharacteristicItem key={index} text={char} />
                      ))}
                    </div>
                  </div>
                </>
              )}
            </div>
          </div>
          <div className="mt-7 lg:mt-0 lg:col-span-2">
            <Tabs>
              <TabList className="flex flex-wrap text-gray-600">
                <Tab className="cursor-pointer py-2 px-5">Mô tả</Tab>
                <Tab className="cursor-pointer py-2 px-5">Ảnh chi tiết</Tab>
                {currentUserId === user.id && (
                  <Tab className="cursor-pointer py-2 px-5">Bạn bè</Tab>
                )}
              </TabList>
              <TabPanel className="mt-4">
                {user.description ? (
                  <p className="whitespace-pre-line">{user.description}</p>
                ) : (
                  <p>Chưa có mô tả</p>
                )}
              </TabPanel>
              <TabPanel className="mt-4 grid grid-cols-1 sm:grid-cols-2 gap-3">
                {user.images.length !== 0 ? (
                  user.images.map((image, index) => (
                    <img
                      key={index}
                      src={getImageUrl(image, "cover")}
                      alt={user.name}
                      className="w-full h-64 object-cover rounded-lg"
                    />
                  ))
                ) : (
                  <p>Chưa có ảnh</p>
                )}
              </TabPanel>
              {currentUserId === user.id && (
                <TabPanel>
                  <UserFriends />
                </TabPanel>
              )}
            </Tabs>
          </div>
        </div>
      </div>

      <div className="mx-5 lg:mx-10 mt-20">
        <CheckinPlaces userName={user.name} userId={user.id} />
      </div>
    </div>
  );
}

export default UserDetailPage;
