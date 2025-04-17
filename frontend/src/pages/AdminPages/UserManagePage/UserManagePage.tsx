import {
  useReactTable,
  getCoreRowModel,
  flexRender,
  createColumnHelper,
} from "@tanstack/react-table";
import { AdminUserVM } from "@/models/viewmodels/admin-user.vm";
import { useState } from "react";
import { useGetUsersByAdmin } from "@/services/admin-user.service";
import Avatar from "@/components/Avatar/Avatar";
import FilterSelects from "../components/FilterSelects";
import Input from "@/components/Input/Input";
import Button from "@/components/Button/Button";
import { AccountStatus, RoleForFilter } from "@/models/app.interface";
import { AdminUserParams } from "@/models/params/admin-user.param";
import Pagination from "@/components/Pagination/Pagination";
import PasswordChange from "../components/PasswordChange";
import AccountStatusChange from "../components/AccountStatusChange";

const columnHelper = createColumnHelper<AdminUserVM>();

const columns = [
  columnHelper.display({
    id: "userInfo",
    header: "Người dùng",
    cell: (info) => {
      const user = info.row.original;
      return (
        <div className="flex items-center space-x-3">
          <Avatar
            src={user.avatar}
            alt="avatar"
            size="size-11"
            hasBorder={false}
          />
          <div>
            <p className="font-semibold">{user.name}</p>
            <p className="text-sm text-gray-500">{user.email}</p>
          </div>
        </div>
      );
    },
  }),
  columnHelper.accessor("city", {
    header: "Thành phố",
  }),
  columnHelper.accessor("role", {
    header: "Vai trò",
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
      const user = info.row.original;
      return (
        <div className="flex flex-col text-xs gap-2">
          <AccountStatusChange
            id={user.id}
            status={AccountStatus[user.status as keyof typeof AccountStatus]}
          />
          <PasswordChange id={user.id} />
        </div>
      );
    },
  }),
];

function UserManagePage() {
  const [params, setParams] = useState<AdminUserParams>({
    pageNumber: 1,
    pageSize: 20,
  });
  const { data, isLoading } = useGetUsersByAdmin(params);

  const users = data?.data?.users || [];
  const pagination = data?.data?.pagination;

  const handleFilterChange = (field: keyof AdminUserParams, value: any) => {
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
        options={AccountStatus}
        onFilterChange={(value) => handleFilterChange("status", value)}
      />
      <FilterSelects
        label="Vai trò"
        options={RoleForFilter}
        onFilterChange={(value) => handleFilterChange("role", value)}
      />
      <form onSubmit={handleSearch} className="flex gap-2 w-1/2 mb-7">
        <Input
          name="search"
          type="text"
          placeholder="Nhập tên hoặc email"
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

export default UserManagePage;
