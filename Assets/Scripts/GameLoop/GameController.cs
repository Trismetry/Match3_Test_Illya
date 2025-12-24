using System.Threading.Tasks;
using UnityEngine;

public class GameController : MonoBehaviour, IGameController
{
    private ITurnService turnService;
    private IGemFactory gemFactory;
    private IAnimationService animationService;

    private bool isTurnRunning = false;

    public void Initialize(
        ITurnService turnService,
        IBoardService board,
        IAnimationService animationService,
        IGemFactory gemFactory,
        ICascadeService cascadeService,
        IRefillService refillService,
        IDestroyService destroyService)
    {
        this.turnService = turnService;
        this.animationService = animationService;
        this.gemFactory = gemFactory;
    }

    public void OnSwapRequested(IGemModel a, IGemModel b)
    {
        if (isTurnRunning)
            return;

        _ = RunTurnAsync(a, b);
    }

    private async Task RunTurnAsync(IGemModel a, IGemModel b)
    {
        isTurnRunning = true;

        // 1. Animate swap (document: GameController handles only input + swap animation)
        SC_Gem viewA = gemFactory.GetViewForModel(a);
        SC_Gem viewB = gemFactory.GetViewForModel(b);
        await animationService.AnimateSwap(viewA, viewB);

        // 2. Execute full turn logic (document: TurnService handles ALL logic)
        await turnService.ExecuteTurn(a, b);

        // 3. Unlock input
        isTurnRunning = false;
    }
}
