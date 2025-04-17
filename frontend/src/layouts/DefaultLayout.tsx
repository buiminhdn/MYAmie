import { Outlet } from "react-router-dom";
import Navbar from "./components/Navbar/Navbar";
import Footer from "./components/Footer/Footer";

function DefaultLayout() {
  return (
    <div>
      <Navbar />
      <div className="container px-3 sm:px-14 min-h-screen">
        <Outlet />
      </div>
      <Footer />
    </div>
  );
}

export default DefaultLayout;
