import Logo from "@/assets/images/Logo";
import { ROUTE_PATH } from "@/routes/route-path";
import { Link } from "react-router-dom";
import cx from "classnames";
import Button from "@/components/Button/Button";
import IconText from "@/components/IconText/IconText";

const socialLinks = [
  {
    id: 1,
    icon: "fa-square-facebook",
    href: "https://www.facebook.com/groups/1229481658056087",
  },
  { id: 2, icon: "fa-instagram", href: "/" },
  { id: 3, icon: "fa-tiktok", href: "/" },
];

const navLinks = [
  { id: 1, name: "TRANG CHỦ", href: ROUTE_PATH.BUSINESSES },
  { id: 2, name: "ĐỊA ĐIỂM", href: ROUTE_PATH.PLACES },
  { id: 3, name: "BẠN BÈ", href: ROUTE_PATH.USERS },
  { id: 4, name: "TRỢ GIÚP", href: ROUTE_PATH.BUSINESSES },
];

function Footer() {
  const handleScrollToTop = () =>
    window.scrollTo({ top: 0, behavior: "smooth" });

  return (
    <footer className="bg-white px-3 sm:px-16 border-t-2 border-gray-300 mt-52 relative">
      {/* Scroll to Top Button */}
      <Button
        onClick={handleScrollToTop}
        className="absolute -top-6 right-12 size-11"
      >
        <i className="fa-xl fa-regular fa-arrow-up"></i>
      </Button>

      {/* Footer Content */}
      <div className="container sm:px-10 grid grid-cols-1 md:grid-cols-2 gap-5 py-16">
        {/* Logo */}
        <Link to={ROUTE_PATH.BUSINESSES} className="m-auto">
          <Logo width={200} height={50} />
        </Link>

        {/* Contact Info & Social Links */}
        <div>
          <div className="flex justify-center sm:justify-start flex-wrap gap-6 mt-4">
            <IconText
              icon="fa-map-location-dot"
              text="Da Nang City"
              textClasses="text-base"
              iconClasses="text-primary w-7"
            />
            <IconText
              icon="fa-phone-alt"
              text="(+84) 70 616 2561"
              textClasses="text-base"
              iconClasses="text-primary w-7"
            />
            <IconText
              icon="fa-at"
              text="support@myamie.site"
              textClasses="text-base"
              iconClasses="text-primary w-7"
            />
          </div>

          {/* Social Media Links */}
          <div className="mt-10 flex flex-wrap items-center justify-center sm:justify-start gap-3 sm:gap-10">
            <p className="text-gray-600">Kết nối thêm:</p>
            <div className="flex gap-6 text-2xl text-primary">
              {socialLinks.map(({ id, icon, href }) => (
                <Link key={id} to={href} target="_blank">
                  <i className={cx("fab", icon)}></i>
                </Link>
              ))}
            </div>
          </div>
        </div>
      </div>

      {/* Footer Bottom Section */}
      <div className="container border-t border-gray-300 py-5 flex gap-4 flex-wrap justify-between">
        <div className="flex flex-wrap gap-3 sm:gap-7 font-medium mx-auto md:mx-0">
          {navLinks.map(({ id, name, href }) => (
            <Link key={id} to={href} className="hover:underline mx-auto">
              {name}
            </Link>
          ))}
        </div>
        <p className="text-gray-600 mx-auto md:mx-0">
          Bản quyền © 2024 - Công ty MYAmie
        </p>
      </div>
    </footer>
  );
}

export default Footer;
