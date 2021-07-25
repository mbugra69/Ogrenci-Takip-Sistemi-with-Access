using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
namespace Ogrenci_Takip_Projesi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        OleDbConnection con;
        OleDbCommand cmd;
        string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Buğra\source\repos\Ogrenci-Takip-Projesi\Ogrenci-Takip-Projesi\ogrenciler.accdb;";
        int ogrenciId = 0;


        private void Form1_Load(object sender, EventArgs e)
        {
            ogrenciLoad();
        }
        void ogrenciLoad()
        {

            try
            {
                con = new OleDbConnection(connectionString);
                cmd = new OleDbCommand("select * from ogrenci", con);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dg.DataSource = dt;
                dg.Columns["id"].Visible = false;

            }
            catch(OleDbException ex) 
            {
                throw ex;
            }
        }
        void ekle()
        {
            try
            {
                con = new OleDbConnection(connectionString);
                cmd = new OleDbCommand("insert into ogrenci(adi,soyadi,cinsiyeti,sinifi,numarasi) values (@adi,@soyadi,@cinsiyeti,@sinifi,@numarasi)", con);
                con.Open();
                cmd.Parameters.AddWithValue("@adi", txtAd.Text);
                cmd.Parameters.AddWithValue("@soyadi", txtSoyad.Text);

                string cinsiyet = "";
                if(radioBtnErkek.Checked)
                {
                    cinsiyet = "Erkek";
                } else if(radioBtnKiz.Checked)
                {
                    cinsiyet = "Kız";
                }
                cmd.Parameters.AddWithValue("@cinsiyeti", cinsiyet );
                cmd.Parameters.AddWithValue("@sinifi", cBSinif.Text);
                cmd.Parameters.AddWithValue("@numarasi", txtNumara.Text);
                cmd.ExecuteNonQuery();
            }
            catch(OleDbException ex)
            {

                throw ex;
            }
            finally
            {
                con.Close();
            }
            ogrenciLoad();

        }

        void guncelle()
        {
            try
            {
                con = new OleDbConnection(connectionString);
                cmd = new OleDbCommand("update ogrenci set adi=@adi,soyadi=@soyadi,cinsiyeti=@cinsiyeti,sinifi=@sinifi,numarasi=@numarasi where id=@id", con);
                con.Open();
                cmd.Parameters.AddWithValue("@adi", txtAd.Text);
                cmd.Parameters.AddWithValue("@soyadi", txtSoyad.Text);

                string cinsiyet = "";
                if (radioBtnErkek.Checked)
                {
                    cinsiyet = "Erkek";
                }
                else if (radioBtnKiz.Checked)
                {
                    cinsiyet = "Kız";
                }
                cmd.Parameters.AddWithValue("@cinsiyeti", cinsiyet);
                cmd.Parameters.AddWithValue("@sinifi", cBSinif.Text);
                cmd.Parameters.AddWithValue("@numarasi", txtNumara.Text);
                cmd.Parameters.Add("@id", OleDbType.Integer).Value = ogrenciId;
                cmd.ExecuteNonQuery();
            }
            catch (OleDbException ex)
            {

                throw ex;
            }
            finally
            {
                con.Close();
            }
            ogrenciLoad();

        }



        void sil()
        {
            try
            {
                con = new OleDbConnection(connectionString);
                cmd = new OleDbCommand("delete from ogrenci  where id=@id", con);
                con.Open();
      
                cmd.Parameters.Add("@id", OleDbType.Integer).Value = ogrenciId;
                cmd.ExecuteNonQuery();
            }
            catch (OleDbException ex)
            {

                throw ex;
            }
            finally
            {
                con.Close();
            }
            ogrenciLoad();

        }


        void temizle()
        {
            foreach (var item in tableLayoutPanel1.Controls)
            {
                if(item is TextBox)
                {
                    ((TextBox)item).Text = "";
                }
                if (item is ComboBox)
                {
                    ((ComboBox)item).Text = "";
                }

            }

            radioBtnErkek.Checked = false;
            radioBtnKiz.Checked = false;
         
        }
        private void btnEkle_Click(object sender, EventArgs e)
        {
            ekle();
        }

        private void dg_CellClick(object sender, DataGridViewCellEventArgs e)
        {
          if(e.RowIndex > -1) // eğer verilerden başka yere tıklarsak sorun çıkmasın diye. veriler 0' dan başlıyor.
            {
                ogrenciId = Convert.ToInt32(dg.Rows[e.RowIndex].Cells["id"].Value);
                try
                {
                    con = new OleDbConnection(connectionString);
                    cmd = new OleDbCommand("select * from ogrenci where id = @id", con);
                    cmd.Parameters.Add("@id", OleDbType.Integer).Value = ogrenciId;

                    OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    radioBtnErkek.Checked = false;
                    radioBtnKiz.Checked = false;

                    foreach (DataRow row in dt.Rows)
                    {
                        txtAd.Text = row["adi"].ToString();
                        txtSoyad.Text = row["soyadi"].ToString();
                        string cinsiyet = row["cinsiyeti"].ToString();
                        if(cinsiyet == "Erkek")
                        {
                            radioBtnErkek.Checked = true;
                        } 
                        else if(cinsiyet == "Kız")
                        {
                            radioBtnKiz.Checked = true;
                        }
                        cBSinif.Text = row["sinifi"].ToString();
                        txtNumara.Text = row["numarasi"].ToString();


                    }
                }
                catch(OleDbException ex)
                {
                    throw ex;
                }



            }
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {

            if (ogrenciId > 0)
            {

           
            guncelle();
            }
            else
            {
                MessageBox.Show("Öğrenci Seçiniz.", "Uyarı!");
            }
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            if (ogrenciId > 0)
            {
                DialogResult dialogResult = MessageBox.Show("Gerçekten bu kaydı silmek istediğinize emin misiniz?", "Uyarı!", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {

                    sil();
                }
                else
                {
                    MessageBox.Show("İşlem iptal edildi.", "Uyarı!");
                }
            }
            else
            {
                MessageBox.Show("Öğrenci Seçiniz.", "Uyarı!");
            }
        }

        private void btnYeni_Click(object sender, EventArgs e)
        {
            temizle();
        }
    }
}
