using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CNegocio;
using TextBox = HandyControl.Controls.TextBox;

namespace TAREA_4_DESARROLLO_DE_SISTEMAS.Pages.Clientes
{
    public partial class CrearCliente : UserControl
    {
        private Cliente cliente;
        public CrearCliente(Cliente _cliente = null)
        {
            InitializeComponent();

            if (_cliente != null)
            {
                cliente = _cliente;
            }

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (cliente != null)
            {
                title.Text = "Actualizar Cliente";
                btnAgregar.Content = "Actualizar";
                txtNombre.Text = cliente.Nombre;
                txtTelefono.Text = cliente.Telefono;
                txtNit.Text = cliente.Nit;
            }
        }

        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            bool isError;
            isError = ValidateNombre("txtNombre", txtNombre.Text);
            isError = ValidateTelefono("txtTelefono", txtTelefono.Text);
            isError = ValidateNit("txtNit", txtNit.Text);

            if (!isError)
            {
                MainWindow._ts.ShowError("Revise que todos los datos sean validos");
                return;
            }

            Cliente cliente = new Cliente()
            {
                Nombre = txtNombre.Text,
                Telefono = txtTelefono.Text,
                Nit = txtNit.Text
            };

            if (this.cliente != null)
            {
                cliente.Id = this.cliente.Id;
                cliente.ActualizarCliente();
                MainWindow._ts.ShowSuccess("Cliente actualizado con exito");
            }
            else
            {
                cliente.CrearCliente();
                ClearInputs();
                MainWindow._ts.ShowSuccess("Cliente agregado con exito");
            }


        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            string campo = txt.Name;

            if (campo == "txtNombre")
            {
                ValidateNombre(campo, txt.Text);
                return;
            };
            if (campo == "txtTelefono")
            {
                ValidateTelefono(campo, txt.Text);
                return;
            }
            if (campo == "txtNit")
            {
                ValidateNit(campo, txt.Text);
                return;
            }
        }

        private void ClearInputs()
        {
            txtNombre.Text = "";
            txtTelefono.Text = "";
            txtNit.Text = "";
            lblNombre.Visibility = Visibility.Collapsed;
            lblTelefono.Visibility = Visibility.Collapsed;
            lblNit.Visibility = Visibility.Collapsed;

        }

        private bool ValidateNombre(string campo, string value)
        {
            if (value == "")
            {
                lblNombre.Content = "El nombre es requerido";
                lblNombre.Visibility = Visibility.Visible;
                return false;
            }

            if (value.Length <= 3)
            {
                lblNombre.Content = "El nombre debe tener mas de 3 caracteres";
                lblNombre.Visibility = Visibility.Visible;
                return false;
            }

            lblNombre.Visibility = Visibility.Collapsed;
            return true;
        }

        private bool ValidateTelefono(string campo, string value)
        {
            if (value == "")
            {
                lblTelefono.Content = "El telefono es requerido";
                lblTelefono.Visibility = Visibility.Visible;
                return false;
            }

            if (value.Length <= 7)
            {
                lblTelefono.Content = "El telefono debe tener al menos 8 digitos";
                lblTelefono.Visibility = Visibility.Visible;
                return false;
            }

            if (value.Length >= 12)
            {
                lblTelefono.Content = "El telefono no es valido";
                lblTelefono.Visibility = Visibility.Visible;
                return false;
            }

            lblTelefono.Visibility = Visibility.Collapsed;
            return true;
        }

        private bool ValidateNit(string campo, string value)
        {
            if (value == "")
            {
                lblNit.Content = "El nit es requerido";
                lblNit.Visibility = Visibility.Visible;
                return false;
            }

            if (value.Length <= 7)
            {
                lblNit.Content = "El nit debe tener al menos 8 digitos";
                lblNit.Visibility = Visibility.Visible;
                return false;
            }

            if (value.Length >= 15)
            {
                lblNit.Content = "El nit no es valido";
                lblNit.Visibility = Visibility.Visible;
                return false;
            }

            lblNit.Visibility = Visibility.Collapsed;
            return true;
        }

        private void OnlyNumber_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }

}
