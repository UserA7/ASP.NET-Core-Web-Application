namespace PlaylistMVC.Models
{
    public class SongInfoGenre
    {
        public int Id { get; set; }
        public Genre Genre { get; set; }
        public SongInfo SongInfo { get; set; }
    }
}
