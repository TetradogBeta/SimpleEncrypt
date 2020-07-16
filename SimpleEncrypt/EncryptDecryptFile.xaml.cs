using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
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
    /// Lógica de interacción para EncryptDecryptFile.xaml
    /// </summary>
    public partial class EncryptDecryptFile : UserControl
    {
        public EncryptDecryptFile()
        {
            InitializeComponent();
        }
        public Gabriel.Cat.S.Seguretat.Key KeyFile { get; set; }
        public bool EncryptOrDecrypt { get; set; }
        public void Refresh()
        {
            if (EncryptOrDecrypt)
                btnEncryptOrDecrypt.Content = "Encrypt";
            else btnEncryptOrDecrypt.Content = "Decrypt";
        }
        private void btnEncontrarDestino_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog opnFile = new SaveFileDialog();
            if (opnFile.ShowDialog().GetValueOrDefault())
                txtUrlDestino.Text = opnFile.FileName;
        }

        private void btnEncontrarOrigen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog opnFile = new OpenFileDialog();
            if (opnFile.ShowDialog().GetValueOrDefault())
                txtUrlOrigen.Text = opnFile.FileName;
        }


        private void btnEncryptOrDecrypt_Click(object sender, RoutedEventArgs e)
        {
            if (EncryptOrDecrypt)
               KeyFile.Encrypt(new FileInfo(txtUrlOrigen.Text), txtUrlDestino.Text);
            else
                KeyFile.Decrypt(new FileInfo(txtUrlOrigen.Text), txtUrlDestino.Text);

            MessageBox.Show("Hecho");

        }
    }
}
