using AutoMapper;
using Entities.DTO.ForumDto;
using Entities.DTO.ForumDto.Create;
using Entities.DTO.ForumDto.Update;
using Entities.Models.Forum;

namespace Forum
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ForumBase, ForumBaseDto>();
            CreateMap<ForumCategory, ForumCategoryDto>();
            CreateMap<ForumCategoryForCreationDto, ForumCategory>();
            CreateMap<ForumBaseForCreationDto, ForumBase>();
            CreateMap<ForumBaseForUpdateDto, ForumBase>();
            CreateMap<ForumCategoryForUpdateDto, ForumCategory>();
        }
    }
}