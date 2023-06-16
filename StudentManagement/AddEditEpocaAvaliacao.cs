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
    public partial class AddEditEpocaAvaliacao : Form
    {
        MyDbContext db;
        int selectedIndex;
        public AddEditEpocaAvaliacao(MyDbContext myDb, int index)
        {
            InitializeComponent();
            selectedIndex = index;

            /*Loads the details of the selected Evaluation Period(EpocaAvaliacao) if a student has been double clicked, 
            * otherwise loads an add/insert form with empty details */
            if (index != -1)
            {
                button4.Visible = true;
                EpocaAvaliacao epoca = myDb.EpocaAvaliacaos.ToList()[index];
                textBox1.Text = epoca.id;
                textBox2.Text = epoca.descricao;
                textBox1.Enabled = false;
            }

            db = myDb;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Deletes the selected evaluation period in the edit form
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                db.EpocaAvaliacaos.Remove(db.EpocaAvaliacaos.ToList()[selectedIndex]);
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

                if (selectedIndex==-1 && db.EpocaAvaliacaos.Where(x=>x.id==textBox1.Text).FirstOrDefault()!=null)
                {
                    MessageBox.Show("Please enter a unique id!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                //Insert a new evaluation period into the database

                if (selectedIndex == -1)
                {
                    EpocaAvaliacao epocaAvaliacao = new EpocaAvaliacao();
                    epocaAvaliacao.id = textBox1.Text;
                    epocaAvaliacao.descricao = textBox2.Text;

                    db.EpocaAvaliacaos.Add(epocaAvaliacao);
                }
                else
                {
                    //Updates the details of the selected evaluation period saved in the database.

                    db.EpocaAvaliacaos.ToList()[selectedIndex].id = textBox1.Text;
                    db.EpocaAvaliacaos.ToList()[selectedIndex].descricao = textBox2.Text;

                    db.EpocaAvaliacaos.Update(db.EpocaAvaliacaos.ToList()[selectedIndex]);
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
            if (e.KeyChar != (char)Keys.Back &&(sender as TextBox).Text.Length == 4)
            {
                e.Handled = true;
                (sender as TextBox).Text = (sender as TextBox).Text.Substring(0, 4);
            }
        }
    }
}
