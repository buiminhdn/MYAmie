import BusinessCard from "@/components/Cards/BusinessCard";
import CategoryContainer from "@/components/CategoryItem/CategoryContainer";
import FilterByCities from "@/components/Filters/FilterByCities";
import Loader from "@/components/Loader/Loader";
import Pagination from "@/components/Pagination/Pagination";
import SearchBar from "@/components/SearchBar/SearchBar";
import Title from "@/components/Title/Title";
import { FilterBusinessParams } from "@/models/params/user.param";
import { useGetAllBusinesses } from "@/services/user.service";
import { handleError } from "@/utils/errorUtils";
import { useEffect, useState } from "react";

function BusinessesPage() {
  const [params, setParams] = useState<FilterBusinessParams>({
    pageNumber: 1,
    pageSize: 20,
  });
  const [tempCityId, setTempCityId] = useState<number>();

  const { data, isLoading, isError, error } = useGetAllBusinesses(params);
  const pagination = data?.data?.pagination;
  const businesses = data?.data?.businesses || [];

  const updateParams = (newParams: Partial<FilterBusinessParams>) =>
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
        Không có dịch vụ nào được tìm thấy.
      </p>
    );
  } else {
    content = (
      <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 xl:grid-cols-4 gap-6">
        {businesses.map((business) => (
          <BusinessCard key={business.id} business={business} />
        ))}
      </div>
    );
  }

  return (
    <div className="mt-10">
      <Title
        subTitle="Hơn 1202 dịch vụ gần đây"
        title="Các dịch vụ nổi bật"
        description="Những dịch vụ uy tín và phổ biến gần vị trí của bạn, dễ dàng lựa chọn và trải nghiệm những dịch vụ tốt nhất"
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
          Danh sách {pagination?.totalCount} dịch vụ
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

export default BusinessesPage;
