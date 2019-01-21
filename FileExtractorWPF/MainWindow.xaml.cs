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
                    ListarDirectorios();              
            }
            btnRutaFinal.IsEnabled = true;
            this.IsEnabled = true;
        }

        
        private void ListarDirectorios()
        {
            trvVisorCarpetas.Items.Clear();
            var rootDirectoryInfo = new DirectoryInfo(tbxRutaInicio.Text);
            try
            {
                trvVisorCarpetas.Items.Add(CrearDirectoriosyNodos(rootDirectoryInfo));
            }
            catch
            {
                trvVisorCarpetas.Items.Add(new TreeViewItem { Header = "ERROR de permisos, no se carga nada aún así busca" });
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
                ListarDirectorios();
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
            fsd.Title = "Selecciona la carpeta para extraer los ficheros";
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
            ventanaPrincipal.Width = 373;
        }

        private void ExpanderExpanded(object sender, RoutedEventArgs e)
        {
            ventanaPrincipal.Width = 690;
        }

        private void TbxRutaInicio_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Directory.Exists(tbxRutaInicio.Text))
                if (exprArbol.IsExpanded)
                    ListarDirectorios();
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

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            AcercaDe acercaDe = new AcercaDe();
            acercaDe.Show();
        }
    }
}
