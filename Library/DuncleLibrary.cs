using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Library.Interfaces;

namespace Library
{
    public class DuncleLibrary
    {
        private IDatabase db;
        public DuncleLibrary(IDatabase db)
        {
            this.db = db;
        }

        
        public async Task<List<Model.Library>> getLibraries()
        {
            List<Model.Library> libraries = await db.getLibraries();
            // Go out to the 'Database'
            //  
            return libraries;
        }

        public async Task createLibrary(Model.Library library)
        {
            await this.db.createLibrary(library);
        }

        public async Task updateLibrary(Model.Library library)
        {
            await this.db.updateLibrary(library);
        }
    }
}