using Godot;

public partial class DestructionEffect : AnimatedSprite2D
{
    public void OnAnimationFinished()
    {
        CallDeferred(Node2D.MethodName.QueueFree);
    }
}
