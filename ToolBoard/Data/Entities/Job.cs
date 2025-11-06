using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ToolBoard.Data.Entities.Base;

namespace ToolBoard.Data.Entities
{
    public class Job : EntityBase
    {
        public IList<ToolSticker> ToolStickers { get; set; } = new List<ToolSticker>();
        public IList<SurfaceSticker> SurfaceStickers { get; set; } = new List<SurfaceSticker>();
        [JsonIgnore]
        public Guid BoardId { get; set; }
        [JsonIgnore]
        public Board Board { get; set; } = new();

        [Required(ErrorMessage ="Укажите конструкцию скважины")]
        public string Type { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Header { get; set; } = "";
        public string Mark { get; set; } = "";
        public string Footer { get; set; } = "";
        public string FieldTeam { get; set; } = "";
    }
}
