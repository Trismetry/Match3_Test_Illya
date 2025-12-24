using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CascadeService : ICascadeService
{
    private readonly IBoardService board;
    private readonly IAnimationService animationService;

    private const float FallDelayPerCell = 0.03f;

    public CascadeService(IBoardService board, IAnimationService animationService)
    {
        this.board = board;
        this.animationService = animationService;
    }

    // Perform gravity cascade and return all gem moves
    public async Task<List<GemMove>> Cascade()
    {
        List<GemMove> moves = new();

        for (int x = 0; x < board.Width; x++)
        {
            int emptyBelow = 0;

            for (int y = 0; y < board.Height; y++)
            {
                IGemModel gem = board.GetGem(x, y);

                if (gem == null)
                {
                    emptyBelow++;
                    continue;
                }

                // Matched gems should not fall
                if (gem.IsMatched)
                    continue;

                if (emptyBelow > 0)
                {
                    int newY = y - emptyBelow;

                    Vector2Int from = new (x, y);
                    Vector2Int to = new (x, newY);

                    board.SetGem(to, gem);
                    board.SetGem(from, null);

                    gem.Position = to;

                    moves.Add(new GemMove
                    {
                        Model = gem,
                        From = from,
                        To = to
                    });
                }
            }
        }

        // Animate all moves using AnimationService
        await AnimateMoves(moves);

        return moves;
    }

    // Delegate animation of gem moves to AnimationService
    private async Task AnimateMoves(List<GemMove> moves)
    {
        List<Task> tasks = new();

        foreach (var move in moves)
        {
            if (move.Model.View == null)
                continue;

            float delay = Mathf.Abs(move.From.y - move.To.y) * FallDelayPerCell;

            tasks.Add(animationService.AnimateGemMove(
                move.Model.View,
                move.From,
                move.To,
                delay
            ));
        }

        await Task.WhenAll(tasks);
    }
}
