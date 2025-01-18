using System.Threading;

namespace GameBoosterNS.Games
{
    abstract class PCGame : Game
    {
        public PCGame(CancellationToken cancellationToken) : base(cancellationToken) { }
    }
}
