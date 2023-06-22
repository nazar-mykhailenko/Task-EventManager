using System.ComponentModel.DataAnnotations;

namespace ToDoListWithAuth.Models
{
    public class Event
    {
        [Key]
        public int Id { get; set; }
        [Required] 
        public string UserId { get; set; }
        [Required]
        public string Title { get; set; }
        public string? Description { get; set; }
        [Required]
        public DateTime Start { get; set;}
        [Required]
        public DateTime End { get; set;}
    }

    public class EventComparer : IComparer<Event>
    {
        public int Compare(Event? x, Event? y)
        {
            return x.Start.CompareTo(y.Start);
        }
    }
}
