interface TitleProps {
  subTitle: string;
  title: string;
  description: string;
}

function Title({ subTitle, title, description }: TitleProps) {
  return (
    <div className="flex flex-col items-center gap-3 text-center">
      <span className="text-xs font-medium bg-primary-light py-1.5 px-4 rounded-full">
        {subTitle}
      </span>
      <h2 id="main-title" className="text-4xl font-semibold text-primary">
        {title}
      </h2>
      <p className="text-gray-800">{description}</p>
    </div>
  );
}

export default Title;
