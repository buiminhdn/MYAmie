import { useEffect, useState } from "react";
import { useSelector } from "react-redux";
import FilterBtn from "@/components/Button/FilterBtn";
import UserCard from "@/components/Cards/UserCard";
import CategoryContainer from "@/components/CategoryItem/CategoryContainer";
import FilterByDistance from "@/components/Filters/FilterByDistance";
import Loader from "@/components/Loader/Loader";
import Pagination from "@/components/Pagination/Pagination";
import Title from "@/components/Title/Title";
import { FilterUserParams } from "@/models/params/user.param";
import { useAllGetUsers } from "@/services/user.service";
import { handleError } from "@/utils/errorUtils";
import {
  accountLatitudeSelector,
  accountLongitudeSelector,
} from "@/store/auth/auth.selector";
import { useUpdateLocation } from "@/services/account.service";

function UsersPage() {
  const storedLatitude = useSelector(accountLatitudeSelector);
  const storedLongitude = useSelector(accountLongitudeSelector);
  const [isLocationUpdated, setIsLocationUpdated] = useState(false);

  const [location, setLocation] = useState<{
    latitude?: number;
    longitude?: number;
  }>({
    latitude: storedLatitude,
    longitude: storedLongitude,
  });

  const [params, setParams] = useState<FilterUserParams>({
    pageNumber: 1,
    pageSize: 20,
  });
  const [tempDistance, setTempDistance] = useState<number | undefined>();

  const updateLocationMutation = useUpdateLocation();

  // Prioritize browser geolocation first, fallback to stored location
  useEffect(() => {
    navigator.geolocation.getCurrentPosition(
      (position) => {
        const newLocation = {
          latitude: position.coords.latitude,
          longitude: position.coords.longitude,
        };
        setLocation(newLocation);
        if (!isLocationUpdated) {
          updateLocationMutation.mutate(newLocation, {
            onSuccess: () => setIsLocationUpdated(true),
          });
        }
      },
      () => {
        if (storedLatitude && storedLongitude) {
          setLocation({ latitude: storedLatitude, longitude: storedLongitude });
        }
      }
    );
  }, [storedLatitude, storedLongitude]);

  // Set params only when valid location is available
  useEffect(() => {
    if (location.latitude && location.longitude) {
      setParams((prev) => ({
        ...prev,
        latitude: location.latitude,
        longitude: location.longitude,
      }));
    }
  }, [location]);

  // Fetch users only if params exist
  const { data, isLoading, isError, error } = useAllGetUsers(params);
  const pagination = data?.data?.pagination;
  const users = data?.data?.users || [];

  const updateParams = (newParams: Partial<FilterUserParams>) =>
    setParams((prev) => ({ ...prev, ...newParams }));

  useEffect(() => {
    setParams((prev) => ({ ...prev, pageNumber: 1 }));
  }, [
    params.categoryId,
    params.distanceInKm,
    params.latitude,
    params.longitude,
  ]);

  let content;
  if (!location.latitude || !location.longitude) {
    content = (
      <p className="error">
        Không thể xác định vị trí của bạn. Vui lòng bật GPS hoặc cung cấp vị
        trí.
      </p>
    );
  } else if (isLoading) {
    content = <Loader className="my-8" />;
  } else if (isError) {
    content = <p className="error">{handleError(error)}</p>;
  } else if (users.length === 0) {
    content = (
      <p className="text-center text-gray-600">
        Không có người dùng nào được tìm thấy.
      </p>
    );
  } else {
    content = (
      <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
        {users.map((user) => (
          <UserCard key={user.id} user={user} />
        ))}
      </div>
    );
  }

  return (
    <div className="mt-10">
      <Title
        subTitle="Hơn 4200 bạn bè gần đây"
        title="Tìm bạn bè quanh đây"
        description="Kết nối với những người bạn mới, khám phá các địa điểm và trải nghiệm dịch vụ tuyệt vời cùng nhau"
      />

      <CategoryContainer
        onSelect={(categoryId) => updateParams({ categoryId })}
      />

      <div className="mt-10">
        <div className="flex justify-between items-end mb-4">
          <p className="font-semibold text-base">
            Danh sách {pagination?.totalCount} bạn gần đây
          </p>
          <FilterBtn
            onActiveClick={() => updateParams({ distanceInKm: tempDistance })}
            onInactiveClick={() => {
              setTempDistance(undefined);
              updateParams({ distanceInKm: undefined });
            }}
          >
            <FilterByDistance
              currentDistance={tempDistance || 5}
              onChange={setTempDistance}
            />
          </FilterBtn>
        </div>
        {content}
      </div>

      <Pagination
        currentPage={params?.pageNumber || 1}
        totalPage={pagination?.totalPages}
        onPageChange={(pageNumber) => updateParams({ pageNumber })}
      />
    </div>
  );
}

export default UsersPage;
