using Godot;

public partial class SpawnEffect : AnimatedSprite2D
{
	#region Properties
	#region Generic

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
	#endregion

	#region Generic
	private void ReadySceneNodes()
	{

	}
	#endregion

	#region Signals
	public void OnAnimationFinished()
	{
		CallDeferred(Node2D.MethodName.QueueFree);
	}

	public void OnSpawnEffectDelayTimerTimeout()
	{
		Visible = true;
		Play("default");
	}
	#endregion
	#endregion
}
