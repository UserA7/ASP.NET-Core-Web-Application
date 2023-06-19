namespace PlaylistMVC.Models
{
    public class SongInfo
    {
        public int Id { get; set; }
        public string? SongName { get; set; }
        public string? ArtistName { get; set; }

        public List<SongInfoGenre> SongInfoGenres { get; set; } = new List<SongInfoGenre>();
    }
}
