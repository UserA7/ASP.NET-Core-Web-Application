namespace PlaylistMVC.Models
{
    public class CreateSongInfoModelView
    {
        public List<CheckboxViewModel> Genres { get; set; }
        public SongInfo SongInfo { get; set; }
    }
}
