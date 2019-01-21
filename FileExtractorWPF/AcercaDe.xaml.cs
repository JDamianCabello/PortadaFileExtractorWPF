using System.Windows;
using System.Windows.Input;


namespace FileExtractorWPF
{
    /// <summary>
    /// Lógica de interacción para Window1.xaml
    /// </summary>
    public partial class AcercaDe : Window
    {
        public AcercaDe()
        {
            InitializeComponent();
        }

        private void Link_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start(link.Text);
        }
    }
}
