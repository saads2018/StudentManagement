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
    public partial class AddEditAnoLetivo : Form
    {
        MyDbContext db;
        int selectedIndex;
        public AddEditAnoLetivo(MyDbContext myDb, int index)
        {
            InitializeComponent();
            selectedIndex = index;

            /*Loads the details of the selected Year(AnoLetivo) if a year has been double clicked, 
            * otherwise loads an add/insert form with empty details */
            if (index != -1)
            {
                button4.Visible = true;
                AnoLetivo anoLetivo = myDb.AnoLetivoes.ToList()[index];
                textBox1.Text = anoLetivo.anoInicial.ToString();
                textBox2.Text = anoLetivo.anoFinal.ToString();
            }

            db = myDb;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AddEditAnoLetivo_Load(object sender, EventArgs e)
        {

        }

        //Deletes the selected year in the edit form
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                db.AnoLetivoes.Remove(db.AnoLetivoes.ToList()[selectedIndex]);
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

                //Insert a new year into the database
                if (selectedIndex == -1)
                {
                    AnoLetivo anoLetivo = new AnoLetivo();

                    if (db.AnoLetivoes.ToList().Count == 0)
                        anoLetivo.id = 0;
                    else
                        anoLetivo.id = db.AnoLetivoes.ToList()[db.AnoLetivoes.Count() - 1].id;

                    anoLetivo.id += 1;
                    anoLetivo.anoInicial = short.Parse(textBox1.Text);
                    anoLetivo.anoFinal = short.Parse(textBox2.Text);
                    db.AnoLetivoes.Add(anoLetivo);
                }
                else
                {
                    //Updates the details of the selected year saved in the database.

                    db.AnoLetivoes.ToList()[selectedIndex].anoInicial = short.Parse(textBox1.Text);
                    db.AnoLetivoes.ToList()[selectedIndex].anoFinal = short.Parse(textBox2.Text);

                    db.AnoLetivoes.Update(db.AnoLetivoes.ToList()[selectedIndex]);
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

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) )
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
