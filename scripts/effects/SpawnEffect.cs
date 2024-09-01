using Godot;

public partial class SpawnEffect : AnimatedSprite2D
{
	#region  Methods

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
