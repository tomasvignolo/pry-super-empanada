using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;
using System.Windows.Forms.DataVisualization.Charting;

namespace SUPER_EMPANADA
{
    public class clsVariedades
    {

        #region OleDb/CadenaConexion
        private OleDbConnection cnx = new OleDbConnection();
        private OleDbCommand cmd = new OleDbCommand();
        private OleDbDataAdapter adap = new OleDbDataAdapter();


        //Conectarse a la base de datos mediante la cadena de conexion y declarar la variable
        private string CadenaConexion = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=SUPER_EMPANADA.mdb";
        private string vSQL = "";
        #endregion


        public void MostrarNumerosDeTicketPorFecha(DataGridView dgv, DateTime fechaInicio, DateTime fechaFin)
        {
            string consulta = "SELECT Tickets.ticket, Tickets.fecha, Variedades.nombre " +
                      "FROM (Tickets INNER JOIN TicketsVariedades ON Tickets.ticket = TicketsVariedades.ticket) " +
                      "INNER JOIN Variedades ON TicketsVariedades.variedad = Variedades.variedad " +
                      "WHERE Tickets.fecha >= @FechaInicio AND Tickets.fecha <= @FechaFin";

            using (OleDbConnection connection = new OleDbConnection(CadenaConexion))
            {
                OleDbCommand command = new OleDbCommand(consulta, connection);
                command.Parameters.AddWithValue("@FechaInicio", fechaInicio);
                command.Parameters.AddWithValue("@FechaFin", fechaFin);

                connection.Open();

                using (OleDbDataReader reader = command.ExecuteReader())
                {
                    dgv.Rows.Clear();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string numeroTicket = reader["ticket"].ToString();
                            string nombreVariedad = reader["nombre"].ToString();
                            DateTime fechaTicket = Convert.ToDateTime(reader["fecha"]);
                            dgv.Rows.Add(numeroTicket, nombreVariedad, fechaTicket.ToShortDateString());


                        }

                    }
                    else
                    {
                        MessageBox.Show("No se encontraron registros para las fechas seleccionadas.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }


                connection.Close();
            }

        }

        public void CargarGraficoCantidadVendida(string variedad, DateTime fecha, Chart grafico, DateTime fechai, DateTime fechaf)
        {
            // Limpia los datos existentes en el gráfico
            grafico.Series.Clear();
            grafico.Titles.Clear();

            // Consulta para obtener la cantidad vendida de la variedad en el rango de fechas especificado
            string consulta = "SELECT Tickets.fecha, TicketsVariedades.cantidad " +
                  "FROM (Tickets INNER JOIN TicketsVariedades ON Tickets.ticket = TicketsVariedades.ticket) " +
                  "INNER JOIN Variedades ON TicketsVariedades.variedad = Variedades.variedad " +
                  "WHERE Variedades.nombre = @Variedad " +
                  "AND Tickets.fecha >= @FechaInicio " +
                  "AND Tickets.fecha <= @FechaFin";

            using (OleDbConnection connection = new OleDbConnection(CadenaConexion))
            {
                OleDbCommand command = new OleDbCommand(consulta, connection);
                command.Parameters.AddWithValue("@Variedad", variedad);
                command.Parameters.AddWithValue("@FechaInicio", fechai);
                command.Parameters.AddWithValue("@FechaFin", fechaf);

                connection.Open();

                using (OleDbDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        // Agrega una nueva serie al gráfico
                        grafico.Series.Add("CantidadVendida");
                        grafico.Series["CantidadVendida"].ChartType = SeriesChartType.Bar;

                        while (reader.Read())
                        {
                            DateTime fechaVenta = Convert.ToDateTime(reader["fecha"]);
                            int cantidadVendida = Convert.ToInt32(reader["cantidad"]);

                            // Agrega los puntos de datos al gráfico
                            grafico.Series["CantidadVendida"].Points.AddXY(fechaVenta.ToShortDateString(), cantidadVendida);
                        }
                    }
                    else
                    {
                        MessageBox.Show("No se encontraron registros de ventas para la variedad y fecha seleccionadas.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                connection.Close();
            }

            // Configura el título del gráfico con el nombre de la variedad seleccionada
            grafico.Titles.Add(variedad);

            // Configura los ejes X e Y del gráfico
            grafico.ChartAreas[0].AxisX.Title = "Fechas";
            grafico.ChartAreas[0].AxisY.Title = "Cantidad Total";
        }


        #region 1
        //    public void CargarGraficoCantidadVendida(string variedad, DateTime fecha, Chart grafico, DateTime fechai, DateTime fechaf)
        //    {
        //        // Limpia los datos existentes en el gráfico
        //        grafico.Series.Clear();

        //        // Consulta para obtener la cantidad vendida de la variedad en la fecha especificada
        //        string consulta = "SELECT Tickets.fecha, TicketsVariedades.cantidad " +
        //             "FROM (Tickets INNER JOIN TicketsVariedades ON Tickets.ticket = TicketsVariedades.ticket) " +
        //             "INNER JOIN Variedades ON TicketsVariedades.variedad = Variedades.variedad " +
        //             "WHERE Variedades.nombre = @Variedad " +
        //             "AND Tickets.fecha >= @FechaInicio " +
        //             "AND Tickets.fecha <= @FechaFin";

        //        using (OleDbConnection connection = new OleDbConnection(CadenaConexion))
        //        {
        //            OleDbCommand command = new OleDbCommand(consulta, connection);
        //            command.Parameters.AddWithValue("@Variedad", variedad);
        //            command.Parameters.AddWithValue("@Fecha", fecha);
        //            command.Parameters.AddWithValue("@FechaInicio", fechai);
        //            command.Parameters.AddWithValue("@FechaFin", fechaf);

        //            connection.Open();

        //            using (OleDbDataReader reader = command.ExecuteReader())
        //            {
        //                if (reader.HasRows)
        //                {
        //                    // Agrega una nueva serie al gráfico
        //                    grafico.Series.Add("CantidadVendida");

        //                    while (reader.Read())
        //                    {
        //                        DateTime fechaVenta = Convert.ToDateTime(reader["fecha"]);
        //                        int cantidadVendida = Convert.ToInt32(reader["cantidad"]);

        //                        // Agrega los puntos de datos al gráfico
        //                        grafico.Series["CantidadVendida"].Points.AddXY(fechaVenta.ToShortDateString(), cantidadVendida);
        //                    }
        //                }
        //                else
        //                {
        //                    MessageBox.Show("No se encontraron registros de ventas para la variedad y fecha seleccionadas.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //                }
        //            }

        //            connection.Close();
        //        }
        //    }



        //}
        #endregion

    }
}

