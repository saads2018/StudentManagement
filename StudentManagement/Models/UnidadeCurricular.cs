
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace StudentManagement
{

    [Table("UnidadeCurricular")]
    public class UnidadeCurricular
    {
        public UnidadeCurricular()
        {
            Inscricaos = new HashSet<Inscricao>();
        }

        [Key]
        public int id { get; set; }

        public int referenciaCurso { get; set; }

        public int numeroDocente { get; set; }

        public string nome { get; set; }

        public string sigla { get; set; }

        public decimal creditos { get; set; }

        public string ano { get; set; }

        public string semestre { get; set; }

        public virtual Curso Curso { get; set; }

        public virtual Docente Docente { get; set; }

        public virtual ICollection<Inscricao> Inscricaos { get; set; }
    }
}
