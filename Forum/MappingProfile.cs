using AutoMapper;
using Entities.DTO.ForumDto;
using Entities.DTO.ForumDto.Create;
using Entities.DTO.ForumDto.Update;
using Entities.DTO.UserDto;
using Entities.DTO.UserDto.Create;
using Entities.Models;
using Entities.Models.Forum;
using Entities.ViewModels;
using Forum.ViewModels;

namespace Forum
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User
            CreateMap<AppUser, UserDto>();
            CreateMap<UserForCreationDto, AppUser>();

            // Category
            CreateMap<ForumCategory, ForumCategoryDto>();
            CreateMap<ForumCategoryForCreationDto, ForumCategory>();
            CreateMap<ForumCategoryForUpdateDto, ForumCategory>();
            CreateMap<ForumCategoryForUpdateDto, ForumCategory>().ReverseMap();

            // Forum
            CreateMap<ForumBase, ForumBaseDto>();
            CreateMap<ForumBaseForCreationDto, ForumBase>();
            CreateMap<ForumBaseForUpdateDto, ForumBase>();
            CreateMap<ForumBaseForUpdateDto, ForumBase>().ReverseMap();

            // Topic
            CreateMap<ForumTopic, ForumTopicDto>();
            CreateMap<ForumTopicForCreationDto, ForumTopic>();
            CreateMap<ForumTopicForUpdateDto, ForumTopic>();
            CreateMap<ForumTopicForUpdateDto, ForumTopic>().ReverseMap();

            // Post
            CreateMap<ForumPost, ForumPostDto>();
            CreateMap<ForumPostForCreationDto, ForumPost>();
            CreateMap<ForumPostForUpdateDto, ForumPost>();
            CreateMap<ForumPostForUpdateDto, ForumPost>().ReverseMap();

            // Views
            CreateMap<RegisterViewModel, UserForCreationDto>();
            CreateMap<LoginViewModel, UserForAuthenticationDto>();
        }
    }
}