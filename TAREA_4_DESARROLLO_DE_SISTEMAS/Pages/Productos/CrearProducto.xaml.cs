using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CNegocio;

namespace TAREA_4_DESARROLLO_DE_SISTEMAS.Pages.Productos
{
    /// <summary>
    /// Interaction logic for CrearProducto.xaml
    /// </summary>
    public partial class CrearProducto : UserControl
    {
        public CrearProducto()
        {
            InitializeComponent();
        }

        private Producto producto;
        public CrearProducto(Producto _producto = null)
        {
            InitializeComponent();

            if (_producto != null)
            {
                producto = _producto;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

            cbxUnidad.Items.Add("Bolsa");
            cbxUnidad.Items.Add("Caja");
            cbxUnidad.Items.Add("Kilogramo");
            cbxUnidad.Items.Add("Litros");
            cbxUnidad.Items.Add("Metros");
            cbxUnidad.Items.Add("Paquete");
            cbxUnidad.Items.Add("Unidad");

            if (producto != null)
            {
                title.Text = "Actualizar Producto";
                btnAgregar.Content = "Actualizar";
                txtDescripcion.Text = producto.Descripcion;
                cbxUnidad.SelectedItem = producto.Unidad;
                txtPrecio.Text = producto.Precio.ToString();
                txtStock.Text = producto.Stock.ToString();
            }
        }

        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            bool isError = false;

            Validar();

            if (!ValidateDescripcion(txtDescripcion.Text) || !ValidateUnidad(cbxUnidad.Text) || !ValidatePrecio(txtPrecio.Text) || !ValidateStock(txtStock.Text)) isError = true;


            if (isError)
            {
                MainWindow._ts.ShowError("Revise que todos los datos sean validos");
                return;
            }

            Producto producto = new Producto()
            {
                Descripcion = txtDescripcion.Text,
                Unidad = cbxUnidad.Text,
                Precio = decimal.Parse(txtPrecio.Text),
                Stock = decimal.Parse(txtStock.Text)

            };

            if (this.producto != null)
            {
                producto.Id = this.producto.Id;
                producto.ActualizarProducto();
                MainWindow._ts.ShowSuccess("Producto actualizado con exito");
            }
            else
            {
                producto.CrearProducto();
                ClearInputs();
                MainWindow._ts.ShowSuccess("Producto agregado con exito");
            }


        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            string campo = txt.Name;

            if (campo == "txtDescripcion")
            {
                ValidateDescripcion(txt.Text);
                return;
            };
            if (campo == "txtPrecio")
            {
                ValidatePrecio(txt.Text);
                return;
            }
            if (campo == "txtStock")
            {
                ValidateStock(txt.Text);
                return;
            }

        }

        private void cbxUnidad_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string cbx = (sender as ComboBox).SelectedItem as string;
            ValidateUnidad(cbx);

            int posicionComa = txtStock.Text.IndexOf(',');
            if (comprobarUnidad(cbx) && posicionComa > 1) txtStock.Text = txtStock.Text.Substring(0, txtStock.Text.IndexOf(','));
        }

        private void ClearInputs()
        {
            txtDescripcion.Text = "";
            cbxUnidad.Text = "";
            txtPrecio.Text = "";
            txtStock.Text = "";
            lblDescripcion.Visibility = Visibility.Collapsed;
            lblUnidad.Visibility = Visibility.Collapsed;
            lblPrecio.Visibility = Visibility.Collapsed;
            lblStock.Visibility = Visibility.Collapsed;

        }

        private bool ValidateDescripcion(string value)
        {
            if (value == "")
            {
                lblDescripcion.Content = "La descripcion es requerido";
                lblDescripcion.Visibility = Visibility.Visible;
                return false;
            }

            if (value.Length <= 3)
            {
                lblDescripcion.Content = "La descripcion debe tener mas de 3 caracteres";
                lblDescripcion.Visibility = Visibility.Visible;
                return false;
            }

            lblDescripcion.Visibility = Visibility.Collapsed;
            return true;
        }

        private bool ValidateUnidad(string value)
        {

            if (value == "")
            {
                lblUnidad.Content = "La unidad es requerida";
                lblUnidad.Visibility = Visibility.Visible;
                return false;
            }


            lblUnidad.Visibility = Visibility.Collapsed;
            return true;
        }

        private bool ValidatePrecio(string value)
        {
            if (value == "")
            {
                lblPrecio.Content = "El precio es requerido";
                lblPrecio.Visibility = Visibility.Visible;
                return false;
            }

            lblPrecio.Visibility = Visibility.Collapsed;
            return true;
        }

        private bool ValidateStock(string value)
        {
            if (value == "")
            {
                lblStock.Content = "El stock es requerido";
                lblStock.Visibility = Visibility.Visible;
                return false;
            }

            lblStock.Visibility = Visibility.Collapsed;
            return true;
        }

        private void OnlyDecimal_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[,][0-9]+$|^[0-9]*[,]{0,1}[0-9]*$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }

        private void OnlyInt_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (comprobarUnidad(cbxUnidad.Text))
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
            ValidateDescripcion(txtDescripcion.Text);
            ValidateUnidad(cbxUnidad.Text);
            ValidatePrecio(txtPrecio.Text);
            ValidateStock(txtStock.Text);
        }
    }
}
