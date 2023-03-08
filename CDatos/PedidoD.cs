using System;
using System.Data;
using System.Globalization;
using System.Windows;

namespace CDatos
{
    public class PedidoD
    {
        ConexionDB conexion = new ConexionDB();

        public DataTable SelectData(bool today = false)
        {
            string sql = $"SELECT p.id, p.cliente_id, c.nombre cliente, p.producto_id, pr.descripcion producto, p.cantidad, p.monto, p.fecha " +
                $"FROM Pedido p, Cliente c, Producto pr " +
                $"WHERE p.cliente_id = c.id AND p.producto_id = pr.id ";
            if (today)
            {
                sql += "AND CAST(fecha AS DATE) = CAST(GETDATE() AS DATE)";
            }
            return conexion.Select(sql).Tables[0];
        }

        public void InsertData(int cliente_id, int producto_id, decimal cantidad, decimal monto)
        {
            string newCantidad = cantidad.ToString(new CultureInfo("en-US"));
            string newMonto = monto.ToString(new CultureInfo("en-US"));

            string sql = $"INSERT INTO Pedido(cliente_id, producto_id, cantidad, monto) VALUES({cliente_id}, {producto_id}, {newCantidad}, {newMonto})";
            conexion.InsertOrUpdate(sql);
        }

        public void UpdateData(int id, int cliente_id, int producto_id, decimal cantidad, decimal monto, string fecha)
        {
            string newCantidad = cantidad.ToString(new CultureInfo("en-US"));
            string newMonto = monto.ToString(new CultureInfo("en-US"));
            var x = Convert.ToDateTime(fecha);
            MessageBox.Show(x.ToString());

            string sql = $"UPDATE Pedido SET cliente_id={cliente_id}, producto_id={producto_id}, cantidad={newCantidad}, monto={newMonto}, fecha='{fecha}' " +
                $"WHERE id={id};";
            conexion.InsertOrUpdate(sql);
        }
    }
}
