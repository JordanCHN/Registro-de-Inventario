using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Registro_de_Inventario
{
    public class Metodos
    {
        double cantidad, precio, pago, total;

        public double Cantidad { get => cantidad; set => cantidad = value; }
        public double Precio { get => precio; set => precio = value; }
        public double Pago1 { get => pago; set => pago = value; }
        public double Total1 { get => total; set => total = value; }

        public Metodos(double cantidad, double precio, double pago, double total)
        {
            this.cantidad = cantidad;
            this.precio = precio;
            this.pago = pago;
            this.total = total;
        }

        public Metodos()
        { }

        public double Total(double cantidad, double precio)
        {
            return cantidad * precio;
        }

        public double Pago(double pago)
        {
            return pago;
        }

        public double Pago(double total, double pago)
        {
            return pago - total;
        }

        public double CantidadNueva(double CantidadA, double CantidadN)
        {
            return CantidadA - CantidadN;
        }
    }
}
