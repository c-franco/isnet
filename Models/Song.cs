namespace isnet.Models
{
    public class Song
    {
        public string Track_Id { get; set; }

        public string Artist_Name { get; set; }

        public string Track_Name { get; set; }

        public int Popularity { get; set; }

        public string Year { get; set; }

        public string Genre { get; set; }

        public int Duration_ms { get; set; }
    }
}
