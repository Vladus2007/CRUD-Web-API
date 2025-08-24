using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace WebApplication4.Models
{
    public class NotesRequest
    {   
        public NotesRequest(string Title,string Description) {
            title = Title;
            description = Description;  
        }
        public string title { get; set; }
        public string description { get; set; }
    }
}
