using System.Threading;

namespace XboxAchiever.Core.Games
{
    abstract class PCGame : Game
    {
        public PCGame(CancellationToken cancellationToken) : base(cancellationToken) { }
    }
}
