using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library.Control
{
    public interface IDatabase
    {
        public Task<List<Model.Library>> getLibraries();
        public Task updateLibrary(Model.Library library);
        public Task createLibrary(Model.Library library);
    }
}