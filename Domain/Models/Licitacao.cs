using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortaldeCompras.Domain.Models
{
    public class Licitacao
    {
        public string TipoLicitacao { get; set; }
        public int NumLicitacao { get; set; }
        public int AnoLicitacao { get; set; }

        public override string ToString()
        {
            return $"{TipoLicitacao} nº {NumLicitacao}/{AnoLicitacao}";
        }
    }
}
