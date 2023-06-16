
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace StudentManagement
{
    [Table("Docente")]
    public class Docente
    {
        public Docente()
        {
            UnidadeCurriculars = new HashSet<UnidadeCurricular>();
        }

        [Key]
        public int numero { get; set; }

        public string nomeProprio { get; set; }

        public string apelido { get; set; }

        public DateTime? dataNascimento { get; set; }

        public string? email { get; set; }

        public string? telefone { get; set; }

        public string? extensao { get; set; }

        public decimal salario { get; set; }

        public virtual ICollection<UnidadeCurricular> UnidadeCurriculars { get; set; }
    }
}
