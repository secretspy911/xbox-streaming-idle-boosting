using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace XboxStreamingIdleBoosting.Games
{
    abstract class PCGame : Game
    {
        public PCGame(bool logInput, CancellationToken cancellationToken) : base(logInput, cancellationToken) 
        {
        }
    }
}
