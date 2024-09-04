using Godot;

public partial class ExplosionEffect : AnimatedSprite2D
{
	#region  Methods

	#region Signals
	public void OnAnimationFinished()
	{
		CallDeferred(Node2D.MethodName.QueueFree);
	}
	#endregion

	#endregion
}
