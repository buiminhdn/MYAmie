import CoverUpload from "./components/CoverUpload";
import AvatarUpload from "./components/AvatarUpload";
import {
  useGetBusinessProfile,
  useGetProfile,
} from "@/services/account.service";
import { useSelector } from "react-redux";
import { accountRoleSelector } from "@/store/auth/auth.selector";
import Loader from "@/components/Loader/Loader";
import DetailProfileUpload from "./components/DetailProfileUpload";
import { Role } from "@/models/app.interface";
import DetailProfileBusinessUpload from "./components/DetailProfileBusinessUpload";
import {
  BusinessProfileVM,
  UserProfileVM,
} from "@/models/viewmodels/profile.vm";

function SettingPage() {
  const role = useSelector(accountRoleSelector);
  const isBusiness = role === Role.BUSINESS;

  const {
    data: profileData,
    isLoading,
    isError,
  } = isBusiness ? useGetBusinessProfile() : useGetProfile();

  if (isLoading) {
    return <Loader className="mt-10 mx-auto" />;
  }

  if (isError || !profileData?.data) {
    return <p className="error mt-10">Lỗi, vui lòng thử lại</p>;
  }

  return (
    <div className="p-5 border-2 border-gray-200 rounded-xl bg-white">
      <CoverUpload image={profileData.data.cover} />
      <div className="flex flex-wrap md:flex-nowrap gap-10 mt-7">
        <AvatarUpload image={profileData.data.avatar} />
        {isBusiness ? (
          <DetailProfileBusinessUpload
            detail={profileData.data as BusinessProfileVM}
          />
        ) : (
          <DetailProfileUpload detail={profileData.data as UserProfileVM} />
        )}
      </div>
    </div>
  );
}

export default SettingPage;
