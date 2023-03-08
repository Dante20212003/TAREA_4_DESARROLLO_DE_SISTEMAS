using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HandyControl.Data;
using ToastNotifications.Core;
using MessageBox = HandyControl.Controls.MessageBox;
using SideMenuItem = HandyControl.Controls.SideMenuItem;

namespace TAREA_4_DESARROLLO_DE_SISTEMAS
{
    public partial class MainWindow : Window
    {
        public static Toast.Toast _ts;


        private bool isMaximized = false;

        Dictionary<string, string> paginas = new Dictionary<string, string>() {
            {"Analiticas", "Pages/Dashboard.xaml"},
            {"Lista de Clientes", "Pages/Clientes/ListaClientes.xaml"},
            {"Lista de Productos", "Pages/Productos/ListaProductos.xaml"},
            {"Lista de Pedidos", "Pages/Pedidos/ListaPedidos.xaml"},
            {"Nuevo Pedido", "Pages/Pedidos/CrearPedido.xaml"},
            {"Generacion de Data", "Pages/GenerarData.xaml"},
        };

        public MainWindow()
        {
            try
            {
                InitializeComponent();
                _ts = new Toast.Toast();
                Unloaded += OnUnload;

                mainNavigaion.Navigate(new Uri("Pages/Dashboard.xaml", UriKind.RelativeOrAbsolute));
                            }
            catch
            {
                MessageBox.Show("awdawd");
            }
          
        }

        public static void mostrarToast(Action<string, MessageOptions> action, string msg = "")
        {
            MessageOptions opts = new MessageOptions
            {
                FreezeOnMouseEnter = true,
                UnfreezeOnMouseLeave = true,
                ShowCloseButton = true,
                FontSize = 14,

            };
            action(msg, opts);

        }

        private void OnUnload(object sender, RoutedEventArgs e)
        {
            _ts.OnUnloaded();
        }

        private void btnCerrarVentana(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(new MessageBoxInfo
            {
                Message = "Estas seguro de cerrar la aplicacion",
                Caption = "Alerta",
                Button = MessageBoxButton.YesNo,
                IconKey = ResourceToken.AskGeometry,
                IconBrushKey = ResourceToken.AccentBrush,
                ConfirmContent = "Si",
            }); ;

            if (result == MessageBoxResult.Yes) Close();
        }

        private void btnMaximizarVentana(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
                WindowState = WindowState.Maximized;
            else
                WindowState = WindowState.Normal;
        }

        private void btnMinimizarVentana(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) DragMove();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount != 2) return;

            if (!isMaximized)
            {
                WindowState = WindowState.Maximized;
                isMaximized = true;
            }
            else
            {
                WindowState = WindowState.Normal;
                Width = 1080;
                Height = 720;
                isMaximized = false;
            }
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {

            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;

        }

        private void SideMenu_SelectionChanged(object sender, FunctionEventArgs<object> e)
        {
            try
            {
                SideMenuItem sideMenuItem = e.Info as SideMenuItem;
                string pagina = sideMenuItem?.Header?.ToString();
                mainNavigaion.Navigate(new Uri(paginas[pagina], UriKind.RelativeOrAbsolute));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
