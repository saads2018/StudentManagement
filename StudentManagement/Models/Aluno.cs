
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace StudentManagement
{

    [Table("Aluno")]
    public class Aluno
    {
        public Aluno()
        {
            Inscricaos = new HashSet<Inscricao>();
        }

        [Key]
        public int numero { get; set; }

        public int referenciaCurso { get; set; }


        public string nomeProprio { get; set; }

        public string apelido { get; set; }

        public DateTime dataNascimento { get; set; }

        public string morada { get; set; }

        public string email { get; set; }

        public string? telefone { get; set; }

        [Column(TypeName = "image")]
        public string? foto { get; set; }

        public virtual Curso Curso { get; set; }

        public virtual ICollection<Inscricao> Inscricaos { get; set; }
    }
}
