using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace CDatos
{
    public class ConexionDB
    {
        private SqlConnection connection;
        private SqlDataAdapter da;
        private SqlCommand comm;

        private string equipo = "DESKTOP-HA2D645";
        private string database = "Tarea4";

        public void Conectar()
        {
            try
            {
                connection = new SqlConnection($"Data Source={equipo};Initial Catalog={database};Integrated Security=True");
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error en DB (Conectar): \n{e.Message}");
            }
        }

        public DataSet Select(string query)
        {
            Conectar();

            DataSet ds = new DataSet();

            try
            {
                connection.Open();

                da = new SqlDataAdapter(query, connection);
                da.Fill(ds);

                connection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error en DB (Select): \n{e.Message}");
            }


            return ds;

        }

        public void InsertOrUpdate(string query)
        {
            try
            {
                Conectar();

                comm = new SqlCommand(query, connection);

                connection.Open();
                comm.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error en DB (InsertOrUpdate): \n{e.Message}");
            }
        }
    }
}
