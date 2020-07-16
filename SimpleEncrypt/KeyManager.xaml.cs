using System;
using System.Windows.Controls;
using System.Windows.Shapes;
using Key = Gabriel.Cat.S.Seguretat.Key;
using System.IO;
using Path = System.IO.Path;
using Gabriel.Cat.S.Binaris;
using System.Windows;
using System.Collections.Generic;
using Gabriel.Cat.S.Extension;

namespace SimpleEncrypt
{
    /// <summary>
    /// Lógica de interacción para KeyManager.xaml
    /// </summary>
    public partial class KeyManager : UserControl
    {
        public static readonly string KeysFolder = Path.Combine(Environment.CurrentDirectory, "Keys");
        SortedList<string, KeyName> KeysDic;
        public event EventHandler KeySelected;
        public KeyManager()
        {
            KeysDic = new SortedList<string, KeyName>();

            InitializeComponent();
            LoadKeys();
        }



        public KeyName Selected { get; set; }
        private void btnAddKey_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            KeyName keyToAdd;
            if (!string.IsNullOrEmpty(txtNameKey.Text) && !string.IsNullOrWhiteSpace(txtNameKey.Text))
            {
                if (KeysDic.ContainsKey(txtNameKey.Text))
                {
                    MessageBox.Show("El nombre ya existe!");
                }
                else
                {
                    keyToAdd =new KeyName(txtNameKey.Text, Key.GetKey(100));
                    keyToAdd.Save(KeysFolder);
                    KeysDic.Add(keyToAdd.Name, keyToAdd);
                    lstKeys.Items.Add(keyToAdd);                  
                    txtNameKey.Text = "";
                    
                }
            }
            else
            {
                MessageBox.Show("Se necesita un nombre!");
            }
        }

        private void lstKeys_Selected(object sender, RoutedEventArgs e)
        {
            Selected = lstKeys.SelectedItem as KeyName;
            if (!Equals(Selected, default))
            {
                lblSelectedKey.Content = Selected.Name;
                if (KeySelected != null)
                    KeySelected(this, new EventArgs());
            }
        }
        public void LoadKeys()
        {
            KeyName aux;
            if (Directory.Exists(KeysFolder))
            {
                KeysDic.Clear();
                lstKeys.Items.Clear();

                foreach (string path in Directory.GetFiles(KeysFolder))
                {
                    aux = new KeyName(path);
                    lstKeys.Items.Add(aux);
                    KeysDic.Add(aux.Name, aux);
                }
                if (lstKeys.Items.Count > 0)
                    lstKeys.SelectedIndex = 0;
                else lblSelectedKey.Content = "";
            }

        }

        private void UserControl_PreviewDragOver(object sender, DragEventArgs e)
        {
            e.Handled = true;
        }

        private void UserControl_Drop(object sender, DragEventArgs e)
        {
            string pathDestino;
            string path = e.Data.GetFilePath();

            if(path!=default)
            {
                pathDestino =System.IO.Path.Combine(KeysFolder, path.Substring(path.LastIndexOf('\\') + 1));
                System.IO.File.Copy(path, pathDestino);
                LoadKeys();
            }
        }
    }
    public class KeyName
    {
        public static KeyBinario KeyBin = new KeyBinario();
        public KeyName(string path)
        {
            Name = Path.GetFileNameWithoutExtension(path);
            Key = (Key)KeyBin.GetObject(File.ReadAllBytes(path));
        }
        public KeyName(string name,Key key)
        {
            Name = name;
            Key = key;
        }
        public string Name { get; set; }
        public Key Key { get; set; }
        public void Save(string folder)
        {
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            File.WriteAllBytes(Path.Combine(folder, Name), KeyBin.GetBytes(Key));
        }
        public override string ToString() => Name;
    }
}
