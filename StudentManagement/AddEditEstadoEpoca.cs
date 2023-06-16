using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
using TextBox = System.Windows.Forms.TextBox;

namespace StudentManagement
{
    public partial class AddEditEstadoEpoca : Form
    {
        MyDbContext db;
        int selectedIndex;
        public AddEditEstadoEpoca(MyDbContext myDb, int index)
        {
            InitializeComponent();
            selectedIndex = index;


            /*Loads the details of the selected Time State(EstadoEpoca) if a time state has been double clicked, 
           * otherwise loads an add/insert form with empty details */
            if (index != -1)
            {
                button4.Visible = true;
                EstadoEpoca state = myDb.EstadoEpocas.ToList()[index];
                textBox1.Text = state.id.ToString();
                textBox2.Text = state.descricao;
                textBox1.Enabled = false;
            }

            db = myDb;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Deletes the selected state in the edit form
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                db.EstadoEpocas.Remove(db.EstadoEpocas.ToList()[selectedIndex]);
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

                if (textBox1.Text == "" || textBox2.Text == "")
                {
                    MessageBox.Show("Please enter all of the details in the form!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (selectedIndex == -1 && db.EstadoEpocas.Where(x => x.id == Int32.Parse(textBox1.Text)).FirstOrDefault() != null)
                {
                    MessageBox.Show("Please enter a unique id!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                //Insert a new state into the database

                if (selectedIndex == -1)
                {
                    EstadoEpoca estadoEpoca = new EstadoEpoca();
                    estadoEpoca.id = short.Parse(textBox1.Text);
                    estadoEpoca.descricao = textBox2.Text;

                    db.EstadoEpocas.Add(estadoEpoca);
                }
                else
                {
                    //Updates the details of the selected state saved in the database.

                    db.EstadoEpocas.ToList()[selectedIndex].id = short.Parse(textBox1.Text);
                    db.EstadoEpocas.ToList()[selectedIndex].descricao = textBox2.Text;

                    db.EstadoEpocas.Update(db.EstadoEpocas.ToList()[selectedIndex]);
                }

                db.SaveChanges();
                this.Close();
            }
            catch
            {
                MessageBox.Show("Sorry, an error occured!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }

          

            if (char.IsDigit(e.KeyChar) && (sender as TextBox).Text.Length == 4)
            {
                e.Handled = true;
                (sender as TextBox).Text = (sender as TextBox).Text.Substring(0, 4);
            }

        }
    }
}
