using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using PdfSharp.Pdf.Security;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace CodificaFileConPassword
{
    public partial class FileProtector : Form
    {
        public FileProtector()
        {
            InitializeComponent();
        }

        private void Sfoglia(object sender, EventArgs e)
        {
            ResetForm();
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Title = "SELEZIONA IL FILE PDF DA PROTEGGERE CON PASSWORD",
                    CheckFileExists = true,
                    CheckPathExists = true,
                    DefaultExt = "pdf",
                    Filter = "pdf files (*.pdf)|*.pdf",
                    FilterIndex = 2,
                    RestoreDirectory = true,
                    ReadOnlyChecked = true,
                    ShowReadOnly = true
                };

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    textBox1.Text = openFileDialog.FileName;
                    textBox2.Text = "-File importato correttamente" + Environment.NewLine;
                }
            }
            catch (Exception ex)
            {
                textBox2.Text = "Errore: " + ex.Message +Environment.NewLine;
            }
        }

        private void ResetForm()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
        }

        private void Codifica_Click(object sender, EventArgs e)
        {

            string filename = textBox1.Text;
            string userPassword = textBox3.Text;
            try
            {
                using (PdfDocument document = PdfReader.Open(filename))
                {
                    PdfSecuritySettings securitySettings = document.SecuritySettings;
                    securitySettings.UserPassword = userPassword;
                    securitySettings.OwnerPassword = userPassword;
                    securitySettings.PermitAccessibilityExtractContent = false;
                    securitySettings.PermitAnnotations = false;
                    securitySettings.PermitAssembleDocument = false;
                    securitySettings.PermitExtractContent = false;
                    securitySettings.PermitFormsFill = true;
                    securitySettings.PermitFullQualityPrint = false;
                    securitySettings.PermitModifyDocument = true;
                    securitySettings.PermitPrint = false;
                    string ecnFilename = "enc_" + Path.GetFileName(filename);
                    filename = filename.Replace(Path.GetFileName(filename), ecnFilename);
                    document.Save(filename);
                    textBox2.Text = textBox2.Text + "-File protetto correttamente: " + ecnFilename + Environment.NewLine;
                }
            }
            catch (Exception ex)
            {
                textBox2.Text = "Errore: " + ex.Message + Environment.NewLine;
            }


        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox3.Text = "";
            try
            {
                string password = generateInitialPassword();
                textBox3.Text = password;
                textBox2.Text = textBox2.Text +"-Password generate correttamente" + Environment.NewLine;
            }
            catch (Exception ex)
            {
                textBox2.Text = "Errore: " + ex.Message + Environment.NewLine;
            }

        }

        private string generateInitialPassword()
        {
            string text1 = "aAbBcCdDeEfFgGhHiIjJhHkKlLmMnNoOpPqQrRsStTuUvVwWxXyYzZ";
            string text2 = ",;:!*$@-_=,;:!*$@-_=";
            string text3 = "01234567890123456789";
            char[] chars = new char[15];
            Random random = new Random();
            chars[0] = GetRandomCharacter(text1, random);
            chars[1] = GetRandomCharacter(text1, random);
            chars[2] = GetRandomCharacter(text1, random);
            chars[3] = GetRandomCharacter(text2, random);
            chars[4] = GetRandomCharacter(text2, random);
            chars[5] = GetRandomCharacter(text2, random);
            chars[6] = GetRandomCharacter(text3, random);
            chars[7] = GetRandomCharacter(text3, random);
            chars[8] = GetRandomCharacter(text3, random);
            chars[9] = GetRandomCharacter(text2, random);
            chars[10] = GetRandomCharacter(text3, random);
            chars[11] = GetRandomCharacter(text1, random);
            chars[12] = GetRandomCharacter(text1, random);
            chars[13] = GetRandomCharacter(text1, random);
            chars[14] = GetRandomCharacter(text1, random);
            string startingPassword = new string(chars);

            return RandomizePassword(startingPassword, random);
        }

        public string RandomizePassword(string password, Random r)
        {
            return new string(password.ToCharArray().OrderBy(s => (r.Next(2) % 2) == 0).ToArray());
        }

        public static char GetRandomCharacter(string text, Random random)
        {
            int index = random.Next(text.Length);
            return text[index];
        }

    }
}
