
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace StudentManagement
{
    [Table("EpocaAvaliacao")]
    public class EpocaAvaliacao
    {
        public EpocaAvaliacao()
        {
            Inscricaos = new HashSet<Inscricao>();
        }

        [Key]
        public string id { get; set; }

        public string descricao { get; set; }

        public virtual ICollection<Inscricao> Inscricaos { get; set; }
    }
}
