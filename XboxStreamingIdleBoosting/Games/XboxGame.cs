using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace XboxStreamingIdleBoosting.Games
{
    abstract class XboxGame : Game
    {
        public XboxGame(CancellationToken cancellationToken) : base(cancellationToken) { }

        protected override string GameWindowName => "Compagnon de la console Xbox";
    }
}
