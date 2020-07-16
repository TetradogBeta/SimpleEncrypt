using Gabriel.Cat.S.Extension;
using Gabriel.Cat.S.Utilitats;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SimpleEncrypt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       
        
        public MainWindow()
        {
            MenuItem item;

            InitializeComponent();
            encryptFile.Refresh();
            decryptFile.Refresh();
            keyManager_KeySelected();
            txtOrigen.ContextMenu.Items.Add(new Separator());
            item = new MenuItem();
            item.Header = "Importar";
            item.Click+= (s, e) => txtOrigen.TextWithFormat = GetContentSelectedFile();
            txtOrigen.ContextMenu.Items.Add(item);

            item = new MenuItem();
            item.Header = "Exportar";
            item.Click += (s, e) => SaveToFile(txtOrigen.TextWithFormat);
            txtOrigen.ContextMenu.Items.Add(item);

            txtDestino.ContextMenu = new ContextMenu();

            item = new MenuItem();
            item.Header = "Importar";
            item.Click += (s, e) => txtDestino.Text = GetContentSelectedFile();
            txtDestino.ContextMenu.Items.Add(item);

            item = new MenuItem();
            item.Header = "Exportar";
            item.Click += (s, e) => SaveToFile(txtDestino.Text);
            txtDestino.ContextMenu.Items.Add(item);

        }

        private void SaveToFile(string text)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            if (saveFile.ShowDialog().GetValueOrDefault())
                System.IO.File.WriteAllBytes(saveFile.FileName,Serializar.GetBytes( text));
        }

        private string GetContentSelectedFile()
        {
            string text;
            OpenFileDialog opnFile = new OpenFileDialog();
            if (opnFile.ShowDialog().GetValueOrDefault())
            {
                text =Serializar.ToString( System.IO.File.ReadAllBytes(opnFile.FileName));
            }
            else text = string.Empty;

            return text;
        }

        private void btnDecrypt_Click(object sender, RoutedEventArgs e)
        {
            if (keyManager.Selected != default)
            {
                txtOrigen.TextWithFormat = keyManager.Selected.Key.Decrypt(txtDestino.Text);
            }
            else MessageBox.Show("Se requiere una Key");
        }

        private void btnEncrypt_Click(object sender, RoutedEventArgs e)
        {
            if (keyManager.Selected != default)
            {
                txtDestino.Text = keyManager.Selected.Key.Encrypt(txtOrigen.TextWithFormat);
            }
            else MessageBox.Show("Se requiere una Key");
        }

        private void keyManager_KeySelected(object sender=null, EventArgs e=null)
        {
            if (keyManager.Selected != default)
            {
                encryptFile.KeyFile = keyManager.Selected.Key;
                decryptFile.KeyFile = keyManager.Selected.Key;
            }

        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                keyManager.LoadKeys();
            }
        }

        private void txtDestino_Drop(object sender, DragEventArgs e)
        { 
            byte[] data = e.Data.GetFileData();
            if (data != default)
            {
                txtDestino.Text = Serializar.ToString(data);
            }
        }

        private void TextPreviewDragOver(object sender, DragEventArgs e)
        {
            e.Handled = true;
        }

   
        private void txtOrigen_Drop(object sender, DragEventArgs e)
        {
            byte[] data = e.Data.GetFileData();
            if (data != default)
            {
                txtOrigen.TextWithFormat = Serializar.ToString(data);
            }

        }
    }
}
