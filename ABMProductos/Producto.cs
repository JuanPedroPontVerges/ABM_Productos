using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMProductos
{
    class Producto
    {
        int codigo,tipo,marca;
        string detalle;
        double precio;
        DateTime fecha;

        public int pCodigo {
            set { codigo = value; }
            get { return codigo; }
        }

        public int pTipo {
            set { tipo = value; }
            get { return tipo; }
        }

        public int pMarca {
            set { marca = value; }
            get { return marca; }
        }

        public string pDetalle {
            set { detalle = value; }
            get { return detalle; }
        }

        public double pPrecio {
            set { precio = value; }
            get { return precio; }
        }

        public DateTime pFecha {
            set { fecha = value; }
            get { return fecha; }
        }

        public Producto()
        {
            codigo = 0;
            tipo = 0;
            marca = 0;
            detalle = "";
            precio = 0;
            fecha = DateTime.Today;
        }

        public Producto(int codigo, int tipo, int marca, string detalle, double precio, DateTime fecha)
        {
            this.codigo = codigo;
            this.tipo = tipo;
            this.marca = marca;
            this.detalle = detalle;
            this.precio = precio;
            this.fecha = fecha;
        }

        public override string ToString()
        {
            return codigo + " - " + detalle;
        }

    }
}
