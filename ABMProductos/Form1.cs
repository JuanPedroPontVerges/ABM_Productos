using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ABMProductos  
{
    public partial class Form1 : Form
    {
        const int tam = 10;
        int c;
        bool nuevo;
        Producto[] arregloP = new Producto[tam];
        AccesoDatos oDatos = new AccesoDatos(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source= C:\Users\jppon\Desktop\Programacion UTN\ABMProductos\DBFProducto.mdb");
        public Form1()
        {
            InitializeComponent();
            nuevo = false;
            c = 0;
            for (int i = 0; i < tam; i++)
            {
                arregloP[i] = null;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.cargarCombo(cboMarca, "Marca");
            this.cargarLista("Producto");
            this.habilitar(false);
        }

        private void cargarLista(string nombreTabla)
        {
            c = 0;
            lstProducto.Items.Clear();
            oDatos.leerTabla("Producto");
            while (oDatos.pLector.Read())
            {
                Producto p = new Producto();
                if(!oDatos.pLector.IsDBNull(0))
                    p.pCodigo = oDatos.pLector.GetInt32(0);
                if (!oDatos.pLector.IsDBNull(1))
                    p.pDetalle = oDatos.pLector["detalle"].ToString();
                if (!oDatos.pLector.IsDBNull(2))
                    p.pTipo = oDatos.pLector.GetInt32(2);
                if (!oDatos.pLector.IsDBNull(3))
                    p.pMarca = oDatos.pLector.GetInt32(3);
                if (!oDatos.pLector.IsDBNull(4))
                    p.pPrecio = oDatos.pLector.GetDouble(4);
                if (!oDatos.pLector.IsDBNull(5))
                    p.pFecha = oDatos.pLector.GetDateTime(5);
                arregloP[c] = p;
                c++;
            }
            oDatos.desconectar();
            for (int i = 0; i < c; i++)
            {
                lstProducto.Items.Add(arregloP[i].ToString());
            }
            lstProducto.SelectedIndex = 0;
        }

        private void cargarCombo(ComboBox combo, string nombreTabla)
        {
            DataTable tabla = new DataTable();
            tabla = oDatos.consultarTabla(nombreTabla);
            combo.DataSource = tabla;
            //Esto es para asociar el id a cada producto
            combo.ValueMember = tabla.Columns[0].ColumnName;
            combo.DisplayMember = tabla.Columns[1].ColumnName;
            //Otra forma de poner la cadena de conexion
            //oDatos.pCadenaConexion = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\Users\alumno\Desktop\ABMProductos\DBFProducto.mdb";
            combo.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            nuevo = true;
            this.habilitar(true);
            this.limpiar();
            txtCodigo.Focus();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            this.habilitar(true);
            txtCodigo.Enabled = false;
            txtDetalle.Focus();
        }

        private void btnBorrar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Esta seguro de eliminar este producto?", "Eliminando", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                string consultaSQL = "delete from producto where codigo=" + arregloP[lstProducto.SelectedIndex].pCodigo;
                oDatos.actualizarBd(consultaSQL);
                cargarLista("Producto");
            }
        }

        private void btnGrabar_Click(object sender, EventArgs e)
        {
            /*--Muy importante validar --*/
            string consultaSQL = "";
            Producto p = new Producto();
            p.pCodigo = Convert.ToInt32(txtCodigo.Text);
            p.pDetalle = txtDetalle.Text;
            p.pMarca = Convert.ToInt32(cboMarca.SelectedValue);
            if (rbtNotebook.Checked)
                p.pTipo = 1;
            else
                p.pTipo = 2;
            p.pPrecio = Convert.ToDouble(txtPrecio.Text);
            p.pFecha = dtpFecha.Value;
            this.habilitar(false);
            if (nuevo)
            {
                if (!existe(p.pCodigo))
                {
                    consultaSQL = "insert into producto (codigo,detalle,tipo,marca,precio,fecha) values(" +p.pCodigo+",'" + p.pDetalle+"',"+p.pTipo+","+p.pMarca+","+p.pPrecio+",'"+p.pFecha+"')";
                    oDatos.actualizarBd(consultaSQL);
                    cargarLista("Producto");
                }
                else
                    MessageBox.Show("El codigo que desea ingresar ya existe");
            }
            else
            {
                consultaSQL="update producto set detalle ='" + p.pDetalle+"'," + "tipo="+p.pTipo+","+"marca="+p.pMarca+","+"precio="+p.pPrecio+","+"fecha='"+p.pFecha+"' "+"where codigo=" + p.pCodigo;
                oDatos.actualizarBd(consultaSQL);
                cargarLista("Producto");
            }
            
            nuevo = false;
        }

        private bool existe(int codigo)
        {
            bool resultado = false;
            for (int i = 0; i < c; i++)
            {
                if (arregloP[i].pCodigo == codigo)
                {
                    resultado = true;
                    return resultado;
                }
            }
            return resultado;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            nuevo = false;
            this.limpiar();
            this.habilitar(false);
            this.cargarCampos(lstProducto.SelectedIndex);
        }

        private void habilitar(bool x)
        {
            btnNuevo.Enabled = !x;
            btnEditar.Enabled = !x;
            btnBorrar.Enabled = !x;
            btnSalir.Enabled = !x;

            txtCodigo.Enabled = x;
            txtDetalle.Enabled = x;
            txtPrecio.Enabled = x;
            cboMarca.Enabled = x;
            rbtNetbook.Enabled = x;
            rbtNotebook.Enabled = x;
            dtpFecha.Enabled = x;
            btnGrabar.Enabled = x;
            btnCancelar.Enabled = x;
        }

        private void limpiar()
        {
            txtCodigo.Clear();
            txtDetalle.Clear();
            txtPrecio.Clear();
            cboMarca.SelectedIndex = -1;
            rbtNetbook.Checked = false;
            rbtNotebook.Checked = false;
            dtpFecha.Value = DateTime.Today;
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Cerrar el Form
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("¿Esta seguro que desea abandonar este formulario?","Salir", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                e.Cancel = false;
            else
                e.Cancel = true;
        }

        private void lstProducto_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargarCampos(lstProducto.SelectedIndex);
        }

        private void cargarCampos(int posicion)
        {
            txtCodigo.Text = arregloP[posicion].pCodigo.ToString();
            txtDetalle.Text = arregloP[posicion].pDetalle;
            cboMarca.SelectedValue = arregloP[posicion].pMarca;
            if (arregloP[posicion].pTipo==1)
            {
                rbtNotebook.Checked = true;
            } else
            {
                rbtNetbook.Checked = true;
            }
            txtPrecio.Text = arregloP[posicion].pPrecio.ToString();
            dtpFecha.Value = arregloP[posicion].pFecha;
        }

    }
}
