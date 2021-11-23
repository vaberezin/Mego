using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mego.Models;

namespace Mego.BaseClasses
{
    public class SearchSystem
    {
        public SearchSystem()
        {

        }
        public SearchSystem(int minDelay, int maxDelay)
        {
            MinDelay = minDelay;
            MaxDelay = maxDelay;            
        }

        public int MinDelay { get; private set; }
        public int MaxDelay { get; private set; }
        public async Task<SearchEngineModel> RequestAsync(int wait, int minDelay, int maxDelay, CancellationTokenSource cancelTokenSource)
        {
            int halfDelay = 1 / 2 * (minDelay + maxDelay);
            //Returns random result -> OK or ERROR
            Random rnd = new Random();
            int searchDelayEmul = rnd.Next(minDelay, maxDelay);
            if (searchDelayEmul > wait + 100)
            {
                cancelTokenSource.Cancel();
                return new SearchEngineModel(this.GetType().Name, "TIMEOUT", searchDelayEmul);
            }
            else
            {
                return await Task.Run(() => Result(searchDelayEmul, halfDelay, cancelTokenSource.Token));

            }

        }

        private SearchEngineModel Result(int searchDelayEmul, int halfDelay, CancellationToken cancelToken)
        {
            if (cancelToken.IsCancellationRequested)
            {
                return new SearchEngineModel(this.GetType().Name, "TIMEOUT", searchDelayEmul); //leave searchDelayEmul as it can be useful in future
            }
            Task.Delay(searchDelayEmul);
            if (searchDelayEmul < halfDelay)
            {
                return new SearchEngineModel(this.GetType().Name, "OK", searchDelayEmul);
            }
            else
            {
                return new SearchEngineModel(this.GetType().Name, "ERROR", searchDelayEmul);
            }
        }

    }
}
