using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mego.BaseClasses;
using System.Threading;

namespace Mego.Controllers
{
    public class SearchController : Controller
    {
        SearchSystem ExternalA;
        SearchSystem ExternalB;
        SearchSystem ExternalC;
        SearchSystem ExternalD;

        

        // GET: SearchController/Details/5
        public ActionResult Search(int wait, int randomMin, int randomMax)
        {            
            ExternalA = new SearchSystem(randomMin, randomMax);
            ExternalB = new SearchSystem(randomMin, randomMax);
            ExternalC = new SearchSystem(randomMin, randomMax);
            ExternalD = new SearchSystem(randomMin, randomMax);

            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            
            Task<string> A = ExternalA.RequestAsync(wait, randomMin, randomMax, cancelTokenSource);
            Task<string> B = ExternalB.RequestAsync(wait, randomMin, randomMax, cancelTokenSource);
            Task<string> C = ExternalC.RequestAsync(wait, randomMin, randomMax, cancelTokenSource);
            Task<string> D = ExternalD.RequestAsync(wait, randomMin, randomMax, cancelTokenSource);

            Task.WhenAll(new Task[] { A, B, C });

            string AResult = A.Result;
            string BResult = B.Result;
            string CResult = C.Result;

            if (CResult == "OK")
            {
                D.Start();
            }
            return View();
        }

        
    }
}
