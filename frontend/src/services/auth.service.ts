import { useMutation } from "@tanstack/react-query";
import { toast } from "react-hot-toast";
import { signUp, signUpBusiness, signIn } from "@/apis/auth.api";
import { handleError } from "@/utils/errorUtils";
import { useDispatch } from "react-redux";
import { login } from "@/store/auth/auth.slice";

export const useSignUp = () => {
  return useMutation({
    mutationFn: signUp,
    onSuccess: () => {
      toast.success("Đăng ký tài khoản thành công");
      toast("Vui lòng xác minh Email", {
        icon: "📧",
      });
    },
    onError: (error: Error) => {
      toast.error(handleError(error));
    },
  });
};

export const useSignUpBusiness = () => {
  return useMutation({
    mutationFn: signUpBusiness,
    onSuccess: () => {
      toast.success("Đăng ký tài khoản doanh nghiệp thành công");
      toast("Vui lòng xác minh Email", {
        icon: "📧",
      });
    },
    onError: (error: Error) => {
      toast.error(handleError(error));
    },
  });
};

export const useSignIn = () => {
  const dispatch = useDispatch();
  return useMutation({
    mutationFn: signIn,
    onSuccess: (data) => {
      toast.success("Đăng nhập thành công");
      if (data.data) {
        dispatch(login(data.data));
      }
    },
    onError: (error: Error) => {
      toast.error(handleError(error));
    },
  });
};

// export const useSignOut = () => {
//   const queryClient = useQueryClient();
//   const dispatch = useDispatch();
//   const navigate = useNavigate();
//   return useMutation({
//     mutationFn: signOut,
//     onSuccess: () => {
//       queryClient.clear();
//       dispatch(logout());
//       navigate(0);
//       toast.success("Đăng xuất thành công");
//     },
//     onError: (error: Error) => {
//       toast.error(handleError(error));
//     },
//   });
// };
