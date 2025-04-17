import { Role } from "@/models/app.interface";
import { NavLink } from "react-router-dom";
import cx from "classnames";
import IconText from "@/components/IconText/IconText";
import { ROLE_ROUTES } from "@/routes/role_routes";

function Sidebar({ role }: { role: Role }) {
  const roleKey = Role[role] as keyof typeof ROLE_ROUTES;
  const roleRoutes = ROLE_ROUTES[roleKey] || [];

  return (
    <div className="xl:mr-14">
      {roleRoutes.map(({ type, routes }, index) => (
        <div key={type} className={cx({ "mt-5": index > 0 })}>
          <p className="font-medium text-gray-500 text-xs">{type}</p>
          <div className="mt-3 flex flex-col gap-1">
            {routes.map(({ path, icon, label }) => (
              <NavLink
                key={path}
                to={path}
                className={({ isActive }) =>
                  cx("px-3 py-2.5 rounded-md hover:bg-white border-2", {
                    "bg-white border-gray-700": isActive,
                    "border-background": !isActive,
                  })
                }
              >
                <IconText icon={icon} text={label} iconClasses="w-5" />
              </NavLink>
            ))}
          </div>
          {index < roleRoutes.length - 1 && (
            <div className="mt-5 border-t border-gray-300" />
          )}
        </div>
      ))}
    </div>
  );
}

export default Sidebar;
