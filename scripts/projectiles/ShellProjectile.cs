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
        Vector2I tilePosition = tileMapLayer.LocalToMap(GlobalPosition);

        if (tileMapLayer.GetCellSourceId(tilePosition) != _emptyCellId)
        {
            HandleEnvironmentDestruction(tileMapLayer, tilePosition);
        }
    }

    private void HandleEnvironmentDestruction(TileMapLayer tileMapLayer, Vector2I tilePosition)
    {

        if (ShellProjectileDirection == Vector2.Up || ShellProjectileDirection == Vector2.Down)
        {
            NewMethod(tileMapLayer, tilePosition);
        }

        if (ShellProjectileDirection == Vector2.Left || ShellProjectileDirection == Vector2.Right)
        {
            Vector2I topFirstNeighborCell = tileMapLayer.GetNeighborCell(tilePosition, TileSet.CellNeighbor.TopSide);
            int topFirstNeighborCellSourceId = tileMapLayer.GetCellSourceId(topFirstNeighborCell);

            Vector2I topSecondNeighborCell = tileMapLayer.GetNeighborCell(topFirstNeighborCell, TileSet.CellNeighbor.TopSide);
            int topSecondNeighborCellSourceId = tileMapLayer.GetCellSourceId(topSecondNeighborCell);

            Vector2I bottomFirstNeighborCell = tileMapLayer.GetNeighborCell(tilePosition, TileSet.CellNeighbor.BottomSide);
            int bottomFirstNeighborCellSourceId = tileMapLayer.GetCellSourceId(bottomFirstNeighborCell);

            ProjectileImpactSide projectileImpactSide = topFirstNeighborCellSourceId == _emptyCellId || topSecondNeighborCellSourceId == _emptyCellId ? ProjectileImpactSide.Left :
                bottomFirstNeighborCellSourceId == _emptyCellId ? ProjectileImpactSide.Right :
                ProjectileImpactSide.Middle;

            if (projectileImpactSide == ProjectileImpactSide.Middle)
            {
                tileMapLayer.SetCell(topSecondNeighborCell, _emptyCellId);
                tileMapLayer.SetCell(topFirstNeighborCell, _emptyCellId);
                tileMapLayer.SetCell(tilePosition, _emptyCellId);
                tileMapLayer.SetCell(bottomFirstNeighborCell, _emptyCellId);
            }

            if (projectileImpactSide == ProjectileImpactSide.Left)
            {
                if (topFirstNeighborCellSourceId == _emptyCellId)
                {
                    tileMapLayer.SetCell(tilePosition, _emptyCellId);
                    tileMapLayer.SetCell(bottomFirstNeighborCell, _emptyCellId);
                }

                if (topSecondNeighborCellSourceId == _emptyCellId)
                {
                    tileMapLayer.SetCell(topSecondNeighborCell, _emptyCellId);
                    tileMapLayer.SetCell(topFirstNeighborCell, _emptyCellId);
                    tileMapLayer.SetCell(tilePosition, _emptyCellId);
                    tileMapLayer.SetCell(bottomFirstNeighborCell, _emptyCellId);
                }
            }

            if (projectileImpactSide == ProjectileImpactSide.Right)
            {
                if (bottomFirstNeighborCellSourceId == _emptyCellId)
                {
                    tileMapLayer.SetCell(tilePosition, _emptyCellId);
                    tileMapLayer.SetCell(topFirstNeighborCell, _emptyCellId);
                }
            }
        }
    }

    private void NewMethod(TileMapLayer tileMapLayer, Vector2I tilePosition)
    {
        Vector2I leftFirstNeighborCell = tileMapLayer.GetNeighborCell(tilePosition, TileSet.CellNeighbor.LeftSide);
        int leftFirstNeighborCellSourceId = tileMapLayer.GetCellSourceId(leftFirstNeighborCell);

        Vector2I leftSecondNeighborCell = tileMapLayer.GetNeighborCell(leftFirstNeighborCell, TileSet.CellNeighbor.LeftSide);
        int leftSecondNeighborCellSourceId = tileMapLayer.GetCellSourceId(leftSecondNeighborCell);

        Vector2I rightFirstNeighborCell = tileMapLayer.GetNeighborCell(tilePosition, TileSet.CellNeighbor.RightSide);
        int rightFirstNeighborCellSourceId = tileMapLayer.GetCellSourceId(rightFirstNeighborCell);

        ProjectileImpactSide projectileImpactSide = leftFirstNeighborCellSourceId == _emptyCellId || leftSecondNeighborCellSourceId == _emptyCellId ? ProjectileImpactSide.Left :
            rightFirstNeighborCellSourceId == _emptyCellId ? ProjectileImpactSide.Right :
            ProjectileImpactSide.Middle;

        if (projectileImpactSide == ProjectileImpactSide.Middle)
        {
            tileMapLayer.SetCell(leftSecondNeighborCell, _emptyCellId);
            tileMapLayer.SetCell(leftFirstNeighborCell, _emptyCellId);
            tileMapLayer.SetCell(tilePosition, _emptyCellId);
            tileMapLayer.SetCell(rightFirstNeighborCell, _emptyCellId);
        }

        if (projectileImpactSide == ProjectileImpactSide.Left)
        {
            if (leftFirstNeighborCellSourceId == _emptyCellId)
            {
                tileMapLayer.SetCell(tilePosition, _emptyCellId);
                tileMapLayer.SetCell(rightFirstNeighborCell, _emptyCellId);
            }

            if (leftSecondNeighborCellSourceId == _emptyCellId)
            {
                tileMapLayer.SetCell(leftSecondNeighborCell, _emptyCellId);
                tileMapLayer.SetCell(leftFirstNeighborCell, _emptyCellId);
                tileMapLayer.SetCell(tilePosition, _emptyCellId);
                tileMapLayer.SetCell(rightFirstNeighborCell, _emptyCellId);
            }
        }

        if (projectileImpactSide == ProjectileImpactSide.Right)
        {
            if (rightFirstNeighborCellSourceId == _emptyCellId)
            {
                tileMapLayer.SetCell(tilePosition, _emptyCellId);
                tileMapLayer.SetCell(leftFirstNeighborCell, _emptyCellId);
            }
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
