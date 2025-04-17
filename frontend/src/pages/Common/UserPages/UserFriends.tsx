import FriendCard from "@/components/Cards/FriendCard";
import Loader from "@/components/Loader/Loader";
import Pagination from "@/components/Pagination/Pagination";
import { FilterFriendshipParams } from "@/models/params/friendship.param";
import { useGetFriendships } from "@/services/friendship.service";
import { useState } from "react";

function UserFriends() {
  const [params, setParams] = useState<FilterFriendshipParams>({
    pageNumber: 1,
  });

  const { data, isLoading, isError } = useGetFriendships(params);

  const friends = data?.data?.friends ?? [];
  const pagination = data?.data?.pagination;

  return (
    <>
      {isLoading ? (
        <Loader />
      ) : isError ? (
        <p className="error">Lỗi, vui lòng thử lại</p>
      ) : friends.length === 0 ? (
        <p>Chưa có bạn bè</p>
      ) : (
        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-3">
          {friends.map((friend) => (
            <FriendCard key={friend.id} friend={friend} />
          ))}
        </div>
      )}

      <Pagination
        currentPage={pagination?.currentPage ?? 1}
        totalPage={pagination?.totalPages ?? 1}
        onPageChange={(pageNumber) =>
          setParams((prev) => ({ ...prev, pageNumber }))
        }
      />
    </>
  );
}

export default UserFriends;
