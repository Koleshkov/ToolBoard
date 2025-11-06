using ToolBoard.Data.Entities.Base;
using ToolBoard.Data.Entities.Enums;

namespace ToolBoard.Data.Entities
{
    public class ToolSticker : EntityBase
    {
        public Guid JobId { get; set; }
        public Job Job { get; set; } = new();
        public string Color { get; set; } = "primary";
        public int Order { get; set; }
    }
}
