using System.Diagnostics;
using Godot;

public partial class ShellProjectile : Area2D
{
    #region Properties

    #region Shell Projectile
    [Export]
    public float Projectile = 100.0f;

    public Vector2 ShellProjectileDirection = Vector2.Zero;

    private Node _shellProjectileParentNode;
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
        if (body is TileMapLayer && body.Name == "LevelAssets")
        {
            DestroyEnvironment((TileMapLayer)body);
        }

        Explode();

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

        if (tileMapLayer.GetCellSourceId(tilePosition) != -1)
        {
            Vector2I leftFirstNeighborCell = tileMapLayer.GetNeighborCell(tilePosition, TileSet.CellNeighbor.LeftSide);
            int leftFirstNeighborCellSourceId = tileMapLayer.GetCellSourceId(leftFirstNeighborCell);

            Vector2I leftSecondNeighborCell = tileMapLayer.GetNeighborCell(leftFirstNeighborCell, TileSet.CellNeighbor.LeftSide);
            int leftSecondNeighborCellSourceId = tileMapLayer.GetCellSourceId(leftSecondNeighborCell);

            Vector2I rightFirstNeighborCell = tileMapLayer.GetNeighborCell(tilePosition, TileSet.CellNeighbor.RightSide);
            int rightFirstNeighborCellSourceId = tileMapLayer.GetCellSourceId(rightFirstNeighborCell);

            // Vector2I rightSecondNeighborCell = tileMapLayer.GetNeighborCell(rightFirstNeighborCell, TileSet.CellNeighbor.RightSide);
            // int rightSecondNeighborCellSourceId = tileMapLayer.GetCellSourceId(rightSecondNeighborCell);

            var side = leftFirstNeighborCellSourceId == -1 || leftSecondNeighborCellSourceId == -1 ? "left" :
            rightFirstNeighborCellSourceId == -1 //|| rightSecondNeighborCellSourceId == -1 
            ? "right" :
            "middle";

            if (side == "middle")
            {
                tileMapLayer.SetCell(leftSecondNeighborCell, -1);
                tileMapLayer.SetCell(leftFirstNeighborCell, -1);
                tileMapLayer.SetCell(tilePosition, -1);
                tileMapLayer.SetCell(rightFirstNeighborCell, -1);
                // tileMapLayer.SetCell(rightSecondNeighborCell, -1);
            }
            if (side == "left")
            {
                if (leftFirstNeighborCellSourceId == -1)
                {
                    tileMapLayer.SetCell(tilePosition, -1);
                    tileMapLayer.SetCell(rightFirstNeighborCell, -1);
                }
                if (leftSecondNeighborCellSourceId == -1)
                {
                    tileMapLayer.SetCell(leftSecondNeighborCell, -1);
                    tileMapLayer.SetCell(leftFirstNeighborCell, -1);
                    tileMapLayer.SetCell(tilePosition, -1);
                    tileMapLayer.SetCell(rightFirstNeighborCell, -1);
                    // tileMapLayer.SetCell(rightSecondNeighborCell, -1);
                }
            }
            if (side == "right")
            {
                if (rightFirstNeighborCellSourceId == -1)
                {
                    tileMapLayer.SetCell(tilePosition, -1);
                    tileMapLayer.SetCell(leftFirstNeighborCell, -1);
                }
                // if (rightSecondNeighborCellSourceId == -1)
                // {
                //     tileMapLayer.SetCell(leftSecondNeighborCell, -1);
                //     tileMapLayer.SetCell(leftFirstNeighborCell, -1);
                //     tileMapLayer.SetCell(tilePosition, -1);
                //     tileMapLayer.SetCell(rightFirstNeighborCell, -1);
                //     tileMapLayer.SetCell(rightSecondNeighborCell, -1);
                // }
            }
        }
    }
    #endregion

    #endregion
}
