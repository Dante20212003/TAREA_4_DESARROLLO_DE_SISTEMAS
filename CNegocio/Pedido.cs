using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using Bogus;
using CDatos;

namespace CNegocio
{
    public class Pedido
    {
        private int id;
        private int cliente_id;
        private string cliente;
        private int producto_id;
        private string producto;
        private decimal cantidad;
        private decimal monto;
        private string fecha;

        private PedidoD pedidoD = new PedidoD();

        public int Id { get => id; set => id = value; }
        public int Cliente_id { get => cliente_id; set => cliente_id = value; }
        public string Cliente { get => cliente; set => cliente = value; }
        public int Producto_id { get => producto_id; set => producto_id = value; }
        public string Producto { get => producto; set => producto = value; }
        public decimal Cantidad { get => cantidad; set => cantidad = value; }
        public decimal Monto { get => monto; set => monto = value; }
        public string Fecha { get => fecha; set => fecha = value; }

        public List<Pedido> GetPedidos(bool today = false)
        {
            List<Pedido> listaPedidos = new List<Pedido>();

            foreach (DataRow R in pedidoD.SelectData(today).Rows)
            {
                Pedido pedido = new Pedido
                {
                    Id = int.Parse(R["id"].ToString()),
                    Cliente_id = int.Parse(R["cliente_id"].ToString()),
                    Cliente = R["cliente"].ToString(),
                    Producto_id = int.Parse(R["producto_id"].ToString()),
                    Producto = R["producto"].ToString(),
                    Cantidad = decimal.Parse(R["cantidad"].ToString()),
                    Monto = decimal.Parse(R["monto"].ToString()),
                    Fecha = Convert.ToDateTime(R["fecha"].ToString()).ToString("dddd dd MMMM 'de' yyyy hh:mm", CultureInfo.CreateSpecificCulture("es-ES"))
                };
                listaPedidos.Add(pedido);

            }
            return listaPedidos;
        }

        public void CrearPedido()
        {
            pedidoD.InsertData(Cliente_id, Producto_id, Cantidad, Monto);
        }

        public void ActualizarPedido()
        {
            pedidoD.UpdateData(Id, Cliente_id, Producto_id, Cantidad, Monto, Fecha);
        }
    }
}
