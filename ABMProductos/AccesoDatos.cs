using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;

namespace ABMProductos
{
    class AccesoDatos
    {
        OleDbConnection conexion;
        OleDbCommand comando;
        OleDbDataReader lector;
        DataTable tabla;
        string cadenaConexion;

        public OleDbConnection pConexion {
            set { conexion = value; }
            get { return conexion; }
        }

        public OleDbCommand pComando {
            set { comando = value; }
            get { return comando; }
        }

        public OleDbDataReader pLector {
            set { lector = value; }
            get { return lector; }
        }

        public DataTable pTabla {
            set { tabla = value; }
            get { return tabla; }
        }

        public string pCadenaConexion {
            set { cadenaConexion = value; }
            get { return cadenaConexion; }
        }

        public AccesoDatos()
        {
            conexion = new OleDbConnection();
            comando = new OleDbCommand();
            tabla = new DataTable();
            lector = null;
            cadenaConexion = "";
        }

        public AccesoDatos(string cadena)
        {
            conexion = new OleDbConnection();
            comando = new OleDbCommand();
            tabla = new DataTable();
            lector = null;
            cadenaConexion = cadena;
        }

        private void conectar()
        {
            conexion.ConnectionString = cadenaConexion;
            conexion.Open();
            comando.Connection = conexion;
            comando.CommandType = CommandType.Text;
        }

        public void desconectar()
        {
            conexion.Close();
            conexion.Dispose();
        }

        public DataTable consultarTabla(string nombreTabla)
        {
            this.conectar();
            this.comando.CommandText = "select * from " + nombreTabla;
            this.tabla.Load(comando.ExecuteReader());
            this.desconectar();
            return this.tabla;
        }

        public DataTable consultarSQL(string consulta)
        {
            this.conectar();
            this.comando.CommandText = consulta;
            this.tabla.Load(comando.ExecuteReader());
            this.desconectar();
            return this.tabla;
        }

        public void leerTabla(string nombreTabla)
        {
            conectar();
            comando.CommandText = "select * from " + nombreTabla;
            lector = comando.ExecuteReader();
        }

        public void actualizarBd(string consultaSQL)
        {
            conectar();
            comando.CommandText = consultaSQL;
            comando.ExecuteNonQuery();
            desconectar();
        }
    }
}
