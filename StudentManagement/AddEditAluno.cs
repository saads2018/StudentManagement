using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudentManagement
{
    public partial class AddEditAluno : Form
    {
        MyDbContext db;
        int selectedIndex;
        public AddEditAluno(MyDbContext myDb,int index)
        {
            InitializeComponent();
            selectedIndex = index;

            //Loads courses into the courses dropdown list
            foreach (Curso curso in myDb.Cursoes.ToList())
            {
                comboBox1.Items.Add(curso.nome);
            }

            /*Loads the details of the selected Student(Aluno) if a student has been double clicked, 
             * otherwise loads an add/insert form with empty details */
           
            if(index != -1)
            {
                button4.Visible = true;
                Aluno aluno = myDb.Alunoes.ToList()[index];
                comboBox1.SelectedIndex = myDb.Cursoes.ToList().IndexOf(myDb.Cursoes.ToList().Where(x => x.referencia == aluno.referenciaCurso).FirstOrDefault());
                textBox1.Text = aluno.nomeProprio;
                dateTimePicker1.Text = aluno.dataNascimento.ToString();
                textBox5.Text = aluno.email;
                textBox2.Text = aluno.apelido;
                textBox3.Text=aluno.morada;
                textBox4.Text=aluno.telefone;
                button1.Text = "Saved Image";
                openFileDialog1.FileName = "Saved Image";
            }

            db = myDb;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        //Upload the student picture
        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Images Files(*.PNG;*.JPG)|*.PNG;*.JPG";

            if (openFileDialog1.ShowDialog()==DialogResult.OK)
            {
                button1.Text = openFileDialog1.SafeFileName;
            }
        }

     
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1.SelectedIndex == -1 || textBox1.Text == "" || textBox2.Text == "" || textBox5.Text == "" || textBox3.Text == "" || textBox4.Text == "" || String.IsNullOrWhiteSpace(openFileDialog1.FileName) || openFileDialog1.FileName == "openFileDialog1")
                {
                    MessageBox.Show("Please enter all of the details in the form!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                //Insert a new student into the database
                if (selectedIndex == -1)
                {
                    Aluno aluno = new Aluno();
                    aluno.referenciaCurso = db.Cursoes.ToList().Where(x => x.nome == comboBox1.Text.ToString()).First().referencia;
                    aluno.nomeProprio = textBox1.Text;
                    aluno.dataNascimento = DateTime.Parse(dateTimePicker1.Text);
                    aluno.email = textBox5.Text;
                    aluno.apelido = textBox2.Text;
                    aluno.morada = textBox3.Text;
                    aluno.telefone = textBox4.Text;

                    if (db.Alunoes.ToList().Count == 0)
                        aluno.numero = 0;
                    else
                        aluno.numero = db.Alunoes.ToList()[db.Alunoes.Count() - 1].numero;

                    aluno.numero += 1;
                    aluno.foto = Convert.ToBase64String(File.ReadAllBytes(openFileDialog1.FileName));
                    db.Alunoes.Add(aluno);
                }
                else
                {
                    //Updates the details of the selected student saved in the database.

                    db.Alunoes.ToList()[selectedIndex].referenciaCurso = db.Cursoes.ToList().Where(x => x.nome == comboBox1.Text.ToString()).First().referencia;
                    db.Alunoes.ToList()[selectedIndex].nomeProprio = textBox1.Text;
                    db.Alunoes.ToList()[selectedIndex].dataNascimento = DateTime.Parse(dateTimePicker1.Text);
                    db.Alunoes.ToList()[selectedIndex].email = textBox5.Text;
                    db.Alunoes.ToList()[selectedIndex].apelido = textBox2.Text;
                    db.Alunoes.ToList()[selectedIndex].morada = textBox3.Text;
                    db.Alunoes.ToList()[selectedIndex].telefone = textBox4.Text;

                    if (button1.Text == "Saved Image")
                    {
                        db.Alunoes.ToList()[selectedIndex].foto = db.Alunoes.ToList()[selectedIndex].foto;
                    }
                    else
                        db.Alunoes.ToList()[selectedIndex].foto = Convert.ToBase64String(File.ReadAllBytes(openFileDialog1.FileName));

                    db.Alunoes.Update(db.Alunoes.ToList()[selectedIndex]);
                }

                db.SaveChanges();
                this.Close();
            }
            catch
            {
                    MessageBox.Show("Sorry, an error occured!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Deletes the selected student in the edit form
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                db.Alunoes.Remove(db.Alunoes.ToList()[selectedIndex]);
                db.SaveChanges();
                this.Close();
            }
            catch
            {
                MessageBox.Show("The entry could not be deleted due to a possible foreign key constraint!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }

            

            if(char.IsDigit(e.KeyChar) && textBox4.Text.Length==12)
            {
                e.Handled = true;
                textBox4.Text = textBox4.Text.Substring(0, 12);
            }

        }
    }
}
