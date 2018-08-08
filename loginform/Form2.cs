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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        DataBaseConnection objConnect;
        string conString;

        DataSet ds;
        DataRow dRow;

        int MaxRows;
        int inc = 0;

        private void btnQuit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'credentialsDataSet.tbl_credentials' table. You can move, or remove it, as needed.
            this.tbl_credentialsTableAdapter.Fill(this.credentialsDataSet.tbl_credentials);

            try
            {
                objConnect = new DataBaseConnection();
                conString = Properties.Settings.Default.CredentialsConnectionString;

                objConnect.connection_string = conString;
                objConnect.Sql = Properties.Settings.Default.SQL;

                ds = objConnect.GetConnection;
                MaxRows = ds.Tables[0].Rows.Count;

                NavigateRecords();

            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void NavigateRecords()
        {

            dRow = ds.Tables[0].Rows[inc];

            textBox1.Text = dRow.ItemArray.GetValue(1).ToString();
            textBox2.Text = dRow.ItemArray.GetValue(2).ToString();

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btn_Insert_Click(object sender, EventArgs e)
        {
            DataRow row = ds.Tables[0].NewRow();

            row[1] = textBox1.Text;
            row[2] = textBox2.Text;

            ds.Tables[0].Rows.Add(row);

            try
            {
                objConnect.UpdateDatabase(ds);
                MaxRows = MaxRows + 1;
                inc = MaxRows - 1;

                MessageBox.Show("Database updated");

               
            }
            catch (Exception err)
            {

                MessageBox.Show(err.Message);

            }

            this.tbl_credentialsTableAdapter.Fill(this.credentialsDataSet.tbl_credentials);
        }

        private void btn_Clear_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            SqlConnection sqlcon = new SqlConnection(conString);

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                DataGridViewRow dr = dataGridView1.Rows[i];
                if (dr.Selected == true)
                {
                    dataGridView1.Rows.RemoveAt(i);
                    try
                    {
                        string query = "Delete from tbl_credentials where ID=" + i + "";
                        SqlDataAdapter sda = new SqlDataAdapter(query, sqlcon);
                        DataTable dtbl = new DataTable();
                        sda.Fill(dtbl);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
            }
        }
    }
}
