using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ToolBoard.Data.Entities.Base
{
    public class EntityBase
    {
        [JsonIgnore]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Введите название")]
        public string Name { get; set; } = "";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
