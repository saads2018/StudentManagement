
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace StudentManagement
{

    [Table("AnoLetivo")]
    public class AnoLetivo
    {
        public AnoLetivo()
        {
            Inscricaos = new HashSet<Inscricao>();
        }

        [Key]
        public short id { get; set; }

        public short anoInicial { get; set; }

        public short anoFinal { get; set; }

        public virtual ICollection<Inscricao> Inscricaos { get; set; }
    }
}
