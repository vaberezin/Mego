using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mego.Models
{
    public class SearchEngineModel
    {
        public SearchEngineModel(string _name, string _result, int _performance)
        {
            SearchEngineName = _name;
            SearchResult = _result;
            Performance = _performance;
        }
        public string SearchEngineName { get; private set; }
        public string SearchResult { get; private set; }
        public int Performance { get; private set; }
    }
}
