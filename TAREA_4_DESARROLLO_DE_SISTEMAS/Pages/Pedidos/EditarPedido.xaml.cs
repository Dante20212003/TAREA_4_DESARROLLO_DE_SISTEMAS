using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CNegocio;
using HandyControl.Controls;
using ComboBox = System.Windows.Controls.ComboBox;
using TextBox = HandyControl.Controls.TextBox;

namespace TAREA_4_DESARROLLO_DE_SISTEMAS.Pages.Pedidos
{
    /// <summary>
    /// Interaction logic for EditarPedido.xaml
    /// </summary>
    public partial class EditarPedido : UserControl
    {


        private Pedido pedido;
        public EditarPedido(Pedido _pedido)
        {
            InitializeComponent();

            if (_pedido != null)
            {
                pedido = _pedido;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ListarProductos();
            ListarClientes();

            cbxCliente.SelectedValue = pedido.Cliente_id;
            cbxProducto.SelectedValue = pedido.Producto_id;
            txtCantidad.Text = pedido.Cantidad.ToString();
            txtMonto.Text = pedido.Monto.ToString();
            txtFecha.Text = pedido.Fecha;
        }

        public void ListarProductos()
        {
            Producto producto = new Producto();
            var data = producto.GetProductos();

            cbxProducto.DisplayMemberPath = "Descripcion";
            cbxProducto.SelectedValuePath = "Id";
            cbxProducto.ItemsSource = data;
        }

        public void ListarClientes()
        {

            Cliente cliente = new Cliente();
            var data = cliente.GetClientes();

            cbxCliente.DisplayMemberPath = "Nombre";
            cbxCliente.SelectedValuePath = "Id";
            cbxCliente.ItemsSource = data;
        }

        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            bool isError = false;

            Validar();

            if (!ValidateProducto(cbxProducto.Text) || !ValidateCliente(cbxCliente.Text) || !ValidateCantidad(txtCantidad.Text) || !ValidateMonto(txtMonto.Text) || !ValidateFecha(txtFecha.Text)) isError = true;

            if (isError)
            {
                MainWindow._ts.ShowError("Revise que todos los datos sean validos");
                return;
            }

            int cliente_id = ((Cliente)cbxCliente.SelectedItem).Id;
            int producto_id = ((Producto)cbxProducto.SelectedItem).Id;

            Pedido pedido = new Pedido()
            {
                Id = this.pedido.Id,
                Cliente_id = cliente_id,
                Producto_id = producto_id,
                Cantidad = decimal.Parse(txtCantidad.Text),
                Monto = decimal.Parse(txtMonto.Text),
                Fecha = txtFecha.Text
            };

            pedido.Id = this.pedido.Id;
            pedido.ActualizarPedido();
            MainWindow._ts.ShowSuccess("Pedido actualizado con exito");
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            string campo = txt.Name;

            if (campo == "cbxProducto")
            {
                ValidateProducto(txt.Text);
                return;
            };
            if (campo == "txtCantidad")
            {
                ValidateCantidad(txt.Text);
                return;
            }
            if (campo == "txtMonto")
            {
                ValidateMonto(txt.Text);
                return;
            }

        }

        private void fechaDateTimeChanged(object sender, HandyControl.Data.FunctionEventArgs<DateTime?> e)
        {
            string fechaSelect = ((DateTimePicker)sender).Text;

            ValidateFecha(fechaSelect);
        }

        private void cbxProducto_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string cbxProducto = (sender as ComboBox).SelectedItem as string;
            ValidateProducto(cbxProducto);
        }
        private void cbxCliente_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string cbxCliente = (sender as ComboBox).SelectedItem as string;
            ValidateCliente(cbxCliente);
        }
        private void cbxUnidad_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string cbx = (sender as ComboBox).SelectedItem as string;
            ValidateCliente(cbx);

            int posicionComa = txtMonto.Text.IndexOf(',');
            if (comprobarUnidad(cbx) && posicionComa > 1) txtMonto.Text = txtMonto.Text.Substring(0, txtMonto.Text.IndexOf(','));
        }

        private bool ValidateProducto(string value)
        {
            if (value == "")
            {
                lblProducto.Content = "El producto es requerido";
                lblProducto.Visibility = Visibility.Visible;
                return false;
            }

            try
            {
                int producto_id = ((Producto)cbxProducto.SelectedItem).Id;
            }
            catch
            {
                lblProducto.Content = "Compruebe que el pruducto sea valido";
                lblProducto.Visibility = Visibility.Visible;
                return false;
            }

            lblProducto.Visibility = Visibility.Collapsed;
            return true;
        }

        private bool ValidateCliente(string value)
        {

            if (value == "")
            {
                lblCliente.Content = "El cliente es requerido";
                lblCliente.Visibility = Visibility.Visible;
                return false;
            }

            try
            {
                int cliente_id = ((Cliente)cbxCliente.SelectedItem).Id;
            }
            catch
            {
                lblCliente.Content = "Compruebe que el cliente sea valido";
                lblCliente.Visibility = Visibility.Visible;
                return false;
            }

            lblCliente.Visibility = Visibility.Collapsed;
            return true;
        }

        private bool ValidateCantidad(string value)
        {
            if (value == "")
            {
                lblCantidad.Content = "La cantidad es requerido";
                lblCantidad.Visibility = Visibility.Visible;
                return false;
            }

            lblCantidad.Visibility = Visibility.Collapsed;
            return true;
        }

        private bool ValidateMonto(string value)
        {
            if (value == "")
            {
                lblMonto.Content = "El monto es requerido";
                lblMonto.Visibility = Visibility.Visible;
                return false;
            }

            lblMonto.Visibility = Visibility.Collapsed;
            return true;
        }

        private bool ValidateFecha(string value)
        {
            if (value == "")
            {
                lblFecha.Content = "La fecha es requerida";
                lblFecha.Visibility = Visibility.Visible;
                return false;
            }

            if (DateTime.TryParse(value, out DateTime dateTime) != true)
            {
                lblFecha.Content = "La fecha no es valida";
                lblFecha.Visibility = Visibility.Visible;
            }

            lblFecha.Visibility = Visibility.Collapsed;
            return true;
        }

        private void OnlyDecimal_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[,][0-9]+$|^[0-9]*[,]{0,1}[0-9]*$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }

        private void OnlyInt_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (comprobarUnidad(cbxCliente.Text))
            {
                Regex regexInt = new Regex("[^0-9]+");
                e.Handled = regexInt.IsMatch(e.Text);
                return;
            }

            Regex regex = new Regex("^[,][0-9]+$|^[0-9]*[,]{0,1}[0-9]*$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }

        private bool comprobarUnidad(string val)
        {
            string[] soloEnteros = new string[] { "Unidad", "Caja", "Bolsa", "Paquete" };

            return soloEnteros.Contains(val);

        }

        private void Validar()
        {
            ValidateProducto(cbxProducto.Text);
            ValidateCliente(cbxCliente.Text);
            ValidateCantidad(txtCantidad.Text);
            ValidateMonto(txtMonto.Text);
            ValidateFecha(txtFecha.Text);
        }
    }
}
