import cx from "classnames";

interface IconTextProps {
  icon: string;
  text: string;
  iconClasses?: string;
  textClasses?: string;
  className?: string;
}

function IconText({
  icon,
  text,
  iconClasses = "",
  textClasses = "",
  className = "",
}: IconTextProps) {
  return (
    <div className={cx("flex items-center gap-2", className)}>
      <span className={cx("flex justify-center flex-none", iconClasses)}>
        <i className={cx("fa-regular", icon)}></i>
      </span>
      <p className={cx("text-wrap", textClasses)}>{text}</p>
    </div>
  );
}

export default IconText;
