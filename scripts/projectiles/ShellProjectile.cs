using Godot;

public partial class ShellProjectile : Area2D
{
    #region Properties

    #region Shell Projectile
    [Export]
    public float Projectile = 100.0f;

    public Vector2 ShellProjectileDirection = Vector2.Zero;

    private Node _shellProjectileParentNode;
    private int _emptyCellId = -1;
    private bool ShellHasVerticalDirection() => ShellProjectileDirection == Vector2.Up || ShellProjectileDirection == Vector2.Down;
    private bool ShellHasHorizontalDirection() => ShellProjectileDirection == Vector2.Left || ShellProjectileDirection == Vector2.Right;

    #endregion

    #region Explosion effect
    [Export]
    public PackedScene ExplosionEffect;
    #endregion

    #region Destruction effect
    [Export]
    public PackedScene DestructionEffect;
    #endregion

    #endregion

    #region  Methods

    #region  Overrides
    public override void _Ready()
    {
        ReadySceneNodes();
    }

    public override void _PhysicsProcess(double delta)
    {
        GlobalPosition += ShellProjectileDirection * Projectile * (float)delta;
    }
    #endregion

    #region Generic
    private void ReadySceneNodes()
    {
        _shellProjectileParentNode = GetParent<Node>();
    }
    #endregion

    #region Signals
    public void OnShellProjectileBodyEntered(Node2D body)
    {
        Explode();

        if (body is TileMapLayer && body.Name == "LevelAssets")
        {
            DestroyEnvironment((TileMapLayer)body);
        }

        if (body is StaticBody2D && body.Name == "PrototypeBase")
        {
            DestroyBase((FriendlyBase)body);
        }

        CallDeferred(Node2D.MethodName.QueueFree);
    }

    #endregion

    #region Explosion effect
    public void Explode()
    {
        ExplosionEffect explosionEffect = (ExplosionEffect)ExplosionEffect.Instantiate();

        explosionEffect.Position = GlobalPosition;

        _shellProjectileParentNode.CallDeferred(Node2D.MethodName.AddChild, explosionEffect);
    }
    #endregion

    #region Destruction effect
    public void Destroy()
    {
        DestructionEffect destructionEffect = (DestructionEffect)DestructionEffect.Instantiate();

        destructionEffect.Position = GlobalPosition;

        _shellProjectileParentNode.CallDeferred(Node2D.MethodName.AddChild, destructionEffect);
    }
    #endregion

    #region Environment
    private void DestroyEnvironment(TileMapLayer tileMapLayer)
    {
        Vector2I initialImpactCell = tileMapLayer.LocalToMap(GlobalPosition);

        if (tileMapLayer.GetCellSourceId(initialImpactCell) != _emptyCellId)
        {
            HandleEnvironmentDestruction(tileMapLayer, initialImpactCell);
        }
    }

    private void HandleEnvironmentDestruction(TileMapLayer tileMapLayer, Vector2I initialImpactCell)
    {

        if (ShellHasVerticalDirection())
        {
            HandleCellsDestruction(tileMapLayer, initialImpactCell, TileSet.CellNeighbor.LeftSide, TileSet.CellNeighbor.RightSide);
        }

        if (ShellHasHorizontalDirection())
        {
            HandleCellsDestruction(tileMapLayer, initialImpactCell, TileSet.CellNeighbor.TopSide, TileSet.CellNeighbor.BottomSide);
        }
    }

    private void HandleCellsDestruction(TileMapLayer tileMapLayer, Vector2I initialImpactCell, TileSet.CellNeighbor primaryNeighborCellSide, TileSet.CellNeighbor secondaryNeighborCellSide)
    {
        Vector2I primaryFirstNeighborCell = tileMapLayer.GetNeighborCell(initialImpactCell, primaryNeighborCellSide);
        int primaryFirstNeighborCellSourceId = tileMapLayer.GetCellSourceId(primaryFirstNeighborCell);

        Vector2I primarySecondNeighborCell = tileMapLayer.GetNeighborCell(primaryFirstNeighborCell, primaryNeighborCellSide);

        Vector2I secondaryNeighborCell = tileMapLayer.GetNeighborCell(initialImpactCell, secondaryNeighborCellSide);
        int secondaryNeighborCellSourceId = tileMapLayer.GetCellSourceId(secondaryNeighborCell);

        if (primaryFirstNeighborCellSourceId == _emptyCellId)
        {
            DestroyBlocks(tileMapLayer, initialImpactCell, secondaryNeighborCell);
        }
        else if (secondaryNeighborCellSourceId == _emptyCellId)
        {
            DestroyBlocks(tileMapLayer, initialImpactCell, primaryFirstNeighborCell);
        }
        else
        {
            DestroyBlocks(tileMapLayer, primarySecondNeighborCell, primaryFirstNeighborCell, initialImpactCell, secondaryNeighborCell);
        }
    }

    private void DestroyBlocks(TileMapLayer tileMapLayer, Vector2I? firstCell = null, Vector2I? secondCell = null, Vector2I? thirdCell = null, Vector2I? fourthCell = null)
    {
        if (firstCell != null)
        {
            tileMapLayer.SetCell(firstCell.Value, _emptyCellId);
        }

        if (secondCell != null)
        {
            tileMapLayer.SetCell(secondCell.Value, _emptyCellId);
        }

        if (thirdCell != null)
        {
            tileMapLayer.SetCell(thirdCell.Value, _emptyCellId);
        }

        if (fourthCell != null)
        {
            tileMapLayer.SetCell(fourthCell.Value, _emptyCellId);
        }
    }

    private void DestroyBase(FriendlyBase body)
    {
        body.Destroy();
        Destroy();
    }
    #endregion

    #endregion
}
