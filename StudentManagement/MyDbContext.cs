using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace StudentManagement
{
    //MyDbContext Is used to connect to the database using connection string in appsettings.json
    public class MyDbContext : DbContext
    {
       
        /*The variables are created so that the tables in the database can be accessed from the 
         * forms in the application.
         */
        public virtual Microsoft.EntityFrameworkCore.DbSet<Aluno> Alunoes { get; set; }
        public virtual Microsoft.EntityFrameworkCore.DbSet<AnoLetivo> AnoLetivoes { get; set; }
        public virtual Microsoft.EntityFrameworkCore.DbSet<Curso> Cursoes { get; set; }
        public virtual Microsoft.EntityFrameworkCore.DbSet<Docente> Docentes { get; set; }
        public virtual Microsoft.EntityFrameworkCore.DbSet<EpocaAvaliacao> EpocaAvaliacaos { get; set; }
        public virtual Microsoft.EntityFrameworkCore.DbSet<EstadoEpoca> EstadoEpocas { get; set; }
        public virtual Microsoft.EntityFrameworkCore.DbSet<Inscricao> Inscricaos { get; set; }
        public virtual Microsoft.EntityFrameworkCore.DbSet<UnidadeCurricular> UnidadeCurriculars { get; set; }

        /*The method below is used to configure the connection string
         * of dbContext. The connection string can be edited in the file
         called appsettings.json.*/
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Program.Configuration.GetConnectionString("Default"));
        }

        /*Used to edit the variables/properties of the model classes to ensure data is 
         * inserted or retrieved from the database without any errors.*/
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<Aluno>()
                .Property(e => e.telefone)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Aluno>()
                .HasMany(e => e.Inscricaos)
                .WithOne(e => e.Aluno)
                .IsRequired()
                .HasForeignKey(e => e.numeroAluno)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AnoLetivo>()
                .HasMany(e => e.Inscricaos)
                .WithOne(e => e.AnoLetivo)
                .IsRequired()
                .HasForeignKey(e => e.idAnoLetivo)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Curso>()
                .HasMany(e => e.Alunoes)
                .WithOne(e => e.Curso)
                .IsRequired()
                .HasForeignKey(e => e.referenciaCurso)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Curso>()
                .HasMany(e => e.UnidadeCurriculars)
                .WithOne(e => e.Curso)
                .IsRequired()
                .HasForeignKey(e => e.referenciaCurso)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Docente>()
                .Property(e => e.telefone)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Docente>()
                .Property(e => e.extensao)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Docente>()
                .Property(e => e.salario)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Docente>()
                .HasMany(e => e.UnidadeCurriculars)
                .WithOne(e => e.Docente)
                .IsRequired()
                .HasForeignKey(e => e.numeroDocente)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EpocaAvaliacao>()
                .Property(e => e.id)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<EpocaAvaliacao>()
                .HasMany(e => e.Inscricaos)
                .WithOne(e => e.EpocaAvaliacao)
                .IsRequired()
                .HasForeignKey(e => e.idEpocaAvaliacao)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EstadoEpoca>()
                .HasMany(e => e.Inscricaos)
                .WithOne(e => e.EstadoEpoca)
                .HasForeignKey(e => e.idEstadoEpoca);

            modelBuilder.Entity<Inscricao>()
                .HasKey(e => new { e.numeroAluno, e.idUnidadeCurricular, e.idAnoLetivo, e.idEpocaAvaliacao });

            modelBuilder.Entity<Inscricao>()
                .Property(e => e.idEpocaAvaliacao)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Inscricao>()
                .Property(e => e.presenca)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<UnidadeCurricular>()
                .Property(e => e.creditos)
                .HasPrecision(10, 2);

            modelBuilder.Entity<UnidadeCurricular>()
                .Property(e => e.ano)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<UnidadeCurricular>()
                .Property(e => e.semestre)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<UnidadeCurricular>()
                .HasMany(e => e.Inscricaos)
                .WithOne(e => e.UnidadeCurricular)
                .IsRequired()
                .HasForeignKey(e => e.idUnidadeCurricular)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
