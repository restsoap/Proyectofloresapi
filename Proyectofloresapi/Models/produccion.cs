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

    public partial class produccion
    {
        public int Idfertirriego { get; set; }
        public int valvula { get; set; }
        public int numerobloque { get; set; }
        public int numerogoteros { get; set; }
        public int idfinca { get; set; }
        public System.DateTime fechacreado { get; set; }
    
        public virtual finca finca { get; set; }
    }
}
