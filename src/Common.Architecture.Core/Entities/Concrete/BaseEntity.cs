using Common.Architecture.Core.Entities.Interface;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Architecture.Core.Entities.Concrete
{
    public class BaseEntity : IEntity
    {
        [Column(Order = 0)]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [Column(Order = 94)]

        public DateTime CreatedAt { get; set; }
        [Column(Order = 95)]

        public Guid? Createdby { get; set; }
        [Column(Order = 96)]

        public DateTime ModifiedAt { get; set; }
        [Column(Order = 97)]

        public Guid? Modifiedby { get; set; }
        [Column(Order = 98)]

        public bool IsActive { get; set; }
        [Column(Order = 99)]

        public bool IsDeleted { get; set; }
    }
}