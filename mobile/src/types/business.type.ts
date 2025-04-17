export type BusinessDetail = {
  name: string;
  description: string;
  tags: string[];
  contact: {
    phone: string;
    email: string;
    hours: string;
  };
  coverImage: string;
  detailImages: string[];
};
