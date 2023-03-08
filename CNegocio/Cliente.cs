using System.Collections.Generic;
using System.Data;
using Bogus;
using CDatos;

namespace CNegocio
{
    public class Cliente
    {
        private int id;
        private string nombre;
        private string telefono;
        private string nit;

        private ClienteD clienteD = new ClienteD();

        public int Id { get => id; set => id = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public string Telefono { get => telefono; set => telefono = value; }
        public string Nit { get => nit; set => nit = value; }

        public List<Cliente> GetClientes()
        {
            List<Cliente> listaClientes = new List<Cliente>();

            foreach (DataRow R in clienteD.SelectData().Rows)
            {
                Cliente cliente = new Cliente
                {
                    Id = int.Parse(R["id"].ToString()),
                    Nombre = R["nombre"].ToString(),
                    Telefono = R["telefono"].ToString(),
                    Nit = R["nit"].ToString()
                };

                listaClientes.Add(cliente);
            }

            return listaClientes;
        }

        public void CrearCliente()
        {
            clienteD.InsertData(Nombre, Telefono, Nit);
        }

        public void ActualizarCliente()
        {
            clienteD.UpdateData(Id, Nombre, Telefono, Nit);
        }

        public void GenerarClientes(int limit)
        {
            Faker faker = new Faker("es_MX");

            for (int i = 0; i < limit; i++)
            {
                Cliente cliente = new Cliente()
                {
                    Nombre = faker.Name.FullName(),
                    telefono = faker.Phone.PhoneNumber("########"),
                    nit = faker.Phone.PhoneNumber("############")
                };

                clienteD.InsertData(cliente.Nombre, cliente.Telefono, cliente.Nit);
            }

        }
    }
}
