using System.Collections.Generic;
using UnityEngine;

public class DestroyService : IDestroyService
{
    private readonly IBoardService board;
    private readonly IAnimationService animationService;
    private readonly IGemPool pool;

    public DestroyService(IBoardService board, IAnimationService animationService, IGemPool pool)
    {
        this.board = board;
        this.animationService = animationService;
        this.pool = pool;
    }

    // Destroy all matched non-bomb gems and return them to the pool
    public List<IGemModel> DestroyMatched()
    {
        List<IGemModel> destroyed = new();

        for (int x = 0; x < board.Width; x++)
        {
            for (int y = 0; y < board.Height; y++)
            {
                IGemModel model = board.GetGem(x, y);
                if (model == null)
                    continue;

                // Skip bombs (handled by BombService)
                if (model.IsBomb)
                    continue;

                if (!model.IsMatched)
                    continue;

                // Animate destruction if view exists
                if (model.View != null)
                    animationService.AnimateDestroy(model.View);

                // Return gem view to pool
                if (model.View != null)
                    pool.Return(model.View);

                // Remove model from board
                board.SetGem(new Vector2Int(x, y), null);

                destroyed.Add(model);
            }
        }

        return destroyed;
    }
}
