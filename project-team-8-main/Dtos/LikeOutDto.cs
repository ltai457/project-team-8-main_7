namespace Project_Authentication.Dtos
{
    public class LikeOutDto
    {
        public int ProjectLikeID { get; set; }

        public int ProjectID { get; set; }

        public int UserID { get; set; }

        public string UserName { get; set; }

        public DateTime DateLiked { get; set; }
    }
}
