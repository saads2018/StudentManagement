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
    public partial class AddEditDocente : Form
    {
        MyDbContext db;
        int selectedIndex;
        public AddEditDocente(MyDbContext myDb, int index)
        {
            InitializeComponent();
            selectedIndex = index;

            /*Loads the details of the selected Teacher(Docente) if a teacher has been double clicked, 
            * otherwise loads an add/insert form with empty details */
            if (index != -1)
            {
                button4.Visible = true;
                Docente docente = myDb.Docentes.ToList()[index];
                textBox1.Text = docente.nomeProprio;
                dateTimePicker1.Text = docente.dataNascimento.ToString();
                textBox5.Text = docente.email;
                textBox2.Text = docente.apelido;
                textBox3.Text = docente.salario.ToString();
                textBox4.Text = docente.telefone;
                textBox6.Text = docente.extensao;
            }

            db = myDb;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Deletes the selected teacher in the edit form
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                db.Docentes.Remove(db.Docentes.ToList()[selectedIndex]);
                db.SaveChanges();
                this.Close();
            }
            catch
            {
                MessageBox.Show("The entry could be deleted due to a possible foreign key constraint!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                //Insert a new teacher into the database

                if (textBox1.Text == "" || textBox2.Text == "" || textBox5.Text == "" || textBox3.Text == "" || textBox4.Text == "" || textBox4.Text == "" || textBox6.Text=="")
                {
                    MessageBox.Show("Please enter all of the details in the form!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (selectedIndex == -1)
                {
                    Docente docente = new Docente();
                    docente.nomeProprio = textBox1.Text;
                    docente.dataNascimento = DateTime.Parse(dateTimePicker1.Text);
                    docente.email = textBox5.Text;
                    docente.apelido = textBox2.Text;
                    docente.salario = Decimal.Parse(textBox3.Text);
                    docente.telefone = textBox4.Text;
                    docente.extensao = textBox6.Text;

                    if (db.Docentes.ToList().Count == 0)
                        docente.numero = 0;
                    else
                        docente.numero = db.Docentes.ToList()[db.Docentes.Count() - 1].numero;

                    docente.numero += 1;
                    db.Docentes.Add(docente);
                }
                else
                {
                    //Updates the details of the selected teacher saved in the database.

                    db.Docentes.ToList()[selectedIndex].nomeProprio = textBox1.Text;
                    db.Docentes.ToList()[selectedIndex].dataNascimento = DateTime.Parse(dateTimePicker1.Text);
                    db.Docentes.ToList()[selectedIndex].email = textBox5.Text;
                    db.Docentes.ToList()[selectedIndex].apelido = textBox2.Text;
                    db.Docentes.ToList()[selectedIndex].salario = Decimal.Parse(textBox3.Text);
                    db.Docentes.ToList()[selectedIndex].telefone = textBox4.Text;
                    db.Docentes.ToList()[selectedIndex].extensao = textBox6.Text;


                }
                db.SaveChanges();
                this.Close();

            }
            catch
            {
                MessageBox.Show("Sorry, an error occured!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
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

            if (char.IsDigit(e.KeyChar) && (sender as TextBox).Text.Length == 8 && (sender as TextBox).Text.IndexOf('.')==-1)
            {
                    e.Handled = true;
                    (sender as TextBox).Text = (sender as TextBox).Text.Substring(0, 8);
            }
            else if(char.IsDigit(e.KeyChar)  && (sender as TextBox).Text.IndexOf('.') != -1 &&(sender as TextBox).Text.Length == (sender as TextBox).Text.Substring(0, (sender as TextBox).Text.IndexOf('.')).Length+3)
            {
                e.Handled = true;
                (sender as TextBox).Text = (sender as TextBox).Text.Substring(0, (sender as TextBox).Text.Substring(0, (sender as TextBox).Text.IndexOf('.')).Length + 3);
            }
                  

        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }

          

            if (char.IsDigit(e.KeyChar) && (sender as TextBox).Text.Length == 12)
            {
                e.Handled = true;
                (sender as TextBox).Text = (sender as TextBox).Text.Substring(0, 12);
            }
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }

           

            if (char.IsDigit(e.KeyChar) && (sender as TextBox).Text.Length == 5)
            {
                e.Handled = true;
                (sender as TextBox).Text = (sender as TextBox).Text.Substring(0, 5);
            }
        }

        private void textBox3_MouseDown(object sender, MouseEventArgs e)
        {
            textBox3.SelectionStart = textBox3.Text.Length;
            textBox3.SelectionLength = 0;
        }
    }
}
