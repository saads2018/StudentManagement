using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Runtime.InteropServices;

namespace StudentManagement
{
    public partial class Form1 : Form
    {
        MyDbContext db = new MyDbContext();

        public Form1()
        {
            InitializeComponent();
            tempImg = pictureBox2.Image;
            db.Database.EnsureCreated();   //Establish Connection with database.
            db.Database.OpenConnection();
            radioButton1.Checked=true;
            pictureBox2.BringToFront();
            LoadLists();          
        }


        //Loads Information if any into the dropdown lists.
        private void LoadLists()
        {
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            comboBox3.Items.Clear();
            comboBox4.Items.Clear();
            comboBox5.Items.Clear();
            comboBox6.Items.Clear();

            List<UnidadeCurricular> uni = new List<UnidadeCurricular>();

            foreach (Curso curso in db.Cursoes.ToList())
            {
                comboBox1.Items.Add(curso.nome);
            }
            foreach (Aluno aluno in db.Alunoes.ToList())
            {
                comboBox6.Items.Add(aluno.numero);
            }
            foreach (AnoLetivo anoLetivo in db.AnoLetivoes.ToList())
            {
                comboBox5.Items.Add(anoLetivo.anoInicial + " to " + anoLetivo.anoFinal);
            }
            foreach (UnidadeCurricular unidadeCurricular in db.UnidadeCurriculars.ToList())
            {
                if (uni.Where(x => x.ano == unidadeCurricular.ano).FirstOrDefault() == null)
                {
                    comboBox2.Items.Add(unidadeCurricular.ano);
                    uni.Add(unidadeCurricular);
                }

            }
            uni.Clear();
            foreach (UnidadeCurricular unidadeCurricular in db.UnidadeCurriculars.ToList())
            {
                if (uni.Where(x => x.semestre == unidadeCurricular.semestre).FirstOrDefault() == null)
                {
                    comboBox3.Items.Add(unidadeCurricular.semestre);
                    comboBox4.Items.Add(unidadeCurricular.semestre);
                    uni.Add(unidadeCurricular);
                }

            }

            if(comboBox1.Items.Count==0)
                comboBox1.SelectedIndex = -1;
            else
                comboBox1.SelectedIndex = 0;

            if (comboBox6.Items.Count == 0)
                comboBox6.SelectedIndex = -1;
            else
                comboBox6.SelectedIndex = 0;
        }
        private void button1_Click(object sender, EventArgs e)
        {

        }

        //Exits the application
        private void button3_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }

        //Minimizes the app
        private void button4_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }


        //panel1_Mouse Move is used to drag the app using the mouse
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        //panel1_Mouse Move is used to drag the app using the mouse
        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        //Switches to the student management page - Manage Aluno
        private void ManageStudents()
        {
           
            panel3.Visible = false;
            panel6.Visible = false;
            pictureBox2.Visible = true;
            label12.Visible = true;
            label13.Visible = true; 
            label2.Text = "Manage Students";
            button1.Text = "Add New Student";
            button5.Text = "Delete All Students"; this.alunoBindingSource.DataSource = db.Alunoes.ToList();
            this.Size = new Size(1021, 446);
            label3.Visible = false;
            dataGridView2.Visible = false;
            dataGridView1.DataSource = alunoBindingSource;
            dataGridView1.Columns[8].Visible = false;
            dataGridView1.Columns[9].Visible = false;
            dataGridView1.Columns[10].Visible = false;

            /*Shows the image of the selected student in the list. A default image is 
             * displayed if no image has been saved for the student*/
            try
            {
                if (!String.IsNullOrEmpty(dataGridView1.CurrentRow.Cells[8].Value.ToString()))
                {
                    byte[] byteimg = Convert.FromBase64String(dataGridView1[8,0].Value.ToString());
                    Image img;
                    using (MemoryStream ms = new MemoryStream(byteimg))
                    {
                        img = Image.FromStream(ms);
                    }
                    pictureBox2.Image = img;
                }
            }
            catch
            {
                pictureBox2.Image = tempImg;
            }

            LoadLists();
        }

        //Switches to the years management page - Manage AnoLetivo
        private void ManageYears()
        {
            panel3.Visible = false;
            panel6.Visible = false;
            pictureBox2.Visible = true;
            pictureBox2.Image = tempImg;
            label12.Visible = true;
            label13.Visible = true; this.Size = new Size(1021, 446);
            label3.Visible = false;
            dataGridView2.Visible = false;
            label2.Text = "Manage Years";
            button1.Text = "Add New Academic Year";
            button5.Text = "Delete All Years"; this.anoLetivoBindingSource.DataSource = db.AnoLetivoes.ToList();
            dataGridView1.DataSource = anoLetivoBindingSource;
            dataGridView1.Columns[3].Visible = false;
            LoadLists();
        }

        //Switches to the courses management page - Manage Curso
        private void ManageCourses()
        {
            panel3.Visible = false;
            panel6.Visible = false;
            pictureBox2.Visible = true;
            pictureBox2.Image = tempImg;
            label12.Visible = true;
            label13.Visible = true; this.Size = new Size(1021, 446);
            label3.Visible = false;

            dataGridView2.Visible = false;
            label2.Text = "Manage Courses";
            button1.Text = "Add New Course";
            button5.Text = "Delete All Courses";
            this.cursoBindingSource.DataSource = db.Cursoes.ToList();
            dataGridView1.DataSource = cursoBindingSource;
            dataGridView1.Columns[4].Visible = false;
            dataGridView1.Columns[5].Visible = false;
            LoadLists();

        }

        //Switches to the teacher management page - Manage Docente
        private void ManageTeachers()
        {
            panel3.Visible = false;
            panel6.Visible = false;
            pictureBox2.Visible = true;
            pictureBox2.Image = tempImg;
            label12.Visible = true;
            label13.Visible = true; this.Size = new Size(1021, 446);
            label3.Visible = false;
            dataGridView2.Visible = false;
            label2.Text = "Manage Teachers";
            button1.Text = "Add New Teacher";
            button5.Text = "Delete All Teachers";
            this.docenteBindingSource.DataSource = db.Docentes.ToList();
            dataGridView1.DataSource = docenteBindingSource;
            dataGridView1.Columns[8].Visible = false;
            LoadLists();

        }

        //Switches to the evaluation period management page - Manage EpocaAvaliacao
        private void ManageEvaluationPeriods()
        {
            panel3.Visible = false;
            panel6.Visible = false;
            pictureBox2.Visible = true;
            pictureBox2.Image = tempImg;
            label12.Visible = true;
            label13.Visible = true; this.Size = new Size(1021, 446);
            label3.Visible = false;
            dataGridView2.Visible = false;
            label2.Text = "Manage Evaluation Periods";
            button1.Text = "Add New Evaluation Period";
            button5.Text = "Delete All Periods";
            this.epocaAvaliacaoBindingSource.DataSource = db.EpocaAvaliacaos.ToList();
            dataGridView1.DataSource = epocaAvaliacaoBindingSource;
            dataGridView1.Columns[2].Visible = false;
            LoadLists();

        }

        //Switches to the time state management page - Manage EstadoEpoca
        private void ManageStates()
        {
            panel3.Visible = false;
            panel6.Visible = false;
            pictureBox2.Visible = true;
            pictureBox2.Image = tempImg;
            label12.Visible = true;
            label13.Visible = true; this.Size = new Size(1021, 446);
            label3.Visible = false;
            dataGridView2.Visible = false;
            label2.Text = "Manage Time States";
            button1.Text = "Add New State";
            button5.Text = "Delete All States";
            this.estadoEpocaBindingSource.DataSource = db.EstadoEpocas.ToList();
            dataGridView1.DataSource = estadoEpocaBindingSource;
            dataGridView1.Columns[2].Visible = false;
            LoadLists();

        }

        //Switches to the registrations management page - Manage Inscricao
        private void ManageRegistrations()
        {
            panel3.Visible = false;
            panel6.Visible = false;
            pictureBox2.Visible = true;
            pictureBox2.Image = tempImg;
            label12.Visible = true;
            label13.Visible = true;
            this.Size = new Size(1021, 446);
            label3.Visible = false;
            dataGridView2.Visible = false;
            label2.Text = "Manage Registration";
            button1.Text = "Enroll Student";
            button5.Text = "Delete All Registrations";
            this.inscricaoBindingSource.DataSource = db.Inscricaos.ToList();
            dataGridView1.DataSource = inscricaoBindingSource;
            dataGridView1.Columns[7].Visible = false;
            dataGridView1.Columns[8].Visible = false;
            dataGridView1.Columns[9].Visible = false;
            dataGridView1.Columns[10].Visible = false;
            dataGridView1.Columns[11].Visible = false;
            LoadLists();

        }

        //Switches to the units management page - Manage UnidadeCurricular
        private void ManageUnits()
        {
            panel3.Visible = true;
            panel6.Visible = false;
            pictureBox2.Visible = false;
            pictureBox2.Image = tempImg;
            label12.Visible = false;
            label13.Visible = false;
            label2.Text = "Manage Curriculiar Units";
            button1.Text = "Add New Unit";
            button5.Text = "Delete All Units";
            pictureBox2.Visible = false;
            this.Size = new Size(1021, 755);
            label3.Visible = true;
            dataGridView2.Visible = true;
            List<Inscricao> inscricaos = db.Inscricaos.ToList().Where(x => x.idUnidadeCurricular == db.UnidadeCurriculars.ToList()[0].id).ToList();

            List<Aluno> tempAluno = new List<Aluno>();
            tempAluno.Clear();
            this.alunoBindingSource.Clear();

            foreach (Aluno aluno in db.Alunoes.ToList())
            {
                foreach (Inscricao inscricao in inscricaos)
                {
                    if (aluno.numero == inscricao.numeroAluno && tempAluno.Where(x => x.numero == inscricao.numeroAluno).FirstOrDefault() == null)
                    {
                        this.alunoBindingSource.Add(aluno);
                        tempAluno.Add(aluno);
                    }
                }
            }

            //Fills in the student table below the units table with all the students enrolled in the selected unit
            dataGridView2.DataSource = this.alunoBindingSource;
            dataGridView2.Columns[9].Visible = false;
            dataGridView2.Columns[10].Visible = false;

            this.unidadeCurricularBindingSource.DataSource = db.UnidadeCurriculars.ToList();
            dataGridView1.DataSource = unidadeCurricularBindingSource;
            dataGridView1.Columns[8].Visible = false;
            dataGridView1.Columns[9].Visible = false;
            dataGridView1.Columns[10].Visible = false;
            LoadLists();
        }
        int selectedMenuIndex = 1;

        //Aluno option under manage tables
        private void alunoToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ManageStudents();
            selectedMenuIndex = 1;
        }

        //anoLetivo option under manage tables
        private void anoLetivoToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ManageYears();
            selectedMenuIndex = 2;
        }

        //curso option under manage tables
        private void cursoToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ManageCourses();
            selectedMenuIndex = 3;     
        }

        //docente option under manage tables
        private void docenteToolStripMenuItem1_Click(object sender, EventArgs e)
        {           
            selectedMenuIndex = 4;
            ManageTeachers();
        }

        //epocaAvaliacao option under manage tables
        private void epocaAvaliacaoToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            selectedMenuIndex = 5;
            ManageEvaluationPeriods();
        }

        //estadoEpoca option under manage tables
        private void estadoEpocaToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ManageStates();
            selectedMenuIndex = 6;
        }

        //inscricao option under manage tables
        private void inscricaoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManageRegistrations();
            selectedMenuIndex = 7;
        }

        //unidadeCurricular option under manage tables
        private void unidadeCurToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManageUnits();
            selectedMenuIndex = 8;
        }

        //Opens add/insert form of the selected management page
        private void button1_Click_1(object sender, EventArgs e)
        {
            if (selectedMenuIndex == 1)
            {
                AddEditAluno add = new AddEditAluno(db, -1);
                add.ShowDialog();
                ManageStudents();
            }
            else if (selectedMenuIndex == 2)
            {
                AddEditAnoLetivo add = new AddEditAnoLetivo(db, -1);
                add.ShowDialog();
                ManageYears();
            }
            else if (selectedMenuIndex == 3)
            {
                AddEditCurso add = new AddEditCurso(db, -1);
                add.ShowDialog();
                ManageCourses();
            }
            else if (selectedMenuIndex == 4)
            {
                AddEditDocente add = new AddEditDocente(db, -1);
                add.ShowDialog();
                ManageTeachers();
            }
            else if (selectedMenuIndex == 5)
            {
                AddEditEpocaAvaliacao add = new AddEditEpocaAvaliacao(db, -1);
                add.ShowDialog();
                ManageEvaluationPeriods();
            }
            else if (selectedMenuIndex == 6)
            {
                AddEditEstadoEpoca add = new AddEditEstadoEpoca(db, -1);
                add.ShowDialog();
                ManageStates();
            }
            else if (selectedMenuIndex == 7)
            {
                AddEditInscricao add = new AddEditInscricao(db, -1);
                add.ShowDialog();
                ManageRegistrations();
            }
            else if (selectedMenuIndex == 8)
            {
                AddEditUnidadeCurricular add = new AddEditUnidadeCurricular(db, -1);
                add.ShowDialog();
                ManageUnits();
            }
        }

        //Delete all the entries of selected management page
        private void button5_Click(object sender, EventArgs e)
        {
            if(selectedMenuIndex==1)
            {
                DialogResult result = MessageBox.Show("Do you want to delete all student entries?", "Manage Student", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (result == DialogResult.OK)
                {
                    db.Alunoes.RemoveRange(db.Alunoes.ToList());
                    db.SaveChanges();
                    ManageStudents();
                }         
            }
            else if (selectedMenuIndex == 2)
            {
                DialogResult result = MessageBox.Show("Do you want to delete all academic entries?", "Manage Academic Years", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (result == DialogResult.OK)
                {
                    db.AnoLetivoes.RemoveRange(db.AnoLetivoes.ToList());
                    db.SaveChanges();
                    ManageYears();
                }
            }
            else if(selectedMenuIndex == 3)
            {
                DialogResult result = MessageBox.Show("Do you want to delete all Courses?", "Manage Courses", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (result == DialogResult.OK)
                {
                    db.Cursoes.RemoveRange(db.Cursoes.ToList());
                    db.SaveChanges();
                    ManageCourses();
                }
            }
            else if(selectedMenuIndex == 4)
            {
                DialogResult result = MessageBox.Show("Do you want to delete all Teacher entries?", "Manage Teachers", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (result == DialogResult.OK)
                {
                    db.Docentes.RemoveRange(db.Docentes.ToList());
                    db.SaveChanges();
                    ManageTeachers();
                }
            }
            else if(selectedMenuIndex == 5)
            {
                DialogResult result = MessageBox.Show("Do you want to delete all Evaluation Period entries?", "Manage Evaluation Periods", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (result == DialogResult.OK)
                {
                    db.EpocaAvaliacaos.RemoveRange(db.EpocaAvaliacaos.ToList());
                    db.SaveChanges();
                    ManageEvaluationPeriods();
                }
            }
            else if(selectedMenuIndex == 6)
            {
                DialogResult result = MessageBox.Show("Do you want to delete all State entries?", "Manage States", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (result == DialogResult.OK)
                {
                    db.EstadoEpocas.RemoveRange(db.EstadoEpocas.ToList());
                    db.SaveChanges();
                    ManageStates();
                }
            }
            else if(selectedMenuIndex == 7)
            {
                DialogResult result = MessageBox.Show("Do you want to delete all Registrations?", "Manage Registrations", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (result == DialogResult.OK)
                {
                    db.Inscricaos.RemoveRange(db.Inscricaos.ToList());
                    db.SaveChanges();
                    ManageRegistrations();
                }
            }
            else if(selectedMenuIndex == 8)
            {
                DialogResult result = MessageBox.Show("Do you want to delete all Units?", "Manage Units", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (result == DialogResult.OK)
                {
                    db.UnidadeCurriculars.RemoveRange(db.UnidadeCurriculars.ToList());
                    db.SaveChanges();
                    ManageUnits();
                }
            }
        }


        /*Displays the student image on the right side of the app when one of the students is selected in 
         * the table on the student management page. Shows the filter section if the unidadecurricular page is selected.*/
        Image tempImg;
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(selectedMenuIndex==1)
            {
                try
                {
                    if (!String.IsNullOrEmpty(dataGridView1.CurrentRow.Cells[8].Value.ToString()))
                    {
                        byte[] byteimg = Convert.FromBase64String(dataGridView1.CurrentRow.Cells[8].Value.ToString());
                        Image img;
                        using (MemoryStream ms = new MemoryStream(byteimg))
                        {
                            img = Image.FromStream(ms);
                        }
                        pictureBox2.Image = img;
                    }
                }
                catch
                {
                    pictureBox2.Image = tempImg;
                }

            }
            else if (selectedMenuIndex == 8)
            {
                try
                {
                    if (!String.IsNullOrEmpty(dataGridView1.CurrentRow.Cells[0].Value.ToString()))
                    {
                        List<Inscricao> inscricaos = db.Inscricaos.ToList().Where(x => x.idUnidadeCurricular == Int32.Parse(dataGridView1.CurrentRow.Cells[0].Value.ToString())).ToList();
                        List<Aluno> tempAluno = new List<Aluno>();
                        tempAluno.Clear();
                        this.alunoBindingSource.Clear();

                        foreach (Aluno aluno in db.Alunoes.ToList())
                        {
                            foreach (Inscricao inscricao in inscricaos)
                            {
                                if (aluno.numero == inscricao.numeroAluno && tempAluno.Where(x=>x.numero==inscricao.numeroAluno).FirstOrDefault()==null)
                                {
                                    this.alunoBindingSource.Add(aluno);
                                    tempAluno.Add(aluno);
                                }
                            }
                        }
                        dataGridView2.DataSource = this.alunoBindingSource;
                    }
                }
                catch
                {

                }

            }
        }

        //Opens the edit form of the selected item depending on the management page opened when a row in the table is double clicked.
        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            if(selectedMenuIndex==1)
            {
                DialogResult result = MessageBox.Show("Do you want to update the selected student?", "Manage Student", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (result==DialogResult.OK)
                {
                    AddEditAluno add = new AddEditAluno(db,dataGridView1.CurrentRow.Index);
                    add.ShowDialog();
                    ManageStudents();
                }
            }
            else if(selectedMenuIndex == 2)
            {
                DialogResult result = MessageBox.Show("Do you want to update the selected academic years?", "Manage Academic Years", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (result == DialogResult.OK)
                {
                    AddEditAnoLetivo add = new AddEditAnoLetivo(db,dataGridView1.CurrentRow.Index);
                    add.ShowDialog();
                    ManageYears();
                }
            }
            else if (selectedMenuIndex == 3)
            {
                DialogResult result = MessageBox.Show("Do you want to update the selected Course?", "Manage Courses", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (result == DialogResult.OK)
                {
                    AddEditCurso add = new AddEditCurso(db, dataGridView1.CurrentRow.Index);
                    add.ShowDialog();
                    ManageCourses();
                }
            }
            else if (selectedMenuIndex == 4)
            {
                DialogResult result = MessageBox.Show("Do you want to update the selected teacher?", "Manage Teachers", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (result == DialogResult.OK)
                {
                    AddEditDocente add = new AddEditDocente(db, dataGridView1.CurrentRow.Index);
                    add.ShowDialog();
                    ManageTeachers();
                }
            }
            else if (selectedMenuIndex == 5)
            {
                DialogResult result = MessageBox.Show("Do you want to update the selected Evaluation Period?", "Manage Evaluation Periods", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (result == DialogResult.OK)
                {
                    AddEditEpocaAvaliacao add = new AddEditEpocaAvaliacao(db, dataGridView1.CurrentRow.Index);
                    add.ShowDialog();
                    ManageEvaluationPeriods();
                }
            }
            else if(selectedMenuIndex == 6)
            {
                DialogResult result = MessageBox.Show("Do you want to update the selected State?", "Manage States", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (result == DialogResult.OK)
                {
                    AddEditEstadoEpoca add = new AddEditEstadoEpoca(db, dataGridView1.CurrentRow.Index);
                    add.ShowDialog();
                    ManageStates();
                }
            }
            else if(selectedMenuIndex == 7)
            {
                DialogResult result = MessageBox.Show("Do you want to update the selected Registration?", "Manage Registrations", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (result == DialogResult.OK)
                {
                    AddEditInscricao add = new AddEditInscricao(db, dataGridView1.CurrentRow.Index);
                    add.ShowDialog();
                    ManageRegistrations();
                }
            }
            else if(selectedMenuIndex == 8)
            {
                DialogResult result = MessageBox.Show("Do you want to update the selected Unit?", "Manage Units", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (result == DialogResult.OK)
                {
                    AddEditUnidadeCurricular add = new AddEditUnidadeCurricular(db, dataGridView1.CurrentRow.Index);
                    add.ShowDialog();
                    ManageUnits();
                }
            }
        }

        //Used to filter the units.
        private void button2_Click(object sender, EventArgs e)
        {
            List<UnidadeCurricular> unidadeCurriculars = db.UnidadeCurriculars.ToList();

            unidadeCurriculars = unidadeCurriculars.Where(x => x.referenciaCurso == db.Cursoes.ToList().Where(x => x.nome == comboBox1.Text.ToString()).First().referencia).ToList();
            
            if (comboBox2.SelectedIndex>=0)
            {
                unidadeCurriculars = unidadeCurriculars.Where(x => x.ano == comboBox2.Text.ToString()).ToList();
            }
            if (comboBox3.SelectedIndex >= 0)
            {
                unidadeCurriculars = unidadeCurriculars.Where(x => x.semestre == comboBox3.Text.ToString()).ToList();
            }
            this.unidadeCurricularBindingSource.DataSource = unidadeCurriculars;
            dataGridView1.DataSource = this.unidadeCurricularBindingSource;
        }

        //Clear Filters
        private void button6_Click(object sender, EventArgs e)
        {
            this.unidadeCurricularBindingSource.DataSource = db.UnidadeCurriculars.ToList();
            dataGridView1.DataSource = this.unidadeCurricularBindingSource;
            comboBox2.SelectedIndex = -1;
            comboBox3.SelectedIndex = -1;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        //Switches to view student picture
        private void label13_Click(object sender, EventArgs e)
        {
            panel3.Visible = false;
            panel6.Visible = false;
            pictureBox2.Visible = true;
        }

        //Switches to search for student details
        private void label12_Click(object sender, EventArgs e)
        {
            panel3.Visible = false;
            panel6.Visible = true;
            pictureBox2.Visible = false;
        }

        //Opens the student details form using selected details.
        private void button7_Click(object sender, EventArgs e)
        {
            int check= 0;
            check=radioButton1.Checked ? 1 : radioButton2.Checked? 2:radioButton3.Checked? 3:0;
            int studentNumber = comboBox6.SelectedIndex>=0? comboBox6.SelectedIndex: -1;
            int sYear = comboBox5.SelectedIndex >= 0? comboBox5.SelectedIndex : -1;
            string semester = comboBox4.SelectedIndex >= 0 ? comboBox4.Text.ToString():"";
                
            StudentDetails studentDetails = new StudentDetails(db,studentNumber,sYear,check, semester);
            studentDetails.ShowDialog();
        }

        //Opens the student management page when the app first loads.
        private void Form1_Load_1(object sender, EventArgs e)
        {
            ManageStudents();
            this.Location=new Point(this.Location.X,this.Location.Y-100);
        }

        //Clear the details selected for the search student section
        private void button8_Click(object sender, EventArgs e)
        {
            comboBox5.SelectedIndex = -1;
            comboBox4.SelectedIndex = -1;
        }
    }
}   