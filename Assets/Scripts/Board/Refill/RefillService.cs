using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class RefillService : IRefillService
{
    private readonly IBoardService board;
    private readonly IGemFactory gemFactory;
    private readonly IAnimationService animationService;

    private const float SpawnDelayPerCell = 0.03f;

    public RefillService(IBoardService board, IGemFactory gemFactory, IAnimationService animationService)
    {
        this.board = board;
        this.gemFactory = gemFactory;
        this.animationService = animationService;
    }

    // Refill all empty cells with new gems and play spawn animations
    public async Task Refill()
    {
        List<Task> spawnTasks = new();

        for (int x = 0; x < board.Width; x++)
        {
            for (int y = 0; y < board.Height; y++)
            {
                if (board.GetGem(x, y) != null)
                    continue;

                Vector2Int pos = new(x, y);

                // Create model + view via factory
                SC_Gem view = gemFactory.CreateGem(pos);

                // Staggered spawn delay based on height
                float delay = y * SpawnDelayPerCell;

                spawnTasks.Add(SpawnWithDelay(view, delay));
            }
        }

        await Task.WhenAll(spawnTasks);
    }

    // Delegate spawn animation to AnimationService with delay
    private async Task SpawnWithDelay(SC_Gem view, float delay)
    {
        if (delay > 0)
            await Task.Delay((int)(delay * 1000));

        await animationService.AnimateSpawn(view);
    }
}
