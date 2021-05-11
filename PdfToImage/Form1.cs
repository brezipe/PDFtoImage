using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PdfToImage
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            InitializeComponent();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {

                InitialDirectory = @"C:\",
                Title = "Vyber PDF",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "pdf",
                Filter = "PDF soubory (*.pdf)|*.pdf",
                FilterIndex = 2,
                RestoreDirectory = true,
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            // nastavim filter s popisem
            saveFileDialog1.Filter = "jpg obrázek (*.jpg)|*.jpg";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
             
                try
                {
                    using (var document = PdfiumViewer.PdfDocument.Load(textBox1.Text))
                    {
                        // document.Render(page number,dpiX,dpiY,forPrinting)
                        // pro strankovani pouzit forku z document.PageCount
                        // (stejne tak size neni doreseno)
                        var image = document.Render(0, 300, 300, true);
                        ImageCodecInfo myImageCodecInfo;
                        System.Drawing.Imaging.Encoder myEncoder;
                        EncoderParameter myEncoderParameter;
                        EncoderParameters myEncoderParameters;
                        myEncoderParameters = new EncoderParameters(1);
                        // nastavim kodek na jpg
                        myImageCodecInfo = GetEncoderInfo("image/jpeg");
                        myEncoder = System.Drawing.Imaging.Encoder.Quality;
                        // nastavim kvalitu na 75% (muze byt i 25, 50 - zbytek viz dokumentace)
                        myEncoderParameter = new EncoderParameter(myEncoder, 75L);
                        myEncoderParameters.Param[0] = myEncoderParameter;
                        image.Save(saveFileDialog1.FileName, myImageCodecInfo, myEncoderParameters);

                    }
                }
                catch (Exception ex)
                {
                    //zobrazim chybovou hlasku, pokud nastane problem:
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    // pokud success:
                    MessageBox.Show("Hotovo");
                }
            }
        }

        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }
    }
}
