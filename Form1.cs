using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace responsi2_rapp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private NpgsqlConnection conn;
        string connstring = "Host=localhost;port=5432;Username=postgres;Password=informatika;Database=karyawan";
        public DataTable dt;
        public NpgsqlCommand cmd;
        private string sql = null;
        private DataGridViewRow r;

        private void Form1_Load(object sender, EventArgs e)
        {
            conn = new NpgsqlConnection(connstring);   
        }
        private void btnLoadData_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                dgvData.DataSource = null;
                sql = @"select * from st_select()";
                cmd = new NpgsqlCommand(sql, conn);
                dt = new DataTable();
                NpgsqlDataReader rd = cmd.ExecuteReader();
                dt.Load(rd);
                dgvData.DataSource = dt;
                conn.Close();
            }
            catch (Exception ex) 
            {
                MessageBox.Show("Error:" +  ex.Message, "FAIL!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                sql = @"select * from st_insert(:_nama,:_nama_dep)";
                cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("_nama");
                cmd.Parameters.AddWithValue("_nama_dep");
                if ((int)cmd.ExecuteScalar() == 1)
                {
                    MessageBox.Show("Data Berhasil Diinput", "SUKSES", MessageBoxButtons.OK, MessageBoxIcon.Information );
                    conn.Close();
                    btnLoadData.PerformClick();
                    tbNamaKaryawan.Text = tbDepKaryawan.Text = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex.Message, "Insert Gagal", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                sql = @"select from * st_update(:_id_dep,:_nama,:_nama_dep)";
                cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("_id_dep", r.Cells["_id_dep"].Value.ToString());
                cmd.Parameters.AddWithValue("_nama", tbNamaKaryawan.Text);
                cmd.Parameters.AddWithValue("_nama_dep", tbDepKaryawan.Text);
                if((int)cmd.ExecuteScalar() == 1) 
                {
                    MessageBox.Show("Data Berhasil diupdate", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    conn.Close();
                    btnLoadData.PerformClick();
                    tbNamaKaryawan.Text = tbDepKaryawan.Text = null;
                    r = null;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex.Message, "Update Gagal", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                sql = @"select * from st_delete(_id_dep)";
                cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("_id_dep", r.Cells["_id_dep"].Value.ToString());
                if((int)cmd.ExecuteScalar() == 1)
                {
                    MessageBox.Show("Data berhasil dihapus", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    conn.Close();
                    btnLoadData.PerformClick();
                    tbNamaKaryawan.Text = tbDepKaryawan.Text = null;
                    r = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex.Message, "Delete Gagal", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
