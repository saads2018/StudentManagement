using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;
using TextBox = System.Windows.Forms.TextBox;

namespace StudentManagement
{
    public partial class AddEditCurso : Form
    {
        MyDbContext db;
        int selectedIndex;
        public AddEditCurso(MyDbContext myDb, int index)
        {
            InitializeComponent();
            selectedIndex = index;

            /*Loads the details of the selected Course(Curso) if a course has been double clicked, 
             * otherwise loads an add/insert form with empty details */
            if (index != -1)
            {
                button4.Visible = true;
                Curso curso = myDb.Cursoes.ToList()[index];
                textBox1.Text = curso.nome;
                dateTimePicker1.Text = curso.dataInicio.ToString();
                textBox2.Text = curso.sigla;
                
            }

            db = myDb;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Deletes the selected course in the edit form
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                db.Cursoes.Remove(db.Cursoes.ToList()[selectedIndex]);
                db.SaveChanges();
                this.Close();
            }
            catch
            {
                MessageBox.Show("The entry could not be deleted due to a possible foreign key constraint!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                //Insert a new course into the database

                if (textBox1.Text == "" || textBox2.Text == "")
                {
                    MessageBox.Show("Please enter all of the details in the form!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (selectedIndex == -1)
                {
                    Curso curso = new Curso();
                    curso.nome = textBox1.Text;
                    curso.dataInicio = DateTime.Parse(dateTimePicker1.Text);
                    curso.sigla = textBox2.Text;

                    if (db.Cursoes.ToList().Count == 0)
                        curso.referencia = 0;
                    else
                        curso.referencia = db.Cursoes.ToList()[db.Cursoes.Count() - 1].referencia;

                    curso.referencia += 1;
                    db.Cursoes.Add(curso);
                }
                else
                {
                    //Updates the details of the selected course saved in the database.

                    db.Cursoes.ToList()[selectedIndex].nome = textBox1.Text;
                    db.Cursoes.ToList()[selectedIndex].dataInicio = DateTime.Parse(dateTimePicker1.Text);
                    db.Cursoes.ToList()[selectedIndex].sigla = textBox2.Text;

                    db.Cursoes.Update(db.Cursoes.ToList()[selectedIndex]);
                }

             

                db.SaveChanges();
                this.Close();
            }
            catch
            {
                MessageBox.Show("Sorry, an error occured!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddEditCurso_Load(object sender, EventArgs e)
        {

        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar!=(char)Keys.Back && (sender as TextBox).Text.Length == 10 )
            {
                e.Handled = true;
                (sender as TextBox).Text = (sender as TextBox).Text.Substring(0, 10);
            }
        }
    }
}
