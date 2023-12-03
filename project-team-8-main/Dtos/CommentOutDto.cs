namespace Project_Authentication.Dtos
{
    public class CommentOutDto
    {
        public int ProjectID { get; set; }

        public string? CommentText { get; set; }

        public int UserID { get; set; }

        public string? UserName { get; set; }

        public DateTime DateTimes { get; set; }

        public DateTime OriginalTimestamp { get; set; }
    }
}
