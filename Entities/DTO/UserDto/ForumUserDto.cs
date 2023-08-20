namespace Entities.DTO.UserDto
{
    public class ForumUserDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FirstAndLastNames { get { return FirstName + " " + LastName; } }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string? AvatarImgSrc { get; set; }
        public int TotalPostCounter { get; set; }
        public DateTime? CreatedAt { get; set; }
        public ICollection<string> Roles { get; set; }
    }
}