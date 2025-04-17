using AutoMapper;
using Common.DTOs.UserDtos;
using Common.ViewModels.AdminPlaceVMs;
using Common.ViewModels.AdminUserVMs;
using Common.ViewModels.AuthVMs;
using Common.ViewModels.BusinessVMs;
using Common.ViewModels.CategoryVMs;
using Common.ViewModels.ChatVMs;
using Common.ViewModels.CityVMs;
using Common.ViewModels.FeedbackVMs;
using Common.ViewModels.PlaceVMs;
using Common.ViewModels.ProfileVMs;
using Common.ViewModels.UserVMs;
using Models.Accounts;
using Models.Categories;
using Models.Cities;
using Models.Feedbacks;
using Models.Messages;
using Models.Places;
using Utility;

namespace Common.Mapping;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Account, AuthAccountVM>();

        CreateMap<Category, CategoryVM>();
        CreateMap<City, CityVM>();

        CreateMap<Account, UserProfileVM>()
            .ForMember(vm => vm.DateOfBirth, opt => opt.MapFrom(src => DateTimeUtils.EpochToDateString(src.DateOfBirth)))
            .ForMember(vm => vm.Characteristics, opt => opt.MapFrom(src => src.Characteristics.Split(';', StringSplitOptions.None)))
            .ForMember(vm => vm.Images, opt => opt.MapFrom(src => src.Images.Split(';', StringSplitOptions.None)))
            .ForMember(vm => vm.CityId, opt => opt.MapFrom(src => src.City.Id))
            .ForMember(vm => vm.CategoryIds, opt => opt.MapFrom(src => src.Categories.Select(c => c.Id).ToList()));

        CreateMap<Account, BusinessProfileVM>()
            .ForMember(vm => vm.Name, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(vm => vm.OpenHour, opt => opt.MapFrom(src => int.Parse(src.Business.OperatingHours.Split('-', StringSplitOptions.None)[0])))
            .ForMember(vm => vm.CloseHour, opt => opt.MapFrom(src => int.Parse(src.Business.OperatingHours.Split('-', StringSplitOptions.None)[1])))
            .ForMember(vm => vm.Images, opt => opt.MapFrom(src => src.Images.Split(';', StringSplitOptions.None)))
            .ForMember(vm => vm.Phone, opt => opt.MapFrom(src => src.Business.Phone))
            .ForMember(vm => vm.Address, opt => opt.MapFrom(src => src.Business.Address))
            .ForMember(vm => vm.CityId, opt => opt.MapFrom(src => src.City.Id))
            .ForMember(vm => vm.CategoryIds, opt => opt.MapFrom(src => src.Categories.Select(c => c.Id).ToList()));

        CreateMap<Place, PlaceVM>()
           .ForMember(vm => vm.Cover, opt => opt.MapFrom(src => ImageUtils.GetCoverImage(src.Images)))
           .ForMember(vm => vm.City, opt => opt.MapFrom(src => src.City.Name))
           .ForMember(vm => vm.OwnerId, opt => opt.MapFrom(src => src.Owner.Id))
           .ForMember(vm => vm.OwnerAvatar, opt => opt.MapFrom(src => src.Owner.Avatar))
           .ForMember(vm => vm.OwnerName, opt => opt.MapFrom(src => src.Owner.LastName + " " + src.Owner.FirstName))
           .ForMember(vm => vm.DateCreated, opt => opt.MapFrom(src => DateTimeUtils.EpochToDateString(src.CreatedDate)));

        CreateMap<Place, PlaceDetailVM>()
         .ForMember(vm => vm.OwnerId, opt => opt.MapFrom(src => src.Owner.Id))
         .ForMember(vm => vm.OwnerAvatar, opt => opt.MapFrom(src => src.Owner.Avatar))
         .ForMember(vm => vm.OwnerName, opt => opt.MapFrom(src => src.Owner.LastName + " " + src.Owner.FirstName))
         .ForMember(vm => vm.DateCreated, opt => opt.MapFrom(src => DateTimeUtils.EpochToTime(src.CreatedDate)))
         .ForMember(vm => vm.Images, opt => opt.MapFrom(src => src.Images.Split(';', StringSplitOptions.None)));

        CreateMap<Account, UserVM>()
            .ForMember(vm => vm.City, opt => opt.MapFrom(src => src.City.Name))
            .ForMember(vm => vm.Name, opt => opt.MapFrom(src => src.LastName + " " + src.FirstName))
            .ForMember(vm => vm.Characteristics, opt => opt.MapFrom(src => src.Characteristics.Split(';', StringSplitOptions.None)))
            .ForMember(vm => vm.Distance, opt => opt.MapFrom((src, dest, destMember, context) =>
            {
                var param = context.Items["FilterUserParams"] as FilterUserParams;
                if (param == null || src.Latitude == 0 || src.Longitude == 0)
                    return 0;
                return GeoUtils.CalculateDistance((double)src.Latitude, (double)src.Longitude, (double)param.Latitude, (double)param.Longitude);
            }));

        CreateMap<Account, UserDetailVM>()
          .ForMember(vm => vm.City, opt => opt.MapFrom(src => src.City.Name))
          .ForMember(vm => vm.Characteristics, opt => opt.MapFrom(src => src.Characteristics.Split(';', StringSplitOptions.None)))
          .ForMember(vm => vm.Images, opt => opt.MapFrom(src => src.Images.Split(';', StringSplitOptions.None)))
          .ForMember(vm => vm.Name, opt => opt.MapFrom(src => src.LastName + " " + src.FirstName));

        CreateMap<Account, BusinessVM>()
            .ForMember(vm => vm.City, opt => opt.MapFrom(src => src.City.Name))
            .ForMember(vm => vm.Name, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(vm => vm.OperatingHours, opt => opt.MapFrom(src => src.Business.OperatingHours));

        CreateMap<Account, BusinessDetailVM>()
           .ForMember(vm => vm.City, opt => opt.MapFrom(src => src.City.Name))
           .ForMember(vm => vm.Phone, opt => opt.MapFrom(src => src.Business.Phone))
           .ForMember(vm => vm.Address, opt => opt.MapFrom(src => src.Business.Address))
           .ForMember(vm => vm.OperatingHours, opt => opt.MapFrom(src => src.Business.OperatingHours))
           .ForMember(vm => vm.Images, opt => opt.MapFrom(src => src.Images.Split(';', StringSplitOptions.None)))
           .ForMember(vm => vm.Name, opt => opt.MapFrom(src => src.FirstName));


        CreateMap<Message, MessageVM>()
            .ForMember(dest => dest.SentAt, opt => opt.MapFrom(src => DateTimeUtils.EpochToDateString(src.CreatedDate)))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

        CreateMap<Feedback, FeedbackVM>()
           .ForMember(vm => vm.Avatar, opt => opt.MapFrom(src => src.Sender.Avatar))
           .ForMember(vm => vm.Name, opt => opt.MapFrom(src => src.Sender.LastName + " " + src.Sender.FirstName))
           .ForMember(vm => vm.OwnerId, opt => opt.MapFrom(src => src.TargetId))
           .ForMember(vm => vm.CreatedDate, opt => opt.MapFrom(src => DateTimeUtils.EpochToDateString(src.CreatedDate)));

        CreateMap<Account, AdminUserVM>()
           .ForMember(vm => vm.City, opt => opt.MapFrom(src => src.City.Name))
           .ForMember(vm => vm.Role, opt => opt.MapFrom(src => src.Role.ToString()))
           .ForMember(vm => vm.Name, opt => opt.MapFrom(src => src.LastName + " " + src.FirstName))
           .ForMember(vm => vm.Status, opt => opt.MapFrom(src => src.Status.ToString()));

        CreateMap<Place, AdminPlaceVM>()
          .ForMember(vm => vm.Cover, opt => opt.MapFrom(src => ImageUtils.GetCoverImage(src.Images)))
          .ForMember(vm => vm.City, opt => opt.MapFrom(src => src.City.Name))
          .ForMember(vm => vm.OwnerAvatar, opt => opt.MapFrom(src => src.Owner.Avatar))
          .ForMember(vm => vm.OwnerName, opt => opt.MapFrom(src => src.Owner.LastName + " " + src.Owner.FirstName))
          .ForMember(vm => vm.Status, opt => opt.MapFrom(src => src.Status.ToString()));
    }
}
