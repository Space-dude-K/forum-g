namespace Entities.RequestFeatures.Forum
{
    public class ForumPostParameters : RequestParameters
    {
        public uint MinLikes { get; set; }
        public uint MaxLikes { get; set; } = int.MaxValue;
        public bool ValidLikeRange => MaxLikes > MinLikes;
    }
}