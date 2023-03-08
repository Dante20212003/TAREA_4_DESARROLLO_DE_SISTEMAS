using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using Bogus;
using CDatos;

namespace CNegocio
{
    public class Producto
    {
        private int id;
        private string descripcion;
        private string unidad;
        private decimal precio;
        private decimal stock;
        private decimal? cantidad;

        ProductoD productoD = new ProductoD();

        public int Id { get => id; set => id = value; }
        public string Descripcion { get => descripcion; set => descripcion = value; }
        public string Unidad { get => unidad; set => unidad = value; }
        public decimal Precio { get => precio; set => precio = value; }
        public decimal Stock { get => stock; set => stock = value; }
        public decimal? Cantidad { get => cantidad; set => cantidad = value; }

        public List<Producto> GetProductos(string query = "")
        {
            List<Producto> listaProductos = new List<Producto>();

            foreach (DataRow R in productoD.SelectData(query).Rows)
            {
                Producto producto = new Producto
                {
                    Id = int.Parse(R["id"].ToString()),
                    Descripcion = R["descripcion"].ToString(),
                    Unidad = R["unidad"].ToString(),
                    Precio = decimal.Parse(R["precio"].ToString()),
                    Stock = decimal.Parse(R["stock"].ToString()),
                    Cantidad = 1,
                };

                if (producto.Stock <= 0) producto.Cantidad = 0;

                listaProductos.Add(producto);
            }

            return listaProductos;
        }

        public void ActualizarStock(decimal valor, bool increment = true)
        {
            decimal stockActual = (decimal)productoD.SelectOneData(Id)["stock"];
            if (valor > stockActual)
            {
                MessageBox.Show(valor + " " + stockActual);
            }
            productoD.UpdateStock(Id, stockActual - valor);
        }

        public void CrearProducto()
        {
            productoD.InsertData(Descripcion, Unidad, Precio, Stock);
        }

        public void ActualizarProducto()
        {
            productoD.UpdateData(Id, Descripcion, Unidad, Precio, Stock);
        }

        public void GenerarProductos(int limit)
        {
            Faker faker = new Faker("es_MX");

            var unidades = new[] { "Unidad", "Caja", "Bolsa", "Paquete", "Kilogramos", "Litros" };

            for (int i = 0; i < limit; i++)
            {
                //var producto = productos.Generate();
                Producto producto = new Producto()
                {
                    Descripcion = faker.Commerce.ProductName(),
                    Unidad = faker.PickRandom(unidades),
                    Precio = Math.Round(faker.Random.Decimal(0, 500), 2),
                };

                if (ComprobarUnidad(producto.Unidad)) producto.Stock = (int)faker.Random.Decimal(0, 100);
                else producto.Stock = Math.Round(faker.Random.Decimal(0, 100), 2);

                productoD.InsertData(producto.Descripcion, producto.Unidad, producto.Precio, producto.Stock);
            }
        }

        private bool ComprobarUnidad(string val)
        {
            string[] soloEnteros = new string[] { "Unidad", "Caja", "Bolsa", "Paquete" };

            return soloEnteros.Contains(val);

        }
    }
}
