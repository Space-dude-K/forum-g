using Entities.DTO.UserDto;

namespace Forum.Extensions
{
    public static class ForumAvatarHelper
    {
        public static string LoadAvatar(this ForumUserDto forumUserDto, string webRootPath)
        {
            string filePath = Path.Combine(webRootPath, "images", "avatars", forumUserDto.FirstAndLastNames + ".jpg");

            if (!string.IsNullOrEmpty(forumUserDto.FirstAndLastNames))
            {
                if (File.Exists(filePath))
                {
                    filePath = "~/images/avatars/" + forumUserDto.FirstAndLastNames + ".jpg";
                }
                else
                {
                    filePath = "~/images/avatars/EmptyAvatar.jpg";
                }
            }

            return filePath;
        }
    }
}