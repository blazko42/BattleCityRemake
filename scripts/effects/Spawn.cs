using Godot;

public partial class Spawn : AnimatedSprite2D
{
	#region Generic

	#endregion
	public override void _Ready()
	{
		ReadySceneNodes();
	}

	private void ReadySceneNodes()
	{
	}

	public override void _Process(double delta)
	{
	}

	public void OnAnimationFinished()
	{
		CallDeferred(Node2D.MethodName.QueueFree);
	}

	public void OnSpawnDelayTimerTimeout()
	{
		Visible = true;
		Play("default");
	}
}
