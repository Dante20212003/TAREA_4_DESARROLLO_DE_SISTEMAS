using System.Data;

namespace CDatos
{
    public class ClienteD
    {
        ConexionDB conexion = new ConexionDB();

        public DataTable SelectData()
        {
            string sql = $"SELECT * FROM Cliente ORDER BY id DESC";
            return conexion.Select(sql).Tables[0];
        }

        public void InsertData(string nombre, string telefono, string nit)
        {
            string sql = $"INSERT INTO Cliente VALUES('{nombre}', '{telefono}', '{nit}')";
            conexion.InsertOrUpdate(sql);
        }

        public void UpdateData(int id, string nombre, string telefono, string nit)
        {
            string query = $"UPDATE Cliente SET nombre='{nombre}', telefono='{telefono}', nit='{nit}' " +
                $"WHERE id={id};";
            conexion.InsertOrUpdate(query);
        }
    }
}
