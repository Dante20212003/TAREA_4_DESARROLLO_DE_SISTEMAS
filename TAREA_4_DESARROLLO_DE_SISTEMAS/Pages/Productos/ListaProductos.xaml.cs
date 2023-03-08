using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CNegocio;
using HandyControl.Controls;

namespace TAREA_4_DESARROLLO_DE_SISTEMAS.Pages.Productos
{
    /// <summary>
    /// Interaction logic for ListaProductos.xaml
    /// </summary>
    public partial class ListaProductos : Page
    {
        public ListaProductos()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

            ListarProductos();
        }

        public void ListarProductos(string query = "")
        {


            if (DataGrid != null)
            {
                Producto producto = new Producto();
                var data = producto.GetProductos(query);

                if (data.Count > 0)
                {
                    DataGrid.ItemsSource = data;
                }
                else
                {
                    MainWindow.mostrarToast(MainWindow._ts.ShowInformation, "Aun no hay productos, empieze a crear nuevos");
                }
            }
        }

        private void actualizarCtxItem_Click(object sender, RoutedEventArgs e)
        {
            int totalSeleccionados = DataGrid.SelectedItems.Count;

            if (totalSeleccionados == 0)
            {
                MainWindow.mostrarToast(MainWindow._ts.ShowInformation, "Debe seleccionar un registro");
                return;
            }

            Producto productoRow = (Producto)DataGrid.SelectedItem;
            ModalProducto(true, productoRow);
        }

        private void DataGrid_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {

            if (Keyboard.Modifiers.Equals(ModifierKeys.Control))
            {
                if (e.Delta < 0)
                {
                    scrollTabla.LineRight();
                }
                else
                {
                    scrollTabla.LineLeft();
                }
            }
            else
            {
                scrollTabla.ScrollToVerticalOffset(scrollTabla.VerticalOffset - e.Delta);
            }
        }

        private void BtnNuevoCliente_Click(object sender, RoutedEventArgs e)
        {
            ModalProducto();
        }

        private void ModalProducto(bool isEdit = false, Producto producto = null)
        {
            PopupWindow popup = new PopupWindow()
            {
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                AllowsTransparency = true,
                WindowStyle = WindowStyle.None,
                Effect = null,
            };

            if (isEdit) popup.PopupElement = new CrearProducto(producto);
            else popup.PopupElement = new CrearProducto();

            if (popup.ShowDialog() != true)
            {
                ListarProductos();
            }
        }

        private void Search_SearchStarted(object sender, HandyControl.Data.FunctionEventArgs<string> e)
        {
            string query = ((SearchBar)sender).Text;
            if (query.Length > 0)
            {
                ListarProductos(query);
            }
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            string txt = ((SearchBar)sender).Text;

            if (txt.Length == 0)
            {
                ListarProductos();
            }
        }
    }
}
