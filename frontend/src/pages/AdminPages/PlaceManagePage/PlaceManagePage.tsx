import Avatar from "@/components/Avatar/Avatar";
import Button from "@/components/Button/Button";
import Pagination from "@/components/Pagination/Pagination";
import { AdminPlaceParams } from "@/models/params/admin-place.param";
import { AdminPlaceVM } from "@/models/viewmodels/admin-place.vm";
import { useGetPlacesByAdmin } from "@/services/admin-place.service";
import {
  createColumnHelper,
  flexRender,
  getCoreRowModel,
  useReactTable,
} from "@tanstack/react-table";
import React, { useState } from "react";
import FilterSelects from "../components/FilterSelects";
import { PlaceStatus } from "@/models/app.interface";
import Input from "@/components/Input/Input";
import getImageUrl from "@/utils/imageUtils";
import PlaceStatusChange from "../components/PlaceStatusChange";

const columnHelper = createColumnHelper<AdminPlaceVM>();

const columns = [
  columnHelper.display({
    id: "placeInfo",
    header: "Địa điểm",
    cell: (info) => {
      const place = info.row.original;
      return (
        <div className="flex items-center space-x-3">
          <img
            src={getImageUrl(place.cover, "cover")}
            className="h-14 w-24 object-cover rounded-md border border-gray-300"
          />
          <p className="font-semibold">{place.name}</p>
        </div>
      );
    },
  }),
  columnHelper.accessor("city", {
    header: "Thành phố",
  }),
  columnHelper.display({
    id: "ownerInfo",
    header: "Chủ sở hữu",
    cell: (info) => {
      const place = info.row.original;
      return (
        <div className="flex items-center space-x-3">
          <Avatar
            src={place.ownerAvatar}
            alt="owner-avatar"
            size="size-8"
            hasBorder={false}
          />
          <p className="text-sm font-medium">{place.ownerName}</p>
        </div>
      );
    },
  }),
  columnHelper.accessor("status", {
    header: "Trạng thái",
    cell: (info) => (
      <span
        className={`px-2 py-1 rounded font-medium text-xs ${
          info.getValue() === "ACTIVATED"
            ? "bg-primary-lighter text-primary"
            : "bg-red-100 text-red-800"
        }`}
      >
        {info.getValue()}
      </span>
    ),
  }),
  columnHelper.display({
    id: "status-update",
    header: "Trạng thái",
    cell: (info) => {
      const place = info.row.original;
      return (
        <PlaceStatusChange
          id={place.id}
          status={PlaceStatus[place.status as keyof typeof PlaceStatus]}
        />
      );
    },
  }),
];

function PlaceManagePage() {
  const [params, setParams] = useState<AdminPlaceParams>({
    pageNumber: 1,
    pageSize: 20,
  });
  const { data, isLoading } = useGetPlacesByAdmin(params);

  const users = data?.data?.places || [];
  const pagination = data?.data?.pagination;

  const handleFilterChange = (field: keyof AdminPlaceParams, value: any) => {
    setParams((prev) => ({ ...prev, [field]: value }));
  };

  const handleSearch = (event: React.FormEvent) => {
    event.preventDefault();
    const searchTerm = (event.target as HTMLFormElement).search.value;
    setParams((prev) => ({ ...prev, searchTerm }));
  };

  const table = useReactTable({
    data: users,
    columns,
    getCoreRowModel: getCoreRowModel(),
  });

  return (
    <div className="space-y-3">
      <FilterSelects
        label="Trạng thái"
        options={PlaceStatus}
        onFilterChange={(value) => handleFilterChange("status", value)}
      />
      <form onSubmit={handleSearch} className="flex gap-2 w-1/2 mb-7">
        <Input
          name="search"
          type="text"
          placeholder="Nhập tên hoặc từ khoá"
          className="bg-white"
        />
        <Button
          type="submit"
          variant="outline"
          className="px-4 hover:cursor-pointer"
        >
          <i className="fa-solid fa-magnifying-glass"></i>
        </Button>
      </form>
      <div className="p-5 border-2 border-gray-200 rounded-xl bg-white overflow-x-auto">
        {isLoading ? (
          <p className="text-center py-5">Đang tải...</p>
        ) : (
          <>
            <table className="min-w-full rounded-lg overflow-hidden">
              <thead>
                {table.getHeaderGroups().map((headerGroup) => (
                  <tr key={headerGroup.id} className="bg-gray-100">
                    {headerGroup.headers.map((header) => (
                      <th
                        key={header.id}
                        className="p-3 border-b border-gray-200 text-left"
                      >
                        {flexRender(
                          header.column.columnDef.header,
                          header.getContext()
                        )}
                      </th>
                    ))}
                  </tr>
                ))}
              </thead>
              <tbody>
                {table.getRowModel().rows.map((row) => (
                  <tr
                    key={row.id}
                    className="hover:bg-gray-50 border-b border-gray-100"
                  >
                    {row.getVisibleCells().map((cell) => (
                      <td key={cell.id} className="p-3">
                        {flexRender(
                          cell.column.columnDef.cell,
                          cell.getContext()
                        )}
                      </td>
                    ))}
                  </tr>
                ))}
              </tbody>
            </table>
            <Pagination
              currentPage={params.pageNumber || 1}
              totalPage={pagination?.totalPages}
              onPageChange={(pageNumber) =>
                setParams((prev) => ({ ...prev, pageNumber }))
              }
            />
          </>
        )}
      </div>
    </div>
  );
}

export default PlaceManagePage;
