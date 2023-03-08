using System;
using System.Data;
using System.Globalization;
using System.Windows;

namespace CDatos
{
    public class ProductoD
    {
        ConexionDB conexion = new ConexionDB();

        public DataTable SelectData(string query = "")
        {
            string sql = $"SELECT * FROM Producto " +
                $"WHERE id LIKE '%{query}%' " +
                $"OR descripcion LIKE '%{query}%' ";

            if (query.Length == 0)
            {
                sql += "ORDER BY id DESC";
            }

            return conexion.Select(sql).Tables[0];
        }

        public DataRow SelectOneData(int id)
        {
            string sql = $"SELECT * FROM Producto WHERE id={id}";
            return conexion.Select(sql).Tables[0].Rows[0];
        }

        public void InsertData(string descripcion, string unidad, decimal precio, decimal stock)
        {
            string newDescripcion = descripcion.Replace("'", "");
            string newPrecio = precio.ToString(new CultureInfo("en-US"));
            string newStock = stock.ToString(new CultureInfo("en-US"));

            string sql = $"INSERT INTO Producto VALUES('{newDescripcion}', '{unidad}', {newPrecio}, {newStock})";
            conexion.InsertOrUpdate(sql);

        }

        public void UpdateData(int id, string descripcion, string unidad, decimal precio, decimal stock)
        {
            string newPrecio = Convert.ToString(precio).Replace(',', '.');
            string newStock = Convert.ToString(stock).Replace(',', '.');

            string query = $"UPDATE Producto SET descripcion='{descripcion}', unidad='{unidad}', precio={newPrecio}, stock={newStock} " +
                $"WHERE id={id};";
            conexion.InsertOrUpdate(query);
        }

        public void UpdateStock(int id, decimal stock)
        {
            string newStock = stock.ToString(new CultureInfo("en-US"));

            string sql = $"UPDATE Producto SET stock={newStock} WHERE id={id}";
            MessageBox.Show(sql);
            conexion.InsertOrUpdate(sql);
        }
    }
}
