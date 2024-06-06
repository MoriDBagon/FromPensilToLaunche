using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FromPensilToLaunche
{
    public partial class Form2 : Form
    {
        private string imageUrl;

        public Form2()
        {
            InitializeComponent();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private async void button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(imageUrl))
                {
                    string url = "http://127.0.0.1:5000/generate_image";
                    string prompt = textBox2.Text;
                    try
                    {
                        Image img = await ImageFetcher2.GetOutputImageAsync(url,prompt,imageUrl);
                        pictureBox2.Image = img;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Something went wrong: " + ex.Message, "Error", MessageBoxButtons.OK);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Multiselect = false;
                    openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png, *.gif, *.bmp)|*.jpg;*.jpeg;*.png;*.gif;*.bmp";

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        imageUrl = openFileDialog.FileName;
                        pictureBox1.Image = Image.FromFile(imageUrl);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (pictureBox2 != null)
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
    }
    public class ImageFetcher2
    {
        public static async Task<Image> GetOutputImageAsync(string url, string prompt, string imageUrl)
        {
            using (HttpClient client = new HttpClient())
            {
                var requestData = new
                {
                    imageUrl = imageUrl,
                    prompt = prompt

                };
                var json = JsonConvert.SerializeObject(requestData);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(url, content);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var jsonResponse = JObject.Parse(responseContent);

                string base64Image = jsonResponse["image"].ToString();
                byte[] imageBytes = Convert.FromBase64String(base64Image);

                using (MemoryStream memoryStream = new MemoryStream(imageBytes))
                {
                    return Image.FromStream(memoryStream);
                }
            }
        }
    }
}