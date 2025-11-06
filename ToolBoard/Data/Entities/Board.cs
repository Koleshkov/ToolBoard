using ToolBoard.Data.Entities.Base;

namespace ToolBoard.Data.Entities
{
    public class Board : EntityBase
    {
        public IList<Job> Jobs { get; set; } = new List<Job>();
    }
}
