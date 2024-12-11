namespace BookStackAPI.Models
{
    public class Books
    {
        public int Id { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public float Price { get; set; }
        public int Count { get; set; }
    }
}
