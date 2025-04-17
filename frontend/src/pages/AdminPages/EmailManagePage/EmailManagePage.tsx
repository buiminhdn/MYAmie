import Pagination from "@/components/Pagination/Pagination";
import { useState } from "react";
import {
  createColumnHelper,
  flexRender,
  getCoreRowModel,
  useReactTable,
} from "@tanstack/react-table";
import { EmailMarketingVM } from "@/models/viewmodels/email.vm";
import { EmailMarketingParams } from "@/models/params/email.param";
import { useGetMarketingEmails } from "@/services/email.service";
import CreateMarketingEmail from "../components/CreateMarketingEmail";
import DeleteMarketingEmail from "../components/DeleteMarketingEmail";

const columnHelper = createColumnHelper<EmailMarketingVM>();

const columns = [
  columnHelper.accessor("subject", {
    header: "Chủ đề",
    cell: (info) => {
      const email = info.row.original;
      return <p className="font-semibold">{email.subject}</p>;
    },
  }),
  columnHelper.accessor("status", {
    header: "Trạng thái",
    cell: (info) => (
      <span
        className={`px-2 py-1 rounded font-medium text-xs ${
          info.getValue() === "SENT"
            ? "bg-primary-lighter text-primary"
            : "bg-red-100 text-red-800"
        }`}
      >
        {info.getValue()}
      </span>
    ),
  }),
  columnHelper.display({
    id: "delete",
    header: "Hành động",
    cell: (info) => {
      const id = info.row.original.id;
      return <DeleteMarketingEmail id={id} />;
    },
  }),
];

function EmailManagePage() {
  const [params, setParams] = useState<EmailMarketingParams>({
    pageNumber: 1,
    pageSize: 20,
  });

  const { data, isLoading } = useGetMarketingEmails(params);

  const emails = data?.data?.emails || [];
  const pagination = data?.data?.pagination;

  const table = useReactTable({
    data: emails,
    columns,
    getCoreRowModel: getCoreRowModel(),
  });

  return (
    <div className="space-y-3">
      <CreateMarketingEmail />
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

export default EmailManagePage;
