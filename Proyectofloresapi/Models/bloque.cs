//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Proyectofloresapi.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class bloque
    {
        public int idbloque { get; set; }
        public int numerobloque { get; set; }
        public double presupuestadoaño { get; set; }
        public double llevamosaño { get; set; }
        public double diferenciaaño { get; set; }
        public double presupuestadomes { get; set; }
        public double llevamosmes { get; set; }
        public double diferenciames { get; set; }
        public int cedula { get; set; }
        public int idinvernadero { get; set; }
        public int idfinca { get; set; }
        public System.DateTime fechacreado { get; set; }
    
        public virtual usuario usuario { get; set; }
        public virtual finca finca { get; set; }
        public virtual invernadero invernadero { get; set; }
    }
}
