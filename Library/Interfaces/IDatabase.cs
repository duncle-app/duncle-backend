﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library.Interfaces
{
    public interface IDatabase
    {
        public Task<List<Model.Library>> getLibraries();
        public Task updateLibrary(Model.Library library);
        public Task<Model.Library> createLibrary(Model.Library library);
    }
}