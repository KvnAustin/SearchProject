using System.ComponentModel.DataAnnotations;

namespace SearchProject.Models
{
    public class BaseEntity<T>
    {
        [Key]
        public T Id { get; set; }
    }
}
