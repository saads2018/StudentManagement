using StudentManagement.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudentManagement
{
    public partial class StudentDetails : Form
    {
        MyDbContext db;
        int studentNo;
        int startY;
        int enroll;
        string sem;

        //Initializes the variables of the class.
        public StudentDetails(MyDbContext db, int studentNumber, int startYear, int enrollOption, string semester)
        {
            InitializeComponent();
            this.db = db;
            this.studentNo = studentNumber;
            this.startY = startYear;
            this.enroll = enrollOption;
            this.sem = semester;
        }

        //Uses the searched student details to load the form with the appropriate details.
        private void StudentDetails_Load(object sender, EventArgs e)
        {
            List<Student> students = new List<Student>();

            label4.Text = db.Alunoes.ToList()[studentNo].nomeProprio + " " + db.Alunoes.ToList()[studentNo].apelido;
            
            //Loads all the registrations related to the selected student number.
            foreach (Inscricao inscricao in db.Inscricaos.ToList())
            {
                Student student = new Student();
                if (inscricao.numeroAluno == db.Alunoes.ToList()[studentNo].numero)
                {
                    student.UnitName = db.UnidadeCurriculars.ToList().Where(x => x.id == inscricao.idUnidadeCurricular).FirstOrDefault().nome;
                    student.AcademicYear = db.AnoLetivoes.ToList().Where(x => x.id == inscricao.idAnoLetivo).FirstOrDefault().anoInicial.ToString() + " to "+ db.AnoLetivoes.ToList().Where(x => x.id == inscricao.idAnoLetivo).FirstOrDefault().anoFinal.ToString();
                    student.AssessmentPeriod = db.EpocaAvaliacaos.ToList().Where((x) => x.id == inscricao.idEpocaAvaliacao).FirstOrDefault().descricao;
                    student.EnrollmentStatus = db.EstadoEpocas.ToList().Where(x => x.id == inscricao.idEstadoEpoca).FirstOrDefault().descricao;
                    student.Note = inscricao.nota.ToString();
                    student.SeasonStatus = inscricao.presenca;

                    students.Add(student);
                }
            }

            //Filters the selected academic year
            if (startY != -1)
                students = students.Where(x => x.AcademicYear == db.AnoLetivoes.ToList()[startY].anoInicial + " to " + db.AnoLetivoes.ToList()[startY].anoFinal).ToList();


            //Filters the selected semester
            if (sem != "")
            {
                List<UnidadeCurricular> unidades = db.UnidadeCurriculars.ToList().Where(x => x.semestre == sem).ToList();
                List<Student> tempStudents = new List<Student>();

                foreach(Student student in students)
                {
                    foreach(UnidadeCurricular unidadeCurricular in unidades)
                    {
                        if (student.UnitName == unidadeCurricular.nome)
                            tempStudents.Add(student);
                    }
                }

                students = tempStudents;
            }

            string notEnrolled = db.EstadoEpocas.ToList()[1].descricao;

            //Filters units in which the student has been enrolled in.
            if (enroll==2)
            {
                students=students.Where(x => x.EnrollmentStatus != notEnrolled).ToList();
            }
            //Filters units in which the student has not been enrolled in.
            else if (enroll==3)
            {
                students=students.Where(x => x.EnrollmentStatus == notEnrolled).ToList();
            }

            this.studentBindingSource.DataSource = students;
            dataGridView1.DataSource = this.studentBindingSource;
        }
        private void button3_Click(object sender, EventArgs e)
            {
                this.Close();
            }
        }
    }
