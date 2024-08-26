using Godot;

public partial class ShellProjectile : Area2D
{

    #region Properties
    #region Generic
    [Export]
    public float ShellSpeed = 100.0f;

    public Vector2 ShellDirection = Vector2.Zero;

    #endregion
    #endregion

    #region  Methods
    #region  Overrides
    public override void _Ready()
    {
        ReadySceneNodes();
    }

    public override void _Process(double delta)
    {

    }

    public override void _PhysicsProcess(double delta)
    {
        GlobalPosition += ShellDirection * ShellSpeed * (float)delta;
    }
    #endregion

    #region Generic
    private void ReadySceneNodes()
    {

    }
    #endregion

    #region Signals
    public void OnShellProjectileBodyEntered(Node2D body)
    {
        CallDeferred(Node2D.MethodName.QueueFree);
    }
    #endregion
    #endregion
}
