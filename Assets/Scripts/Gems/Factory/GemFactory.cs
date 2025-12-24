using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Creates gem models and Unity gem objects using a pooling system.
/// Connects model, view, and input into a single entity.
/// </summary>
public class GemFactory : IGemFactory
{
    private readonly IBoardService board;
    private readonly IGemPool pool;
    private readonly Sprite[] gemSprites;

    // Services injected later
    private ITurnService turnService;

    // Maps model instances to their corresponding view objects
    private readonly Dictionary<IGemModel, SC_Gem> modelToView = new();

    public GemFactory(
        IBoardService board,
        Sprite[] gemSprites,
        IGemPool pool)
    {
        this.board = board;
        this.gemSprites = gemSprites;
        this.pool = pool;
    }

    /// <summary>
    /// Allows late injection of the turn service (after factory creation).
    /// </summary>
    public void SetTurnService(ITurnService turnService)
    {
        this.turnService = turnService;
    }

    /// <summary>
    /// Creates a new gem model at the given position.
    /// Uses anti-match logic to avoid instant matches whenever possible.
    /// </summary>
    public IGemModel CreateModel(Vector2Int position)
    {
        GlobalEnums.GemType type = GenerateSafeType(position);
        return new GemModel(position, type);
    }

    private GlobalEnums.GemType GenerateSafeType(Vector2Int pos)
    {
        int maxTypes = gemSprites.Length;

        List<GlobalEnums.GemType> allTypes = new();
        List<GlobalEnums.GemType> safeTypes = new();

        for (int i = 0; i < maxTypes; i++)
        {
            var type = (GlobalEnums.GemType)i;
            allTypes.Add(type);

            if (!WouldCreateMatch(pos, type))
                safeTypes.Add(type);
        }

        if (safeTypes.Count > 0)
        {
            int index = Random.Range(0, safeTypes.Count);
            return safeTypes[index];
        }

        int fallbackIndex = Random.Range(0, allTypes.Count);
        return allTypes[fallbackIndex];
    }

    private bool WouldCreateMatch(Vector2Int pos, GlobalEnums.GemType type)
    {
        if (MatchesTwoLeft(pos, type)) return true;
        if (MatchesTwoRight(pos, type)) return true;
        if (MatchesTwoDown(pos, type)) return true;
        if (MatchesTwoUp(pos, type)) return true;
        return false;
    }

    private bool MatchesTwoLeft(Vector2Int pos, GlobalEnums.GemType type)
    {
        if (pos.x < 2) return false;
        var a = board.GetGem(pos.x - 1, pos.y);
        var b = board.GetGem(pos.x - 2, pos.y);
        return a != null && b != null && a.Type == type && b.Type == type;
    }

    private bool MatchesTwoRight(Vector2Int pos, GlobalEnums.GemType type)
    {
        if (pos.x > board.Width - 3) return false;
        var a = board.GetGem(pos.x + 1, pos.y);
        var b = board.GetGem(pos.x + 2, pos.y);
        return a != null && b != null && a.Type == type && b.Type == type;
    }

    private bool MatchesTwoDown(Vector2Int pos, GlobalEnums.GemType type)
    {
        if (pos.y < 2) return false;
        var a = board.GetGem(pos.x, pos.y - 1);
        var b = board.GetGem(pos.x, pos.y - 2);
        return a != null && b != null && a.Type == type && b.Type == type;
    }

    private bool MatchesTwoUp(Vector2Int pos, GlobalEnums.GemType type)
    {
        if (pos.y > board.Height - 3) return false;
        var a = board.GetGem(pos.x, pos.y + 1);
        var b = board.GetGem(pos.x, pos.y + 2);
        return a != null && b != null && a.Type == type && b.Type == type;
    }

    /// <summary>
    /// Creates a gem entity at the given grid position.
    /// Combines model (data), view (visual), and input (interaction) into one unit.
    /// Uses pooling to avoid unnecessary allocations.
    /// </summary>
    public SC_Gem CreateGem(Vector2Int position)
    {
        // 1. Create the gem's data model
        IGemModel model = CreateModel(position);

        // 2. Retrieve a pooled GameObject that represents the gem
        SC_Gem gem = pool.Get();

        // 3. Ensure the gem has a GemView component
        if (!gem.TryGetComponent(out GemView view))
            view = gem.gameObject.AddComponent<GemView>();

        // 4. Add GemInput as a MonoBehaviour and initialize it with TurnService
        GemInput input = gem.gameObject.AddComponent<GemInput>();
        input.Initialize(gem, turnService);

        // 5. Bind the model, view, and input together
        gem.Initialize(model, view, input);

        // 6. Assign the correct sprite to the gem's view
        view.SetSprite(gemSprites[(int)model.Type]);

        // 7. Register the gem's model on the board
        board.SetGem(position, model);

        // 8. Store the mapping between model and view
        model.View = gem;
        modelToView[model] = gem;

        return gem;
    }

    /// <summary>
    /// Returns the view associated with the given model, or null if not found.
    /// </summary>
    public SC_Gem GetViewForModel(IGemModel model)
    {
        return modelToView.TryGetValue(model, out var view) ? view : null;
    }
}
