using System.Data;

namespace PortaldeCompras.Domain.Interfaces
{
    public interface IDataSaver
    {
        int Save(DataTable data, string path);
    }
}
