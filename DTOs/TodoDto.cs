using External_API_Integration.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace External_API_Integration.DTOs
{
    public class TodoDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime DueDate { get; set; }
        public string Priority { get; set; }
    }
}
