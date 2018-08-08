using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace LoginForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        DataBaseConnection objConnect;
        string conString;

        DataSet ds;
        DataRow dRow;

        int MaxRows;
        int inc = 0;


        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                objConnect = new DataBaseConnection();
                conString = Properties.Settings.Default.CredentialsConnectionString;

                objConnect.connection_string = conString;
                objConnect.Sql = Properties.Settings.Default.SQL;

                ds = objConnect.GetConnection;
                MaxRows = ds.Tables[0].Rows.Count;

                


            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void NavigateRecords() {
            dRow = ds.Tables[0].Rows[inc];
        }



        private void btnCancel_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            this.Close();
            Application.Exit();
        }

        private void btnLogIn_Click(object sender, EventArgs e)
        {
            SqlConnection sqlcon = new SqlConnection(conString);
            //byte bytePW = textBox2.Text;
            //Encoding.UTF8.GetString(
            string passwordString = textBox2.Text;
            //byte[] passwordEncrypted = Encoding.ASCII.GetBytes(passwordString);


            string query = "Select * from tbl_credentials where user_name = '" + textBox1.Text.Trim() + "' and password = '" + passwordString + "'";
            SqlDataAdapter sda = new SqlDataAdapter(query, sqlcon);
            DataTable dtbl = new DataTable();
            sda.Fill(dtbl);
            if(dtbl.Rows.Count == 1)
            {
                Form2 homeForm = new Form2();
                this.Hide();
                homeForm.Show();
            }
            else
            {
                MessageBox.Show("Wrong credentials");
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
 
            try
            {
                DataRow row = ds.Tables[0].NewRow();
                byte[] encryptedPW = Encoding.ASCII.GetBytes(textBox4.Text);
                row[1] = textBox3.Text;
                row[2] = encryptedPW;
                //row[3] = textBox5.Text;
                //row[4] = textBox6.Text;
                //row[5] = textBox7.Text;
                //row[6] = textBox8.Text;

                ds.Tables[0].Rows.Add(row);

                try
                {
                    objConnect.UpdateDatabase(ds);

                    MessageBox.Show("Database updated");
                }
                catch (Exception err)
                {

                    MessageBox.Show(err.Message);

                }

                textBox3.Clear();
                textBox4.Clear();
                textBox5.Clear();
                textBox6.Clear();
                textBox7.Clear();
                textBox8.Clear();
            }

            catch
            {
                MessageBox.Show("Fill all boxes");
            }
           
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }
    }
}