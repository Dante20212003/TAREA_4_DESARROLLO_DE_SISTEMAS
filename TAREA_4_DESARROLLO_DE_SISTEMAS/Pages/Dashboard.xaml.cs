using System.Windows;
using System.Windows.Controls;
using CNegocio;

namespace TAREA_4_DESARROLLO_DE_SISTEMAS.Pages
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Page
    {
        public Dashboard()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ListarPedidos();
        }

        public void ListarPedidos()
        {
            try
            {


                Pedido pedido = new Pedido();
                var data = pedido.GetPedidos(true);

                DataGrid.ItemsSource = data;
            }
            catch
            {

            }
        }
    }
}
