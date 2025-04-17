import LoginForm from "../components/LoginForm/LoginForm";
import ForgetPassword from "../components/ForgetPassword/ForgetPassword";
import VerifyEmail from "../components/VerifyEmail/VerifyEmail";

function LoginPage() {
  return (
    <div>
      <p id="login-title" className="text-lg font-semibold">
        Đăng nhập
      </p>
      <LoginForm />
      <div className="flex flex-wrap justify-center gap-4 mt-3">
        <ForgetPassword />
        <VerifyEmail />
      </div>
    </div>
  );
}

export default LoginPage;
