using Newtonsoft.Json;

namespace PlaylistMVC.Models
{
    public class SongDetailsModel
    {
        public Artist artist { get; set; }
        public Tracks tracks { get; set; }
    }

    public class Artist
    {
        public Hit[] hits { get; set; }
    }

    public class Hit
    {
        public Artist1 artist { get; set; }
    }

    public class Artist1
    {
        public int adam_id { get; set; }
        public object alias { get; set; }
        public string avatar { get; set; }
        public object[] genres { get; set; }
        public object genres_primary { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public bool verified { get; set; }
    }

    public class Tracks
    {
        public Hit1[] hits { get; set; }
    }

    public class Hit1
    {
        public Action1[] actions { get; set; }
        public string alias { get; set; }
        public Artist2[] artists { get; set; }
        public Heading heading { get; set; }
        public Images images { get; set; }
        public string key { get; set; }
        public Share share { get; set; }
        public Stores stores { get; set; }
        public Streams streams { get; set; }
        public string type { get; set; }
        public string url { get; set; }
        public Urlparams urlparams { get; set; }
    }

    public class Heading
    {
        public string subtitle { get; set; }
        public string title { get; set; }
    }

    public class Images
    {
        public string blurred { get; set; }
        public string _default { get; set; }
        public string play { get; set; }
    }

    public class Share
    {
        public string avatar { get; set; }
        public string href { get; set; }
        public string html { get; set; }
        public string image { get; set; }
        public string snapchat { get; set; }
        public string subject { get; set; }
        public string text { get; set; }
        public string twitter { get; set; }
    }

    public class Stores
    {
        public Apple apple { get; set; }
    }

    public class Apple
    {
        public Action[] actions { get; set; }
        public string coverarturl { get; set; }
        public bool _explicit { get; set; }
        public string previewurl { get; set; }
        public string productid { get; set; }
        public string trackid { get; set; }
    }

    public class Action
    {
        public string type { get; set; }
        public string uri { get; set; }
    }

    public class Streams
    {
    }

    public class Urlparams
    {
        public string trackartist { get; set; }
        public string tracktitle { get; set; }
    }

    public class Action1
    {
        public string id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
    }

    public class Artist2
    {
        public string adamid { get; set; }
        public string alias { get; set; }
        public string id { get; set; }
    }

}
