using System;
using System.Data;
using System.Windows.Forms;
using Npgsql;

namespace projek_kyyme
{
    public partial class Form1 : Form
    {
        private string id = "";
        private int intRow = 0;

        public Form1()
        {
            InitializeComponent();
            resetMe();
        }
        private void resetMe()
        {
            this.id = string.Empty;

            namaDepanTextBox.Text = "";
            namaBelakangTextBox.Text = "";

            if (jenisKelaminComboBox.Items.Count > 0)
            {
                jenisKelaminComboBox.SelectedIndex = 0;
            }

            updateButton.Text = "Update ()";
            deleteButton.Text = "Delete ()";

            keywordTextBox.Clear();

            if (keywordTextBox.CanSelect)
            {
                keywordTextBox.Select();
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void keywordTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            loadData("");
        }

        private void loadData(string keyword)
        {
            CRUD.sql = "SELECT idplayer, namadepan, namabelakang, CONCAT(namadepan, ' ',namabelakang) AS namalengkap, jeniskelamin FROM dataplayer " +
                       "WHERE CONCAT(CAST(idplayer as varchar), ' ', namadepan, ' ', namabelakang) LIKE @keyword::varchar " +
                       "OR TRIM(jeniskelamin) LIKE @keyword::varchar ORDER BY idplayer ASC";

            string strKeyword = string.Format("%(0)%", keyword);

            CRUD.cmd = new NpgsqlCommand(CRUD.sql, CRUD.con);
            CRUD.cmd.Parameters.Clear();
            CRUD.cmd.Parameters.AddWithValue("keyword", strKeyword);

            DataTable dt = CRUD.PerformCRUD(CRUD.cmd);

            if (dt.Rows.Count > 0)
            {
                intRow = Convert.ToInt32(dt.Rows.Count.ToString());
            }
            else
            {
                intRow = 0;
            }

            toolStripStatusLabel1.Text = "Jumlah kolom(s): " + intRow.ToString();

            DataGridView dgv1 = dataGridView1;

            dgv1.MultiSelect = false;
            dgv1.AutoGenerateColumns = true;
            dgv1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            dgv1.DataSource = dt;

            dgv1.Columns[0].HeaderText = "ID";
            dgv1.Columns[1].HeaderText = "Nama Depan";
            dgv1.Columns[2].HeaderText = "Nama Belakang";
            dgv1.Columns[3].HeaderText = "Nama Lengkap";
            dgv1.Columns[4].HeaderText = "Jenis Kelamin";

            dgv1.Columns[0].Width = 85;
            dgv1.Columns[1].Width = 170;
            dgv1.Columns[2].Width = 170;
            dgv1.Columns[3].Width = 220;
            dgv1.Columns[4].Width = 100;
        }

        private void execute(string mySQL, string param)
        {
            CRUD.cmd = new NpgsqlCommand(mySQL, CRUD.con);
            addParameters(param);
            CRUD.PerformCRUD(CRUD.cmd);
        }

        private void addParameters(string str)
        {
            CRUD.cmd.Parameters.Clear();
            CRUD.cmd.Parameters.AddWithValue("@namaDepan", namaDepanTextBox.Text.Trim());
            CRUD.cmd.Parameters.AddWithValue("@namaBelakang", namaBelakangTextBox.Text.Trim());
            CRUD.cmd.Parameters.AddWithValue("@jenisKelamin", jenisKelaminComboBox.SelectedItem.ToString());

            if ((str == "Update" || str == "Delete") && !string.IsNullOrEmpty(this.id))
            {
                CRUD.cmd.Parameters.AddWithValue("@id", this.id);
            }
        }

        private void insertButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(namaDepanTextBox.Text.Trim()) || string.IsNullOrEmpty(namaBelakangTextBox.Text.Trim()))
            {
                MessageBox.Show("Tolong masukkan data nama depan dan nama belakang.", "Insert Data : Rizky Ramdhani",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            CRUD.sql = "INSERT INTO dataplayer(namadepan, namabelakang, jeniskelamin) VALUES (@namaDepan, @namaBelakang, @jenisKelamin)";

            execute(CRUD.sql, "Insert");

            MessageBox.Show("Jejak Anda telah disimpan.", "Insert Data : Rizky Ramdhani",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

            loadData("");

            resetMe();
        }



        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex != -1)
            {
                DataGridView dgv1 = dataGridView1;

                this.id = Convert.ToString(dgv1.CurrentRow.Cells[0].Value);
                updateButton.Text = "Update(" + this.id + ")";
                deleteButton.Text = "Delete(" + this.id + ")";

                namaDepanTextBox.Text = Convert.ToString(dgv1.CurrentRow.Cells[1].Value);
                namaBelakangTextBox.Text = Convert.ToString(dgv1.CurrentRow.Cells[2].Value);

                jenisKelaminComboBox.SelectedItem = Convert.ToString(dgv1.CurrentRow.Cells[4].Value);
            }
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                return;
            }

            if (string.IsNullOrEmpty(this.id))
            {
                MessageBox.Show("Tolong pilih item dari list.", "Update Data : Rizky Ramdhani",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (string.IsNullOrEmpty(namaDepanTextBox.Text.Trim()) || string.IsNullOrEmpty(namaBelakangTextBox.Text.Trim()))
            {
                MessageBox.Show("Tolong masukkan data nama depan dan nama belakang.", "Insert Data : Rizky Ramdhani",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            CRUD.sql = "UPDATE dataplayer SET namadepan = @namaDepan, namabelakang = @namaBelakang, jeniskelamin = @jenisKelamin WHERE idplayer = @id::integer";

            execute(CRUD.sql, "Update");

            MessageBox.Show("Jejak Anda telah di ubah.", "Update Data : Rizky Ramdhani",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

            loadData("");

            resetMe();
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                return;
            }

            if (string.IsNullOrEmpty(this.id))
            {
                MessageBox.Show("Tolong pilih item dari list.", "Delete Data : Rizky Ramdhani",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            
            if (MessageBox.Show("Apakah Anda yakin ingin menghapusnya?","Delete Data : Rizky Ramdhani",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
           
            CRUD.sql = "DELETE FROM dataplayer WHERE idplayer = @id::integer";

            execute(CRUD.sql, "Update");

            MessageBox.Show("Jejak Anda telah dihapus.", "Delete Data : Rizky Ramdhani",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

            loadData("");

            resetMe();
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(keywordTextBox.Text.Trim()))
            {
                loadData("");
            }
            else
            {
                loadData(keywordTextBox.Text.Trim());
            }

            resetMe();
        }
    }
}
