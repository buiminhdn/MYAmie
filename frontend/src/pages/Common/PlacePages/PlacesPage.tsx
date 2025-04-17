import PlaceCard from "@/components/Cards/PlaceCard";
import CategoryContainer from "@/components/CategoryItem/CategoryContainer";
import FilterByCities from "@/components/Filters/FilterByCities";
import Loader from "@/components/Loader/Loader";
import Pagination from "@/components/Pagination/Pagination";
import SearchBar from "@/components/SearchBar/SearchBar";
import Title from "@/components/Title/Title";
import { FilterPlaceParams } from "@/models/params/place.param";
import { useGetAllPlaces } from "@/services/place.service";
import { handleError } from "@/utils/errorUtils";
import { useEffect, useState } from "react";

function PlacesPage() {
  const [params, setParams] = useState<FilterPlaceParams>({
    pageNumber: 1,
    pageSize: 20,
  });
  const [tempCityId, setTempCityId] = useState<number>();

  const { data, isLoading, isError, error } = useGetAllPlaces(params);
  const pagination = data?.data?.pagination;
  const businesses = data?.data?.places || [];

  const updateParams = (newParams: Partial<FilterPlaceParams>) =>
    setParams((prev) => ({ ...prev, ...newParams }));

  useEffect(() => {
    setParams((prev) => ({
      ...prev,
      pageNumber: 1,
    }));
  }, [params.searchTerm, params.cityId, params.categoryId]);

  let content;
  if (isLoading) {
    content = <Loader className="my-8" />;
  } else if (isError) {
    content = <p className="error">{handleError(error)}</p>;
  } else if (businesses.length === 0) {
    content = (
      <p className="text-center text-gray-600">
        Không có địa điểm nào được tìm thấy.
      </p>
    );
  } else {
    content = (
      <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 xl:grid-cols-4 gap-6">
        {businesses.map((place) => (
          <PlaceCard key={place.id} place={place} />
        ))}
      </div>
    );
  }

  return (
    <div className="mt-10">
      <Title
        subTitle="Hơn 2256 địa điểm gần đây"
        title="Các địa điểm mới nhất"
        description="Khám phá những địa điểm hấp dẫn, tận hưởng những trải nghiệm mới cùng những người bạn"
      />

      <SearchBar
        onSearch={(searchTerm) => updateParams({ searchTerm })}
        onActiveClick={() => updateParams({ cityId: tempCityId ?? 0 })}
        onInactiveClick={() => {
          setTempCityId(undefined);
          updateParams({ cityId: 0 });
        }}
      >
        <FilterByCities
          currentCityId={params.cityId}
          onCitySelect={setTempCityId}
          onClear={() => setTempCityId(undefined)}
        />
      </SearchBar>

      <CategoryContainer
        onSelect={(categoryId) => updateParams({ categoryId })}
      />

      <div className="mt-10">
        <p className="font-medium text-base mb-4">
          Danh sách {pagination?.totalCount} địa điểm
        </p>

        {content}
      </div>

      <Pagination
        currentPage={params.pageNumber || 1}
        totalPage={pagination?.totalPages}
        onPageChange={(pageNumber) => updateParams({ pageNumber })}
      />
    </div>
  );
}

export default PlacesPage;
