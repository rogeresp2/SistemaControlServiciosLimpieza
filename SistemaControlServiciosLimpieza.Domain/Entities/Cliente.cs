using SistemaControlServiciosLimpieza.Domain.Core;
using System.ComponentModel.DataAnnotations;

namespace SistemaControlServiciosLimpieza.Domain.Entities
{
    public class Cliente: BaseEntity
    {
        //public int Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(15)]
        public string? Phone { get; set; }

        [StringLength(150)]
        public string? Email { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
