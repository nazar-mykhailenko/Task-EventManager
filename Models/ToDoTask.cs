using System.ComponentModel.DataAnnotations;

namespace ToDoListWithAuth.Models
{
	public class ToDoTask
	{
		[Key]
		public int ID { get; set; }
		[Required]
		public string UserId { get; set; }
		public DateTime? Deadline { get; set; }
		[Required]
		public string Title { get; set; }
		public string? Description { get; set; }
		[Required]
		public bool IsDone { get; set; }



	}

    public class TaskComparer : IComparer<ToDoTask>
    {
        public int Compare(ToDoTask? x, ToDoTask? y)
        {
            if (x.IsDone == y.IsDone)
            {
                if (x.Deadline == null && y.Deadline == null)
                {
                    return 0;
                }
                else if (y.Deadline == null || x.Deadline < y.Deadline)
                {
                    return -1;
                }
                else if (x.Deadline == null || x.Deadline > y.Deadline)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            else if (x.IsDone)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
    }
}
