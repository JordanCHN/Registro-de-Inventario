using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Registro_de_Inventario
{
    public partial class Form1 : Form
    { 
        public Form1()
        {
            InitializeComponent();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try 
            {
                string producto, codigo;
                double cantidad, precio;

                producto = Interaction.InputBox("Nombre del Producto");
                codigo = Interaction.InputBox("Código del Producto");
                if (producto == "" || codigo == "")
                {
                    MessageBox.Show("No puede ingresar valores en blancos", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                        
                    cantidad = Convert.ToDouble(Interaction.InputBox("Cantidad del Producto"));
                    precio = Convert.ToDouble(Interaction.InputBox("Precio del Producto"));

                    string[] datos = new string[4];

                    datos[0] = producto;
                    datos[1] = codigo;
                    datos[2] = Convert.ToString(cantidad);
                    datos[3] = Convert.ToString(precio);

                            dgvDatos.Rows.Add(datos);
                }
            } 
            catch 
            {
                MessageBox.Show("No puede ingresar valores en blancos"); 
            }
        }

        private void dgvDatos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtP.Text = dgvDatos.Rows[e.RowIndex].Cells["Producto"].Value.ToString();
            txtC.Text = dgvDatos.Rows[e.RowIndex].Cells["Cantidad"].Value.ToString();
            txtMoney.Text = dgvDatos.Rows[e.RowIndex].Cells["Precio"].Value.ToString();
            txtProducto.Text = dgvDatos.Rows[e.RowIndex].Cells["Producto"].Value.ToString();
            txtPrecio.Text = dgvDatos.Rows[e.RowIndex].Cells["Precio"].Value.ToString();

            txtNombre.Enabled = true;
            txtApellido.Enabled = true;
            txtCantidad.Enabled = true;
            cbPago.Enabled = true;
            dgvFactura.Enabled = true;
            dtpFecha.Visible = true;
        }

        private void btnAgrega_Click(object sender, EventArgs e)
        {
            Metodos metodos = new Metodos();

            string fecha, nombre, apellido, producto, pago, total;
            double cantidad, precio, cambio;

            fecha = dtpFecha.Text;
            nombre = txtNombre.Text;
            apellido = txtApellido.Text;
            producto = txtProducto.Text;
            cantidad = Convert.ToDouble(txtCantidad.Text);
            
            precio = Convert.ToDouble(txtPrecio.Text);
            total = txtTotal.Text;
            pago = cbPago.Text;
            cambio = Convert.ToDouble(txtCambio.Text);

            string[] factura = new string[9];

            factura[0] = fecha;
            factura[1] = nombre;
            factura[2] = apellido;
            factura[3] = producto;
            factura[4] = Convert.ToString(cantidad);
            factura[5] = Convert.ToString(precio);
            factura[6] = total;
            factura[7] = Convert.ToString(pago);
            factura[8] = Convert.ToString(cambio);

            dgvFactura.Rows.Add(factura);

            DataGridViewRow row = dgvDatos.CurrentRow;

            if (row != null)
            {
                row.Cells["Cantidad"].Value = txtC.Text;
            }

            txtNombre.Enabled = true;
            txtApellido.Enabled = true;
            txtCantidad.Enabled = true;
            cbPago.Enabled = true;
            dgvFactura.Enabled = true;
            dtpFecha.Visible = false;
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            if (dgvFactura.Rows.Count > 0)

            {

                SaveFileDialog save = new SaveFileDialog();

                save.Filter = "PDF (*.pdf)|*.pdf";

                save.FileName = "Result.pdf";

                bool ErrorMessage = false;

                if (save.ShowDialog() == DialogResult.OK)

                {

                    if (File.Exists(save.FileName))

                    {

                        try

                        {

                            File.Delete(save.FileName);

                        }

                        catch (Exception ex)

                        {

                            ErrorMessage = true;

                            MessageBox.Show("Unable to wride data in disk" + ex.Message);

                        }

                    }

                    if (!ErrorMessage)

                    {

                        try

                        {

                            PdfPTable pTable = new PdfPTable(dgvFactura.Columns.Count);

                            pTable.DefaultCell.Padding = 2;

                            pTable.WidthPercentage = 100;

                            pTable.HorizontalAlignment = Element.ALIGN_LEFT;

                            foreach (DataGridViewColumn col in dgvFactura.Columns)

                            {

                                PdfPCell pCell = new PdfPCell(new Phrase(col.HeaderText));

                                pTable.AddCell(pCell);

                            }

                            foreach (DataGridViewRow viewRow in dgvFactura.Rows)

                            {

                                foreach (DataGridViewCell dcell in viewRow.Cells)

                                {

                                    pTable.AddCell(dcell.Value.ToString());

                                }

                            }

                            using (FileStream fileStream = new FileStream(save.FileName, FileMode.Create))

                            {

                                Document document = new Document(PageSize.A4, 8f, 16f, 16f, 8f);

                                PdfWriter.GetInstance(document, fileStream);

                                document.Open();

                                document.Add(pTable);

                                document.Close();

                                fileStream.Close();

                            }

                            MessageBox.Show("Data Export Successfully", "info");

                        }

                        catch (Exception ex)

                        {

                            MessageBox.Show("Error while exporting Data" + ex.Message);

                        }

                    }

                }

            }

            else

            {

                MessageBox.Show("No Record Found", "Info");

            }

        }

        private void cbPago_SelectedIndexChanged(object sender, EventArgs e)
        {
            Metodos metodos = new Metodos();

            double total;

            if (cbPago.SelectedIndex == 0)
            {
                txtPago.Enabled = true;
                txtPago.Text = "";
            }
            else
            {
                txtPago.Enabled = false;
                total = metodos.Pago(Convert.ToDouble(txtTotal.Text));
                txtPago.Text = Convert.ToString(total);
                txtCambio.Text = "0";
                btnAgrega.Enabled = true;
                btnImprimir.Enabled = true;
            }
        }
        private void btnGuardar_Click(object sender, EventArgs e)
        {

        }

        private void txtPago_KeyDown_1(object sender, KeyEventArgs e)
        {
            Metodos metodos = new Metodos();

            double pago, total;

            if (txtPago.Text == "")
            {
                txtCambio.Text = "0";
            }
            else if (e.KeyCode == Keys.Enter)
            {
                pago = Convert.ToDouble(txtPago.Text);
                total = Convert.ToDouble(txtTotal.Text);
                if (pago < total)
                {
                    MessageBox.Show("El pago es insuficiente");
                }
                else
                {
                    double cambio;

                    total = Convert.ToDouble(txtTotal.Text);
                    pago = Convert.ToDouble(txtPago.Text);

                    cambio = metodos.Pago(total, pago);

                    txtCambio.Text = Convert.ToString(cambio);

                    btnAgrega.Enabled = true;
                    btnImprimir.Enabled = true;
                }
            }
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            txtP.Enabled = false;
            txtC.Enabled = false;
            txtMoney.Enabled = false;
            txtPago.Enabled = false;
            dgvDatos.AllowUserToAddRows = false;
            dgvFactura.AllowUserToAddRows = false;
            dtpFecha.Visible = false;
        }

        private void txtCantidad_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (txtCantidad.Text == "")
            {
                txtTotal.Text = "0";
            }
            else if (e.KeyCode == Keys.Enter)
            {
                if (txtCantidad.Text == "")
                {
                    MessageBox.Show("Debe ingresar una cantidad", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (txtCantidad.Text == "0")
                {
                    MessageBox.Show("Debe seleccionar una cantidad válida", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    Metodos metodos = new Metodos();

                    double cantidad, precio, total, cantidadn, nueva;
                    cantidadn = Convert.ToDouble(txtC.Text);
                    cantidad = Convert.ToDouble(txtCantidad.Text);
                    precio = Convert.ToDouble(txtPrecio.Text);
                    total = metodos.Total(cantidad, precio);
                    txtTotal.Text = total.ToString();

                    nueva = metodos.CantidadNueva(cantidadn, cantidad);
                    txtC.Text = Convert.ToString(nueva);
                }
            }
        }
    }
}
