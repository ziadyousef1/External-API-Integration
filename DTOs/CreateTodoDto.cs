using System.ComponentModel.DataAnnotations.Schema;
using External_API_Integration.Model;

namespace External_API_Integration.DTOs
{
    public class CreateTodoDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
        
        public DateTime CreatedDate { get; set; }
        public DateTime DueDate { get; set; }
        public Priority Priority { get; set; }
    }
}
