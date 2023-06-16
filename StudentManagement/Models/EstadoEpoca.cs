
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace StudentManagement
{
    [Table("EstadoEpoca")]
    public class EstadoEpoca
    {
        public EstadoEpoca()
        {
            Inscricaos = new HashSet<Inscricao>();
        }

        [Key]
        public short id { get; set; }

        public string descricao { get; set; }

        public virtual ICollection<Inscricao> Inscricaos { get; set; }
    }

}
