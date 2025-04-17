export type PlaceDetail = {
  id: number;
  title: string;
  description: string;
  shortIntro: string;
  tags: string[];
  user: {
    name: string;
    avatar: string;
    postedAt: string;
  };
  location: string;
  city: string;
  images: string[];
};
