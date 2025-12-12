using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoGameCatalog.Models
{
    public class Juego
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Genero { get; set; }
        public decimal Nota { get; set; }
        public int PlataformaId { get; set; }

        public override string ToString()
        {
            return Titulo;
        }
    }
}