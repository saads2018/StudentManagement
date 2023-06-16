
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace StudentManagement
{
    [Table("Inscricao")]
    public partial class Inscricao
    {
        [Key]        
        public int numeroAluno { get; set; }

        [Key]
        public int idUnidadeCurricular { get; set; }

        [Key]
        public short idAnoLetivo { get; set; }

        [Key]
        public string idEpocaAvaliacao { get; set; }

        public short? idEstadoEpoca { get; set; }

        public string? presenca { get; set; }

        public short? nota { get; set; }

        public virtual Aluno Aluno { get; set; }

        public virtual AnoLetivo AnoLetivo { get; set; }

        public virtual EpocaAvaliacao EpocaAvaliacao { get; set; }

        public virtual EstadoEpoca EstadoEpoca { get; set; }

        public virtual UnidadeCurricular UnidadeCurricular { get; set; }
    }
}
