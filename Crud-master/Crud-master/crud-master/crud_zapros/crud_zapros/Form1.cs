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

namespace crud_zapros
{
    public partial class Form1 : Form
    {

        SqlConnection con = new SqlConnection("Data Source=DESKTOP-4CN6F62\\SQLEXPRESS;Initial Catalog=CRUD;Integrated Security=True");

        SqlCommand cmd;
        SqlDataAdapter da;
        DataTable dt;
        int id=-1;

        

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cmd = new SqlCommand("select count (*) from USERS where LOGIN=@Login", con);
            cmd.Parameters.AddWithValue("@login", textBox1.Text);
            con.Open();
            if ((int)cmd.ExecuteScalar() == 0)

            {
                SqlCommand add = new SqlCommand("insert into USERS (LOGIN, password, administrator) values (@login,@password, @administrator)", con);
                add.Parameters.AddWithValue("login", textBox1.Text);
                add.Parameters.AddWithValue("password", textBox2.Text);
                add.Parameters.AddWithValue("administrator", checkBox1.Checked ? 1 : 0);
                add.ExecuteNonQuery();
                DisplayData();

                MessageBox.Show("Пользователь добавлен");
            }
            else
            MessageBox.Show("Пользователь уже существует");
            con.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "cRUDDataSet.USERS". При необходимости она может быть перемещена или удалена.
            this.uSERSTableAdapter.Fill(this.cRUDDataSet.USERS); 

        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlCommand delete = new SqlCommand("delete from USERS where ID=@id", con);
            delete.Parameters.AddWithValue("id", id);
            con.Open();
            delete.ExecuteNonQuery();
            DisplayData();
            con.Close();
            

            MessageBox.Show("Пользователь удален");
        }

        public void DisplayData()
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM USERS", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            cmd = new SqlCommand("select count (*) from USERS where LOGIN=@Login and ID not like @ID", con);
            cmd.Parameters.AddWithValue("@ID",id);
            cmd.Parameters.AddWithValue("@login", textBox1.Text);
            con.Open();
            if ((int)cmd.ExecuteScalar() == 0)
            {
                cmd = new SqlCommand("update USERS set LOGIN=@Login, password=@password, administrator=@administrator where ID=@ID", con);
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.Parameters.AddWithValue("@login", textBox1.Text);
                cmd.Parameters.AddWithValue("@password", textBox2.Text);
                cmd.Parameters.AddWithValue("@administrator", checkBox1.Checked ? 1 : 0);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Информация исправлена");
                DisplayData();
            }
            
            con.Close();
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                id = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);
                textBox1.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                textBox2.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            }

            catch { }
        }
    }
}
