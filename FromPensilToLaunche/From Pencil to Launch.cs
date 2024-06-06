using System;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Windows.Forms;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace FromPensilToLaunche
{
    public partial class Form1 : Form
    {
        private static HttpClient client = new HttpClient();

        String strCn = "Data Source=(localdb)\\MSSQLLocalDB;integrated security=True;initial catalog=ImageDatabase";
        public Form1()
        {
            InitializeComponent();
            button5.Click += new EventHandler(FetchButton_Click);
        }

        private async void FetchButton_Click(object sender, EventArgs e)
        {
            string url = "http://127.0.0.1:5000/generate_image";
            string prompt = textBox2.Text;
            try
            {
                Image img = await ImageFetcher.GetImageAsync(url, prompt);
                pictureBox1.Image = img;
            }
            catch (Exception ex) 
            {
                MessageBox.Show("Something went wrong!" + ex.Message, "ERROR", MessageBoxButtons.OK);
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.pictureBox1.Dock = DockStyle.Fill;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (pictureBox1 != null)
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "Jpeg Image|*.jpg|Png Image|*.png|BMP Image|*.bmp";
                    saveFileDialog.Title = "Save an Image file";
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string fileName = saveFileDialog.FileName;
                        Bitmap bitmap = new Bitmap(pictureBox1.Image);
                        bitmap.Save(fileName);
                        bitmap.Dispose();

                        MessageBox.Show("Image saved successfully!", "Success", MessageBoxButtons.OK);

                    }
                }
                else
                {
                    MessageBox.Show("No Image to save!", "Error", MessageBoxButtons.OK);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving image: " + ex.Message, "Error", MessageBoxButtons.OK);
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (pictureBox1 != null)
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "Jpeg Image|*.jpg|Png Image|*.png|BMP Image|*.bmp";
                    saveFileDialog.Title = "Save an Image file";
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string fileName = saveFileDialog.FileName;
                        Bitmap bitmap = new Bitmap(pictureBox2.Image);
                        bitmap.Save(fileName);
                        bitmap.Dispose();

                        MessageBox.Show("Image saved successfully!", "Success", MessageBoxButtons.OK);

                    }
                }
                else
                {
                    MessageBox.Show("No Image to save!", "Error", MessageBoxButtons.OK);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving image: " + ex.Message, "Error", MessageBoxButtons.OK);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (pictureBox1 != null)
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "Jpeg Image|*.jpg|Png Image|*.png|BMP Image|*.bmp";
                    saveFileDialog.Title = "Save an Image file";
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string fileName = saveFileDialog.FileName;
                        Bitmap bitmap = new Bitmap(pictureBox3.Image);
                        bitmap.Save(fileName);
                        bitmap.Dispose();

                        MessageBox.Show("Image saved successfully!", "Success", MessageBoxButtons.OK);

                    }
                }
                else
                {
                    MessageBox.Show("No Image to save!", "Error", MessageBoxButtons.OK);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving image: " + ex.Message, "Error", MessageBoxButtons.OK);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (pictureBox1 != null)
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "Jpeg Image|*.jpg|Png Image|*.png|BMP Image|*.bmp";
                    saveFileDialog.Title = "Save an Image file";
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string fileName = saveFileDialog.FileName;
                        Bitmap bitmap = new Bitmap(pictureBox4.Image);
                        bitmap.Save(fileName);
                        bitmap.Dispose();

                        MessageBox.Show("Image saved successfully!", "Success", MessageBoxButtons.OK);

                    }
                }
                else
                {
                    MessageBox.Show("No Image to save!", "Error", MessageBoxButtons.OK);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving image: " + ex.Message, "Error", MessageBoxButtons.OK);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(strCn))
                {
                    cn.Open();

                    //Retrieve BLOB from database into DataSet.
                    SqlCommand cmd = new SqlCommand("SELECT TOP 4 ImageData FROM (SELECT TOP 4 * FROM Images ORDER BY Id DESC) AS LastFour ORDER BY Id ASC", cn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    int pictureBoxIndex = 1;

                    while (reader.Read())
                    {
                        byte[] imageData = (byte[])reader["ImageData"];

                        if (imageData != null && imageData.Length > 0)
                        {
                            using (MemoryStream ms = new MemoryStream(imageData))
                            {
                                PictureBox pictureBox = (PictureBox)this.Controls.Find("pictureBox" + pictureBoxIndex, true).FirstOrDefault();
                                if (pictureBox != null)
                                {
                                    pictureBox.Image = Image.FromStream(ms);
                                }
                                pictureBoxIndex++;
                            }
                        }
                    }
                    for (int i = pictureBoxIndex; i <= 4; i++)
                    {
                        PictureBox pictureBox = (PictureBox)(this.Controls.Find("pictureBox" + i, true).FirstOrDefault());
                        if (pictureBox != null)
                        {
                            pictureBox.Image = null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            Form2 frm2 = new Form2();
            frm2.Show();
        }
    }
    public class ImageFetcher
    {
        public static async Task<Image> GetImageAsync(string url, string prompt)
        {
            using (HttpClient client = new HttpClient())
            {
                var data = new { prompt = prompt };

                var json = JsonConvert.SerializeObject(data);

                var content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");

                var response = await client.PostAsync(url, content);
                response.EnsureSuccessStatusCode();

                var responseString = await response.Content.ReadAsStringAsync();

                JObject jsonResponse = JObject.Parse(responseString);

                string bases64Image = jsonResponse["image"].ToString();

                byte[] imageBytes = Convert.FromBase64String(bases64Image);

                using (MemoryStream memoryStream = new MemoryStream(imageBytes))
                {
                    return Image.FromStream(memoryStream);
                }
            }
        }
    }
}
