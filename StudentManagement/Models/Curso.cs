
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace StudentManagement
{
    [Table("Curso")]
    public class Curso
    {
        public Curso()
        {
            Alunoes = new HashSet<Aluno>();
            UnidadeCurriculars = new HashSet<UnidadeCurricular>();
        }

        [Key]
        public int referencia { get; set; }

        public string nome { get; set; }


        public string sigla { get; set; }

        public DateTime dataInicio { get; set; }

        public virtual ICollection<Aluno> Alunoes { get; set; }

        public virtual ICollection<UnidadeCurricular> UnidadeCurriculars { get; set; }
    }
}
