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
    public partial class AddEditUnidadeCurricular : Form
    {
        MyDbContext db;
        int selectedIndex;
        public AddEditUnidadeCurricular(MyDbContext myDb, int index)
        {
            InitializeComponent();
            selectedIndex = index;

            //Loads courses into the courses dropdown list
            foreach (Curso curso in myDb.Cursoes.ToList())
            {
                comboBox1.Items.Add(curso.nome);
            }

            //Loads teachers into the teacher dropdown list
            foreach (Docente docente in myDb.Docentes.ToList())
            {
                comboBox2.Items.Add(docente.nomeProprio +" "+docente.apelido);
            }


            /*Loads the details of the selected Unit(UnidadeCurricular) if a unit has been double clicked, 
            * otherwise loads an add/insert form with empty details */

            if (index != -1)
            {
                button4.Visible = true;
                UnidadeCurricular unidadeCurricular = myDb.UnidadeCurriculars.ToList()[index];
                comboBox1.SelectedIndex = myDb.Cursoes.ToList().IndexOf(myDb.Cursoes.ToList().Where(x => x.referencia == unidadeCurricular.referenciaCurso).FirstOrDefault());
                comboBox2.SelectedIndex = myDb.Docentes.ToList().IndexOf(myDb.Docentes.ToList().Where(x => x.numero == unidadeCurricular.numeroDocente).FirstOrDefault());
                textBox1.Text = unidadeCurricular.nome;
                textBox5.Text = unidadeCurricular.creditos.ToString();
                textBox2.Text = unidadeCurricular.sigla;
                textBox3.Text = unidadeCurricular.ano;
                textBox4.Text = unidadeCurricular.semestre;
            }

            db = myDb;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Deletes the selected unit in the edit form
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                db.UnidadeCurriculars.Remove(db.UnidadeCurriculars.ToList()[selectedIndex]);
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

                //Insert a new unit into the database
                if (comboBox1.SelectedIndex == -1 || comboBox2.SelectedIndex == -1 || textBox1.Text == "" || textBox2.Text == "" || textBox5.Text == "" || textBox3.Text == "" || textBox4.Text == "")
                {
                    MessageBox.Show("Please enter all of the details in the form!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (selectedIndex == -1)
                {
                    UnidadeCurricular unidadeCurricular = new UnidadeCurricular();
                    unidadeCurricular.referenciaCurso = db.Cursoes.ToList().Where(x => x.nome == comboBox1.Text.ToString()).First().referencia;
                    unidadeCurricular.numeroDocente = db.Docentes.ToList().Where(x => x.nomeProprio + " " + x.apelido == comboBox2.Text.ToString()).First().numero;
                    unidadeCurricular.nome = textBox1.Text;
                    unidadeCurricular.creditos = Decimal.Parse(textBox5.Text);
                    unidadeCurricular.sigla = textBox2.Text;
                    unidadeCurricular.ano = textBox3.Text;
                    unidadeCurricular.semestre = textBox4.Text;

                    if (db.UnidadeCurriculars.ToList().Count == 0)
                        unidadeCurricular.id = 0;
                    else
                        unidadeCurricular.id = db.UnidadeCurriculars.ToList()[db.UnidadeCurriculars.Count() - 1].id;


                    unidadeCurricular.id += 1;

                    db.UnidadeCurriculars.Add(unidadeCurricular);
                }
                else
                {
                    //Updates the details of the selected unit saved in the database.

                    db.UnidadeCurriculars.ToList()[selectedIndex].referenciaCurso = db.Cursoes.ToList().Where(x => x.nome == comboBox1.Text.ToString()).First().referencia;
                    db.UnidadeCurriculars.ToList()[selectedIndex].numeroDocente = db.Docentes.ToList().Where(x => x.nomeProprio + " " + x.apelido == comboBox2.Text.ToString()).First().numero;
                    db.UnidadeCurriculars.ToList()[selectedIndex].nome = textBox1.Text;
                    db.UnidadeCurriculars.ToList()[selectedIndex].creditos = Decimal.Parse(textBox5.Text);
                    db.UnidadeCurriculars.ToList()[selectedIndex].sigla = textBox2.Text;
                    db.UnidadeCurriculars.ToList()[selectedIndex].ano = textBox3.Text;
                    db.UnidadeCurriculars.ToList()[selectedIndex].semestre = textBox4.Text;

                    db.UnidadeCurriculars.Update(db.UnidadeCurriculars.ToList()[selectedIndex]);
                }

                db.SaveChanges();
                this.Close();
            }
            catch
            {
                MessageBox.Show("Sorry, an error occured!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }   

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }

            if (char.IsDigit(e.KeyChar) && (sender as TextBox).Text.Length == 13)
            {
                e.Handled = true;
                (sender as TextBox).Text = (sender as TextBox).Text.Substring(0, 13);
            }

            if (char.IsDigit(e.KeyChar) && (sender as TextBox).Text.Length == 8 && (sender as TextBox).Text.IndexOf('.') == -1)
            {
                e.Handled = true;
                (sender as TextBox).Text = (sender as TextBox).Text.Substring(0, 8);
            }
            else if (char.IsDigit(e.KeyChar) && (sender as TextBox).Text.IndexOf('.') != -1 && (sender as TextBox).Text.Length == (sender as TextBox).Text.Substring(0, (sender as TextBox).Text.IndexOf('.')).Length + 3)
            {
                e.Handled = true;
                (sender as TextBox).Text = (sender as TextBox).Text.Substring(0, (sender as TextBox).Text.Substring(0, (sender as TextBox).Text.IndexOf('.')).Length + 3);
            }


        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }

            if (char.IsDigit(e.KeyChar) && (sender as TextBox).Text.Length == 1)
            {
                e.Handled = true;
                (sender as TextBox).Text = (sender as TextBox).Text.Substring(0, 1);
            }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }

            if (char.IsDigit(e.KeyChar) && (sender as TextBox).Text.Length == 1)
            {
                e.Handled = true;
                (sender as TextBox).Text = (sender as TextBox).Text.Substring(0, 1);
            }
        }

        private void textBox3_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Back &&  (sender as TextBox).Text.Length == 10)
            {
                e.Handled = true;
                (sender as TextBox).Text = (sender as TextBox).Text.Substring(0, 10);
            }
        }

        private void textBox5_MouseDown(object sender, MouseEventArgs e)
        {
            textBox5.SelectionStart = textBox5.Text.Length;
            textBox5.SelectionLength = 0;
        }
    }
}
