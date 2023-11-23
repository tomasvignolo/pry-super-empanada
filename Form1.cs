using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SUPER_EMPANADA
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        clsVariedades clsVariedades = new clsVariedades();
        string numeroTicket;
        string nombreVariedad;
        DateTime fecha;

        private void button1_Click(object sender, EventArgs e)
        {
          
            DateTime fechaInicio = dateTimePicker1.Value;
            DateTime fechaFin = dateTimePicker2.Value;
            fechaInicio = fechaInicio.Date;
            fechaFin = fechaFin.Date.AddDays(1).AddSeconds(-1);

            clsVariedades.MostrarNumerosDeTicketPorFecha(dataGridView1, fechaInicio, fechaFin);
        }

        public void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0) // Verificar que se haya hecho clic en una fila válida
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // Obtener los datos de la celda seleccionada
                numeroTicket = row.Cells["Ticket"].Value.ToString();
                nombreVariedad = row.Cells["Variedad"].Value.ToString();

                if (DateTime.TryParse(row.Cells["Fecha"].Value.ToString(), out DateTime fechaSeleccionada))
                {
                    fecha = fechaSeleccionada.Date;
                    fecha = fecha.Date.AddDays(1).AddSeconds(-1);
                }
                else
                {
                    // Si la conversión de la fecha falla, mostrar un mensaje de error o tomar una acción adecuada
                    MessageBox.Show("La fecha seleccionada no es válida.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void tabControl1_TabIndexChanged(object sender, EventArgs e)
        {
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabPage2) // Verificar si la pestaña seleccionada es la segunda pestaña
            {
                // Obtener la variedad seleccionada en el DataGridView
                 nombreVariedad = dataGridView1.CurrentRow.Cells["Variedad"].Value.ToString();

                // Obtener la fecha seleccionada en el DateTimePicker
                string fechaP = dataGridView1.CurrentRow.Cells["Fecha"].Value.ToString();
                fecha = DateTime.Parse(fechaP);

                // Cargar el gráfico con la cantidad vendida de la variedad en la fecha seleccionada
               DateTime fechai = dateTimePicker1.Value;
               DateTime fechaf = dateTimePicker2.Value;
               fechai = fechai.Date.AddDays(1).AddSeconds(-1);
               fechaf = fechaf.Date.AddDays(1).AddSeconds(-1);
               fecha = fecha.Date.AddDays(1).AddSeconds(-1);


                clsVariedades.CargarGraficoCantidadVendida(nombreVariedad, fecha, chart1,fechai, fechaf);
            }
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }
    }
}
