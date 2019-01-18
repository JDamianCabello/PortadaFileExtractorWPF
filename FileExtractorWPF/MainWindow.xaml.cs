using CodeExtractor;
using FolderSelect; //Original author http://www.lyquidity.com/devblog/?p=136 (MIT license)
//-------------------
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace FileExtractorWPF
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnRutaInicialClick(object sender, RoutedEventArgs e)
        {

            var fsd = new FolderSelectDialog();
            fsd.Title = "Selecciona la carpeta con tus ejercicios";
            fsd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            btnRutaFinal.IsEnabled = false;
            this.IsEnabled = false;
            if (fsd.ShowDialog(IntPtr.Zero))
            {
                tbxRutaInicio.Text = fsd.FileName;
                if(exprArbol.IsExpanded)
                    ListarDirectorios(trvVisorCarpetas, fsd.FileName);              
            }
            btnRutaFinal.IsEnabled = true;
            this.IsEnabled = true;
        }

        
        private void ListarDirectorios(System.Windows.Controls.TreeView treeView, string path)
        {
            treeView.Items.Clear();
            var rootDirectoryInfo = new DirectoryInfo(path);
            try
            {
                treeView.Items.Add(CrearDirectoriosyNodos(rootDirectoryInfo));
            }
            catch
            {
                treeView.Items.Add(new TreeViewItem { Header = "ERROR de permisos, no se carga nada aún así busca" });
            }
            exprArbol.IsExpanded = true;
        }

        private TreeViewItem CrearDirectoriosyNodos(DirectoryInfo directoryInfo)
        {
            var directoryNode = new TreeViewItem { Header = directoryInfo.Name };


            foreach (var directory in directoryInfo.GetDirectories())
                directoryNode.Items.Add(CrearDirectoriosyNodos(directory));

            foreach (var file in directoryInfo.GetFiles())
                directoryNode.Items.Add(new TreeViewItem { Header = file.Name });

            return directoryNode;

        }

        private void CbxExpand_Checked(object sender, RoutedEventArgs e)
        {
            exprArbol.IsExpanded = true;
            exprArbol.Visibility = Visibility.Visible;
            if (Directory.Exists(tbxRutaInicio.Text))
                ListarDirectorios(trvVisorCarpetas, tbxRutaInicio.Text);
        }

        private void CbxExpand_Unchecked(object sender, RoutedEventArgs e)
        {
            exprArbol.IsExpanded = false;
            exprArbol.Visibility = Visibility.Hidden;
        }

        private void BtnIniciar_Click(object sender, RoutedEventArgs e)
        {
            Copy.ExpecificFiles(tbxRutaInicio.Text, tbxRutaFinal.Text, cbxCs.IsChecked.Value, cbxExe.IsChecked.Value, cbxDll.IsChecked.Value, cbxComprimir.IsChecked.Value, cbxBorrar.IsChecked.Value, cbxExploradorWindows.IsChecked.Value);
        }

        private void btnRutaFinalClick(object sender, RoutedEventArgs e)
        {
            var fsd = new FolderSelectDialog();
            fsd.Title = "Selecciona la carpeta para extarer los ficheros";
            fsd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            btnRutaInicio.IsEnabled = false;
            this.IsEnabled = false;
            if (fsd.ShowDialog(IntPtr.Zero))
            {
                tbxRutaFinal.Text = fsd.FileName;
            }
            btnRutaInicio.IsEnabled = true;
            this.IsEnabled = true;
        }

        private void ExpanderCollapsed(object sender, RoutedEventArgs e)
        {
            ventanaPrincipal.Width = 372.826;
        }

        private void ExpanderExpanded(object sender, RoutedEventArgs e)
        {
            ventanaPrincipal.Width = 691.161;
        }

        private void TbxRutaInicio_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Directory.Exists(tbxRutaInicio.Text))
                if (exprArbol.IsExpanded)
                    ListarDirectorios(trvVisorCarpetas, tbxRutaInicio.Text);
                else if (tbxRutaInicio.Text == "" || Directory.Exists(tbxRutaInicio.Text))
                {
                    trvVisorCarpetas.Items.Clear();
                    exprArbol.IsExpanded = false;
                }
        }

        private void CbxComprimir_Checked(object sender, RoutedEventArgs e)
        {
            cbxBorrar.IsEnabled = cbxComprimir.IsChecked.Value;
        }

        private void CbxComprimir_Unchecked(object sender, RoutedEventArgs e)
        {
            cbxBorrar.IsEnabled = cbxComprimir.IsChecked.Value;
            cbxBorrar.IsChecked = cbxComprimir.IsChecked.Value;
        }
    }
}
