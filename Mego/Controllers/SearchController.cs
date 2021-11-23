using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mego.BaseClasses;
using System.Threading;
using Mego.Models;

namespace Mego.Controllers
{

    [ApiController]    
    [Route("[controller]/[action]")]
    public class SearchDataController : Controller
    {
        DataManagement dataManagement;
        public SearchDataController(DataManagement _data)
        {
            this.dataManagement = _data;
        }
        SearchSystem ExternalA;
        SearchSystem ExternalB;
        SearchSystem ExternalC;
        SearchSystem ExternalD;

        
        [HttpGet]
        // GET: Search/Search?wait=X&randomMin=Y&randomMax=Z
        public ActionResult Search(int wait, int randomMin, int randomMax)
        {            
            ExternalA = new SearchSystem(randomMin, randomMax);
            ExternalB = new SearchSystem(randomMin, randomMax);
            ExternalC = new SearchSystem(randomMin, randomMax);
            ExternalD = new SearchSystem(randomMin, randomMax);

            //Create list of objects and send it to client
            List<SearchEngineModel>  resultList = new List<SearchEngineModel>();

            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            
            Task<SearchEngineModel> A = ExternalA.RequestAsync(wait, randomMin, randomMax, cancelTokenSource);
            Task<SearchEngineModel> B = ExternalB.RequestAsync(wait, randomMin, randomMax, cancelTokenSource);
            Task<SearchEngineModel> C = ExternalC.RequestAsync(wait, randomMin, randomMax, cancelTokenSource);
            Task<SearchEngineModel> D = ExternalD.RequestAsync(wait, randomMin, randomMax, cancelTokenSource);

            Task.WhenAll(new Task[] { A, B, C });

            SearchEngineModel AResult = A.Result;
            SearchEngineModel BResult = B.Result;
            SearchEngineModel CResult = C.Result;
            SearchEngineModel DResult;


            if (CResult.SearchResult == "OK")
            {
                Task.WhenAll(new Task[]{D});
                DResult = D.Result;
                resultList.AddRange(new SearchEngineModel[]{AResult,BResult,CResult,DResult});
            }
            else
            {
                resultList.AddRange(new SearchEngineModel[]{AResult,BResult,CResult});
            }
            
            //Add searchResults to DataManagement object (serves as a warehouse here).
            dataManagement.warehouse.AddRange(resultList);

            return Json(resultList);
        }

        [HttpGet]
        public ActionResult Metrics(){  //Report action
            
            var Result = dataManagement.warehouse.GroupBy(d => (Math.Ceiling(Convert.ToDecimal(d.Performance/1000)),d.SearchEngineName)).ToList(); //test math.ceiling

            return Json(Result);
        }
        
    }
}
