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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;
using TextBox = System.Windows.Forms.TextBox;

namespace StudentManagement
{
    public partial class AddEditInscricao : Form
    {
        MyDbContext db;
        int selectedIndex;
        List<Inscricao> regList;
        List<UnidadeCurricular> unitList;
        List<Aluno> studentList;
        List<AnoLetivo> yearList;
        List<EpocaAvaliacao> epochList;
        List<EstadoEpoca> estadosList;

        public AddEditInscricao(MyDbContext myDb, int index)
        {
            InitializeComponent();
            init(myDb, index);
            LoadLists();
            checkFirstInstance();
            LoadItem();
        }

        /*Loads the details of the selected Registration(Inscricao) if a registration has been double clicked, 
             * otherwise loads an add/insert form with empty details */
        private void LoadItem()
        {
            if (selectedIndex != -1)
            {
                button4.Visible = true;

                comboBox1.SelectedIndex = db.AnoLetivoes.ToList().IndexOf(db.AnoLetivoes.ToList().Where(x => x.id == regList[selectedIndex].idAnoLetivo).FirstOrDefault());
                comboBox2.SelectedIndex = db.Alunoes.ToList().IndexOf(db.Alunoes.ToList().Where(x => x.numero == regList[selectedIndex].numeroAluno).FirstOrDefault());
                comboBox3.SelectedIndex = db.UnidadeCurriculars.ToList().IndexOf(db.UnidadeCurriculars.ToList().Where(x => x.id == regList[selectedIndex].idUnidadeCurricular).FirstOrDefault());
                comboBox4.Text = epochList[db.EpocaAvaliacaos.ToList().IndexOf(db.EpocaAvaliacaos.ToList().Where(x => x.id == regList[selectedIndex].idEpocaAvaliacao).FirstOrDefault())].descricao;
                comboBox5.SelectedIndex = db.EstadoEpocas.ToList().IndexOf(db.EstadoEpocas.ToList().Where(x => x.id == regList[selectedIndex].idEstadoEpoca).FirstOrDefault());
                comboBox6.Text = regList[selectedIndex].presenca;
                textBox3.Text = regList[selectedIndex].nota.ToString();
            }
        }

        //Initializes the variables of the class
        private void init(MyDbContext tempDb, int index)
        {
            db = tempDb;
            unitList = db.UnidadeCurriculars.ToList();
            studentList = db.Alunoes.ToList();
            yearList = db.AnoLetivoes.ToList();
            regList = db.Inscricaos.ToList();
            epochList = db.EpocaAvaliacaos.ToList();
            estadosList = db.EstadoEpocas.ToList();
            selectedIndex = index;
        }

        //Loads the dropdown lists with the relevant information.
        private void LoadLists()
        {
            foreach (Aluno aluno in db.Alunoes.ToList())
            {
                comboBox2.Items.Add(aluno.nomeProprio + " " + aluno.apelido);
            }
            foreach (UnidadeCurricular unidade in db.UnidadeCurriculars.ToList())
            {
                comboBox3.Items.Add(unidade.nome);
            }
            foreach (AnoLetivo ano in db.AnoLetivoes.ToList())
            {
                comboBox1.Items.Add(ano.anoInicial.ToString() + " to " + ano.anoFinal.ToString());
            }

            foreach (EstadoEpoca estado in db.EstadoEpocas.ToList())
            {
                comboBox5.Items.Add(estado.descricao);
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Checks if the student is registering into the unit for the first time.
        private void checkFirstInstance()
        {
            if(comboBox1.SelectedIndex!=-1&&comboBox2.SelectedIndex!=-1&&comboBox3.SelectedIndex!=-1)
            {

                comboBox4.Items.Clear();
                comboBox4.Text = "";

                if (regList.Where(x => x.idUnidadeCurricular == unitList[comboBox3.SelectedIndex].id && x.numeroAluno == studentList[comboBox2.SelectedIndex].numero && x.idAnoLetivo == yearList[comboBox1.SelectedIndex].id).FirstOrDefault() == null)
                {
                    comboBox4.Items.Add(epochList[1].descricao);
                }
                else
                {
                    if (selectedIndex != -1)
                    {
                        comboBox4.Items.Add(epochList.Where(x => x.id == regList[selectedIndex].idEpocaAvaliacao).FirstOrDefault().descricao);
                    }
                    else
                    {             
                         if (regList.Where(x => x.idUnidadeCurricular == unitList[comboBox3.SelectedIndex].id && x.numeroAluno == studentList[comboBox2.SelectedIndex].numero && x.idAnoLetivo == yearList[comboBox1.SelectedIndex].id && x.idEpocaAvaliacao == epochList[3].id && x.idEstadoEpoca == db.EstadoEpocas.ToList()[1].id).FirstOrDefault() != null)
                            comboBox4.Items.Add(epochList[0].descricao);
                         else if (regList.Where(x => x.idUnidadeCurricular == unitList[comboBox3.SelectedIndex].id && x.numeroAluno == studentList[comboBox2.SelectedIndex].numero && x.idAnoLetivo == yearList[comboBox1.SelectedIndex].id && x.idEpocaAvaliacao == epochList[2].id && x.idEstadoEpoca == db.EstadoEpocas.ToList()[1].id).FirstOrDefault() != null)
                            comboBox4.Items.Add(epochList[3].descricao);
                    }
                }
                if (comboBox4.Items.Count == 0)
                    comboBox4.SelectedIndex = -1;
                else
                    comboBox4.SelectedIndex = 0;
            }

        }

        //Deletes the selected registration in the edit form

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                db.Inscricaos.Remove(db.Inscricaos.ToList()[selectedIndex]);
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
                if (comboBox1.SelectedIndex == -1 || comboBox2.SelectedIndex == -1 || comboBox3.SelectedIndex == -1 || comboBox4.SelectedIndex == -1 || comboBox5.SelectedIndex == -1)
                {
                    MessageBox.Show("Please enter all of the details in the form!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                    //Insert a new registration row into the database

                    if (selectedIndex == -1)
                {
                    Inscricao inscricao = new Inscricao();
                    inscricao.numeroAluno = db.Alunoes.ToList().Where(x => x.nomeProprio + " " + x.apelido == comboBox2.Text.ToString()).First().numero;
                    inscricao.idUnidadeCurricular = db.UnidadeCurriculars.ToList().Where(x => x.nome == comboBox3.Text.ToString()).First().id;
                    inscricao.idAnoLetivo = db.AnoLetivoes.ToList().Where(x => x.anoInicial.ToString() + " to " + x.anoFinal.ToString() == comboBox1.Text.ToString()).First().id;
                    inscricao.idEpocaAvaliacao = db.EpocaAvaliacaos.ToList().Where(x => x.descricao == comboBox4.Text.ToString()).First().id;
                    inscricao.idEstadoEpoca = db.EstadoEpocas.ToList().Where(x => x.descricao == comboBox5.Text.ToString()).First().id;
                    inscricao.presenca = comboBox6.Text;
                    inscricao.nota = String.IsNullOrWhiteSpace(textBox3.Text) ? null : short.Parse(textBox3.Text);

                    if ((inscricao.nota != null && inscricao.nota >= 10) && (inscricao.presenca != "P" || inscricao.idEstadoEpoca != estadosList[2].id))
                    {
                        MessageBox.Show("A registration with the note greater than or equal to ten must have a presence value of 'P' and a State of Approved!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (regList.Where(x => x.idUnidadeCurricular == unitList[comboBox3.SelectedIndex].id && x.numeroAluno == studentList[comboBox2.SelectedIndex].numero && x.idAnoLetivo == yearList[comboBox1.SelectedIndex].id && x.idEpocaAvaliacao == epochList[3].id && estadosList[1].id == x.idEstadoEpoca).FirstOrDefault() != null && comboBox5.Text != estadosList[2].id.ToString())
                    {
                        if (inscricao.idEpocaAvaliacao == db.EpocaAvaliacaos.ToList()[0].id)
                        {
                            Inscricao tempinscricao1 = new Inscricao();
                            tempinscricao1.numeroAluno = db.Alunoes.ToList().Where(x => x.nomeProprio + " " + x.apelido == comboBox2.Text.ToString()).First().numero;
                            tempinscricao1.idUnidadeCurricular = db.UnidadeCurriculars.ToList().Where(x => x.nome == comboBox3.Text.ToString()).First().id;
                            tempinscricao1.idAnoLetivo = db.AnoLetivoes.ToList().Where(x => x.anoInicial.ToString() + " to " + x.anoFinal.ToString() == comboBox1.Text.ToString()).First().id;
                            tempinscricao1.idEpocaAvaliacao = db.EpocaAvaliacaos.ToList()[0].id;
                            tempinscricao1.idEstadoEpoca = db.EstadoEpocas.ToList()[0].id;
                            tempinscricao1.presenca = "";
                            tempinscricao1.nota = null;

                            db.Inscricaos.Add(tempinscricao1);
                        }
                    }

                    else if (regList.Where(x => x.idUnidadeCurricular == unitList[comboBox3.SelectedIndex].id && x.numeroAluno == studentList[comboBox2.SelectedIndex].numero && x.idAnoLetivo == yearList[comboBox1.SelectedIndex].id && x.idEpocaAvaliacao == epochList[2].id && x.idEstadoEpoca == db.EstadoEpocas.ToList()[1].id).FirstOrDefault() != null && comboBox5.Text != estadosList[2].id.ToString())
                    {
                        if (inscricao.idEpocaAvaliacao == db.EpocaAvaliacaos.ToList()[3].id)
                        {
                            Inscricao tempinscricao = new Inscricao();
                            tempinscricao.numeroAluno = db.Alunoes.ToList().Where(x => x.nomeProprio + " " + x.apelido == comboBox2.Text.ToString()).First().numero;
                            tempinscricao.idUnidadeCurricular = db.UnidadeCurriculars.ToList().Where(x => x.nome == comboBox3.Text.ToString()).First().id;
                            tempinscricao.idAnoLetivo = db.AnoLetivoes.ToList().Where(x => x.anoInicial.ToString() + " to " + x.anoFinal.ToString() == comboBox1.Text.ToString()).First().id;
                            tempinscricao.idEpocaAvaliacao = db.EpocaAvaliacaos.ToList()[3].id;
                            tempinscricao.idEstadoEpoca = db.EstadoEpocas.ToList()[0].id;
                            tempinscricao.presenca = "";
                            tempinscricao.nota = null;

                            db.Inscricaos.Add(tempinscricao);
                        }
                    }
                    else if (comboBox4.Text == epochList[1].descricao)
                    {
                        if (!String.IsNullOrWhiteSpace(textBox3.Text) && comboBox5.Text != estadosList[2].id.ToString())
                        {
                            if ((Int32.Parse(textBox3.Text) < 10 && comboBox6.Text == "P" && comboBox5.Text == db.EstadoEpocas.ToList()[0].descricao) || (comboBox6.Text == "F" && comboBox5.Text == db.EstadoEpocas.ToList()[0].descricao))
                            {
                                Inscricao inscricao1 = new Inscricao();
                                inscricao1.numeroAluno = db.Alunoes.ToList().Where(x => x.nomeProprio + " " + x.apelido == comboBox2.Text.ToString()).First().numero;
                                inscricao1.idUnidadeCurricular = db.UnidadeCurriculars.ToList().Where(x => x.nome == comboBox3.Text.ToString()).First().id;
                                inscricao1.idAnoLetivo = db.AnoLetivoes.ToList().Where(x => x.anoInicial.ToString() + " to " + x.anoFinal.ToString() == comboBox1.Text.ToString()).First().id;
                                inscricao1.idEpocaAvaliacao = db.EpocaAvaliacaos.ToList()[2].id;
                                inscricao1.idEstadoEpoca = db.EstadoEpocas.ToList()[0].id;
                                inscricao1.presenca = "";
                                inscricao1.nota = null;

                                db.Inscricaos.Add(inscricao1);
                            }
                        }
                        db.Inscricaos.Add(inscricao);
                    }
                    else if (comboBox5.Text != estadosList[2].id.ToString())
                    {
                        MessageBox.Show("A previous entry with the same student, subject, and academic year has already been approved!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else
                    {
                        MessageBox.Show("Please complete the necessary requirements to insert a registration with this evaluation period!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    //Updates the details of the selected registration saved in the database.

                    db.Inscricaos.ToList()[selectedIndex].numeroAluno = db.Alunoes.ToList().Where(x => x.nomeProprio + " " + x.apelido == comboBox2.Text.ToString()).First().numero;
                    db.Inscricaos.ToList()[selectedIndex].idUnidadeCurricular = db.UnidadeCurriculars.ToList().Where(x => x.nome == comboBox3.Text.ToString()).First().id;
                    db.Inscricaos.ToList()[selectedIndex].idAnoLetivo = db.AnoLetivoes.ToList().Where(x => x.anoInicial.ToString() + " to " + x.anoFinal.ToString() == comboBox1.Text.ToString()).First().id;
                    db.Inscricaos.ToList()[selectedIndex].idEpocaAvaliacao = db.EpocaAvaliacaos.ToList().Where(x => x.descricao == comboBox4.Text.ToString()).First().id;
                    db.Inscricaos.ToList()[selectedIndex].idEstadoEpoca = db.EstadoEpocas.ToList().Where(x => x.descricao == comboBox5.Text.ToString()).First().id;
                    db.Inscricaos.ToList()[selectedIndex].presenca = comboBox6.Text;
                    db.Inscricaos.ToList()[selectedIndex].nota = String.IsNullOrWhiteSpace(textBox3.Text) ? null : short.Parse(textBox3.Text);

                    db.Inscricaos.Update(db.Inscricaos.ToList()[selectedIndex]);

                    if (!String.IsNullOrWhiteSpace(textBox3.Text) && comboBox5.Text != estadosList[2].id.ToString())
                    {
                        if ((Int32.Parse(textBox3.Text) < 10 && comboBox6.Text == "P" && comboBox5.Text == db.EstadoEpocas.ToList()[0].descricao) || (comboBox6.Text == "F" && comboBox5.Text == db.EstadoEpocas.ToList()[0].descricao))
                        {
                            Inscricao inscricao = new Inscricao();
                            inscricao.numeroAluno = db.Alunoes.ToList().Where(x => x.nomeProprio + " " + x.apelido == comboBox2.Text.ToString()).First().numero;
                            inscricao.idUnidadeCurricular = db.UnidadeCurriculars.ToList().Where(x => x.nome == comboBox3.Text.ToString()).First().id;
                            inscricao.idAnoLetivo = db.AnoLetivoes.ToList().Where(x => x.anoInicial.ToString() + " to " + x.anoFinal.ToString() == comboBox1.Text.ToString()).First().id;
                            inscricao.idEpocaAvaliacao = db.EpocaAvaliacaos.ToList()[2].id;
                            inscricao.idEstadoEpoca = db.EstadoEpocas.ToList()[0].id;
                            inscricao.presenca = "";
                            inscricao.nota = null;

                            db.Inscricaos.Add(inscricao);
                        }
                    }

                }


                db.SaveChanges();
                this.Close();

            }
            catch
            {
                 MessageBox.Show("Sorry, an error occured!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            checkFirstInstance();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            checkFirstInstance();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            checkFirstInstance();
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
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
    }
}
