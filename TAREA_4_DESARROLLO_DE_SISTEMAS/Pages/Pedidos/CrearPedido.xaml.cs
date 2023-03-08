using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CNegocio;
using HandyControl.Controls;
using ComboBox = System.Windows.Controls.ComboBox;
using MessageBox = HandyControl.Controls.MessageBox;
using Window = System.Windows.Window;

namespace TAREA_4_DESARROLLO_DE_SISTEMAS.Pages.Pedidos
{
    /// <summary>
    /// Interaction logic for CrearPedido.xaml
    /// </summary>
    public partial class CrearPedido : Page
    {
        List<Pedido> listaPedido = new List<Pedido>();
        List<Producto> listaProductos = new List<Producto>();

        decimal montoTotal = 0;

        public CrearPedido()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ListarProductos();
            ListarClientes();
            DataGridPedido.ItemsSource = listaPedido;

        }

        public void ListarProductos(string buscar = "")
        {
            Producto producto = new Producto();
            listaProductos = producto.GetProductos(buscar);

            DataGridProductos.ItemsSource = listaProductos;
        }

        public void ListarClientes()
        {

            Cliente cliente = new Cliente();
            var data = cliente.GetClientes();

            cbxCliente.DisplayMemberPath = "Nombre";
            cbxCliente.SelectedValuePath = "Id";
            cbxCliente.ItemsSource = data;
        }

        private void ClearPedido()
        {
            DataGridPedido.ItemsSource = null;
            listaPedido.Clear();

            cbxCliente.SelectedItem = null;
            txtNit.Text = "";
            txtTelefono.Text = "";

            btnRealizarPedido.Visibility = Visibility.Hidden;
            totalPagar.Text = "";
        }

        private bool ComprobarUnidad(string val)
        {
            string[] soloEnteros = new string[] { "Unidad", "Caja", "Bolsa", "Paquete" };

            return soloEnteros.Contains(val);

        }

        private void cbxCliente_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Cliente cbx = (sender as ComboBox).SelectedItem as Cliente;

                if (cbx != null)
                {
                    txtTelefono.Text = cbx.Telefono;
                    txtNit.Text = cbx.Nit;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void agregarAPedidoCtxItem_Click(object sender, RoutedEventArgs e)
        {
            int i = listaPedido.Count + 1;

            if (DataGridProductos.SelectedItems.Count == 0)
            {
                MainWindow.mostrarToast(MainWindow._ts.ShowInformation, $"Seleccione un producto");
                return;
            }

            Producto productoRow = (Producto)DataGridProductos.SelectedItem;

            if (productoRow.Cantidad == 0)
            {
                MainWindow.mostrarToast(MainWindow._ts.ShowError, $"El producto {productoRow.Descripcion} no cuenta con stock disponible");
            }
            else if (productoRow.Cantidad > productoRow.Stock)
            {
                MainWindow.mostrarToast(MainWindow._ts.ShowWarning, $"El producto {productoRow.Descripcion} no tiene suficiente stock");
            }
            else if (ComprobarUnidad(productoRow.Unidad) && productoRow.Cantidad < 1)
            {
                MainWindow.mostrarToast(MainWindow._ts.ShowWarning, $"La cantidad para {productoRow.Unidad} debe ser mayor a 0");
            }
            else
            {
                Pedido pedido = new Pedido()
                {
                    Id = i,
                    Producto_id = productoRow.Id,
                    Producto = productoRow.Descripcion,
                    Cantidad = (decimal)productoRow.Cantidad,
                    Monto = (decimal)Math.Round((double)(productoRow.Cantidad * productoRow.Precio), 2)
                };

                Pedido existeItem = listaPedido.Where(ped => ped.Producto_id == pedido.Producto_id).FirstOrDefault();

                if (existeItem != null)
                {
                    existeItem.Cantidad += (decimal)productoRow.Cantidad;
                    existeItem.Monto += (decimal)Math.Round((double)(productoRow.Cantidad * productoRow.Precio), 2);
                }
                else
                {
                    i++;
                    listaPedido.Add(pedido);
                }

                decimal nuevoStock = (decimal)(productoRow.Stock - productoRow.Cantidad);
                productoRow.Stock = nuevoStock;

                if (nuevoStock == 0)
                {
                    DataGridProductos.SelectedItem = null;
                    productoRow.Cantidad = 0;
                }

                montoTotal += pedido.Monto;

                DataGridPedido.ItemsSource = listaPedido;
                DataGridPedido.Items.Refresh();
                DataGridProductos.Items.Refresh();

                if (montoTotal >= 0)
                {
                    totalPagar.Text = $"Monto Total: {montoTotal}";
                    btnRealizarPedido.Visibility = Visibility.Visible;
                    btnVaciarPedido.Visibility = Visibility.Visible;
                }

                MainWindow.mostrarToast(MainWindow._ts.ShowSuccess, $"Producto agregados al pedido correctamente");
            }
        }

        private void quitarPedido_Click(object sender, RoutedEventArgs e)
        {

            if (DataGridPedido.SelectedItems.Count == 0)
            {
                MainWindow.mostrarToast(MainWindow._ts.ShowInformation, $"Seleccione un registro");
                return;
            }

            Pedido pedidoRow = (Pedido)DataGridPedido.SelectedItem;

            Producto existeItem = listaProductos.Where(pr => pr.Id == pedidoRow.Producto_id).FirstOrDefault();

            if (existeItem != null)
            {
                existeItem.Stock += pedidoRow.Cantidad;
                existeItem.Cantidad = 1;

                listaPedido.Remove(pedidoRow);

                DataGridProductos.Items.Refresh();
                DataGridPedido.Items.Refresh();

                montoTotal -= pedidoRow.Monto;

                if (montoTotal == 0)
                {
                    totalPagar.Text = "";
                    btnRealizarPedido.Visibility = Visibility.Hidden;
                    btnVaciarPedido.Visibility = Visibility.Hidden;
                }
            }
        }

        private void actualizarCtxItem_Click(object sender, RoutedEventArgs e)
        {
        }

        private void RealizarPedido_Click(object sender, RoutedEventArgs e)
        {
            if (txtNit.Text == "" || cbxCliente.Text == "" || txtTelefono.Text == "")
            {
                MainWindow.mostrarToast(MainWindow._ts.ShowWarning, "Debe agregar un cliente para realizar el pedido");
                return;
            }

            Cliente cliente = (Cliente)cbxCliente.SelectedItem;
            Producto producto = new Producto();

            MessageBoxResult result = MessageBox.Show($"Desea confirmar el pedido para {cliente.Nombre}?", "Alerta", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                foreach (Pedido pedidoRow in listaPedido)
                {
                    producto.Id = pedidoRow.Producto_id;
                    producto.ActualizarStock(pedidoRow.Cantidad);

                    pedidoRow.Cliente_id = cliente.Id;
                    pedidoRow.CrearPedido();
                }

                ClearPedido();
                ListarProductos();
                MainWindow.mostrarToast(MainWindow._ts.ShowSuccess, "Pedido generado con exito");
            }
        }

        private void VaciarPedido_Click(object sender, RoutedEventArgs e)
        {
            ListarProductos();

            listaPedido.Clear();
            DataGridPedido.ItemsSource = null;

            btnRealizarPedido.Visibility = Visibility.Hidden;
            btnVaciarPedido.Visibility = Visibility.Hidden;
            totalPagar.Text = "";
        }

        private void NumericUpDown_ValueChanged(object sender, HandyControl.Data.FunctionEventArgs<double> e)
        {
            NumericUpDown cantidad = (NumericUpDown)sender;
            Producto producto = (Producto)DataGridProductos.SelectedItem;

            if (producto != null)
            {
                if (ComprobarUnidad(producto.Unidad))
                {
                    string value = cantidad.Value.ToString();

                    if (value.Contains(","))
                    {
                        int comma = value.IndexOf(',');
                        if (comma > 0)
                        {
                            cantidad.Value = double.Parse(value.Substring(0, comma));
                        }
                        cantidad.DecimalPlaces = 0;
                    }
                }
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

        private void RegresarAtras_Click(object sender, RoutedEventArgs e)
        {
            var mw = Application.Current.Windows
    .Cast<Window>()
    .FirstOrDefault(window => window is MainWindow) as MainWindow;

            mw.mainNavigaion.Content = new ListaPedidos();
        }
    }


}
