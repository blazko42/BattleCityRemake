using Godot;

public partial class ExplosionEffect : AnimatedSprite2D
{
	public void OnAnimationFinished()
	{
		CallDeferred(Node2D.MethodName.QueueFree);
	}
}
