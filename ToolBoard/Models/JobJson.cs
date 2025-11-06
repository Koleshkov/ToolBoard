using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using ToolBoard.Data.Entities;

namespace ToolBoard.Models
{
    internal class JobJson
    {

        [Column("Месторождение")]
        public string? Field { get; set; }

        [Column("Куст")]
        public string? Pad { get; set; }

        [Column("Скважина")]
        public string? Well { get; set; }

        [Column("Полевая партия")]
        public string? FieldTeam { get; set; }

        [Column("Номер телефона")]
        public string? Phone { get; set; }

        [Column("Тип скважины")]
        public string? Type { get; set; }

        [Column("Буровой подрядчик")]
        public string? DrillingContractor { get; set; }

    }
}
