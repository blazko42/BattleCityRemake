using Godot;

public partial class Shell : Area2D
{

    #region Generic
    #endregion

    #region Projectile
    [Export]
    public float ShellSpeed = 100.0f;

    public Vector2 ShellDirection = Vector2.Zero;

    #endregion

    public override void _Ready()
    {

    }

    public override void _Process(double delta)
    {

    }

    public override void _PhysicsProcess(double delta)
    {
        GlobalPosition += ShellDirection * ShellSpeed * (float)delta;
    }

    public void OnShellBodyEntered(Node2D body)
    {
        CallDeferred(Node2D.MethodName.QueueFree);
    }
}
