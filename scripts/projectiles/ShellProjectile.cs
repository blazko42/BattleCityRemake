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
        EnableExplosionEffect();

        CallDeferred(Node2D.MethodName.QueueFree);
    }
    #endregion

    #region Explosion effect
    public void EnableExplosionEffect()
    {
        ExplosionEffect explosionEffect = (ExplosionEffect)ExplosionEffect.Instantiate();

        explosionEffect.Position = GlobalPosition;

        _shellProjectileParentNode.CallDeferred(Node2D.MethodName.AddChild, explosionEffect);
    }
    #endregion

    #endregion
}
