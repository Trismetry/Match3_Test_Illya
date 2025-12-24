using UnityEngine;

/// <summary>
/// Central installer that wires up all services and connects them together.
/// Ensures SOLID architecture: each service has a single responsibility,
/// and dependencies are injected explicitly.
/// </summary>
public class GameInstaller : MonoBehaviour
{
    [SerializeField] private Sprite[] gemSprites;
    [SerializeField] private GameObject gemPrefab; // ✅ single prefab for pooling

    private IBoardService boardService;
    private IGemPool gemPool;
    private IGemFactory gemFactory;
    private IGemSwapService swapService;
    private IMatchService matchService;
    private IBombService bombService;
    private IDestroyService destroyService;
    private ICascadeService cascadeService;
    private IRefillService refillService;
    private IScoreService scoreService;
    private IComboService comboService;
    private IAnimationService animationService;
    private ITurnService turnService;

    private void Awake()
    {
        // 1. Core board and pool
        boardService = new BoardService(SC_GameVariables.Instance.rowsSize, SC_GameVariables.Instance.colsSize);
        gemPool = new GemPool(gemPrefab, this.transform); // ✅ prefab + parent

        // 2. Factory (creates gems with model, view, input)
        gemFactory = new GemFactory(boardService, gemSprites, gemPool);

        // 3. Swap service (pure model swap)
        swapService = new GemSwapService(boardService);

        // 4. Animation service
        ITweenProvider tweenProvider = new UnityTweenProvider(); // ✅ your tween implementation
        IEffectFactory effectFactory = new UnityEffectFactory(); // ✅ your FX factory
        animationService = new AnimationService(tweenProvider, effectFactory);

        // 5. Gameplay services (with animation injected)
        matchService = new MatchService(boardService);
        bombService = new BombService(boardService, animationService);
        destroyService = new DestroyService(boardService, animationService, gemPool);
        cascadeService = new CascadeService(boardService, animationService);
        refillService = new RefillService(boardService, gemFactory, animationService);
        scoreService = new ScoreService();
        comboService = new ComboService();

        // 6. Turn service (orchestrates swap + pipeline)
        turnService = new TurnService(
            swapService,
            boardService,
            matchService,
            bombService,
            destroyService,
            cascadeService,
            refillService,
            scoreService,
            comboService,
            animationService,
            gemFactory
        );

        // 7. Inject TurnService into factory
        gemFactory.SetTurnService(turnService);

        // 8. Initialize board
        InitializeBoard();
    }

    private void InitializeBoard()
    {
        for (int y = 0; y < SC_GameVariables.Instance.rowsSize; y++)
        {
            for (int x = 0; x < SC_GameVariables.Instance.colsSize; x++)
            {
                Vector2Int pos = new Vector2Int(x, y);
                gemFactory.CreateGem(pos);
            }
        }
    }
}
