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
using System.IO;

namespace IoBrowser_Shower
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            SetBrowserDir(System.Windows.Forms.Application.StartupPath);
        }
        public void SetBrowserDir(string dir)
        {
            BrowserIO.Items.Clear();
            TreeViewItem rootDir = new TreeViewItem();
            rootDir.Tag = dir;
            rootDir.Header = new DirectoryInfo(dir).Name;
            BrowserIO.Items.Add(rootDir);
            RecursiveAddingBrowser(rootDir);
        }

        private void Expander(object sender, RoutedEventArgs e)
        {
            TreeViewItem itDir = (sender as TreeViewItem);
            if (itDir.Items.Count <= 0)
            {
                MessageBox.Show("Ошибка доступа к каталогу!", "Отказано в доступе", MessageBoxButton.OK, MessageBoxImage.Error);
                itDir.Expanded -= Expander;
                return;
            }

            if (itDir.IsExpanded && (itDir.Items[0] as TreeViewItem).Tag.ToString() == "None")
            {
                itDir.Items.RemoveAt(0);
                RecursiveAddingBrowser(itDir);
            }
        }

        public void RecursiveAddingBrowser(TreeViewItem itemRoot)
        {

            string[] fls = Directory.GetFiles(itemRoot.Tag.ToString());
            foreach (var v in fls)
            {
                TreeViewItem it = new TreeViewItem();
                it.Header = new FileInfo(v).Name;
                it.Tag = v;
                itemRoot.Items.Add(it);
            }
            string[] dirs = Directory.GetDirectories(itemRoot.Tag.ToString());
            foreach (var v in dirs)
            {
                TreeViewItem itDir = new TreeViewItem();

                itDir.Header = new DirectoryInfo(v).Name;
                itDir.Tag = v;

                TreeViewItem noner = new TreeViewItem();
                noner.Header = "None";
                noner.Tag = "None";
                itDir.Items.Add(noner);
                itDir.Expanded += Expander;
                itemRoot.Items.Add(itDir);
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog fold = new System.Windows.Forms.FolderBrowserDialog();
            fold.Description = "Выберите каталог";
            if(fold.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                SetBrowserDir(fold.SelectedPath);
            }
        }

        private void BrowserIO_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(BrowserIO.SelectedItem != null)
            {
                string path = (BrowserIO.SelectedItem as TreeViewItem).Tag.ToString();
                if (File.Exists(path))
                {
                    textBlock.Text = File.ReadAllText(path);
                }
            }
        }
    }
}
