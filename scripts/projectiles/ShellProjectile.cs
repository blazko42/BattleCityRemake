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
        if (body is TileMapLayer)
        {
            TileMapLayer tileMapLayer = (TileMapLayer)body;
            GD.Print(tileMapLayer);
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

    #endregion
}
