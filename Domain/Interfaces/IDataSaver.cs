using System.Data;

namespace PortaldeCompras.Domain.Interfaces
{
    public interface IDataSaver
    {
        void Save(DataTable data, string path);
    }
}
