import cx from "classnames";

interface LoaderProps {
  className?: string;
  size?: string;
}

function Loader({ className = "", size = "fa-2x" }: LoaderProps) {
  return (
    <span
      className={cx("w-full inline-flex justify-center", className)}
      aria-label="Loading"
    >
      <i className={cx("text-primary fa-spin fa-solid fa-spinner", size)}></i>
    </span>
  );
}

export default Loader;
