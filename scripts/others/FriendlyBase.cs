using Godot;

public partial class FriendlyBase : StaticBody2D
{
	#region Properties

	#region FriendlyBase
	private Node _friendlyBaseParentNode;
	private Sprite2D _friendlyBaseSprite2D;

	private Rect2 _friendlyBaseAliveRegionRect = new Rect2(48, 33, 16, 14);
	private Rect2 _friendlyBaseDestroyedRegionRect = new Rect2(64, 34, 16, 14);

	#endregion

	#region Destruction effect
	[Export]
	public PackedScene DestructionEffect;
	#endregion

	#region Sound effects
	private AudioStreamPlayer2D _destroySFXAudioStreamPlayer2D;
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
		_friendlyBaseParentNode = GetParent<Node>();

		_friendlyBaseSprite2D = GetNode<Sprite2D>("FriendlyBaseSprite2D");

		_destroySFXAudioStreamPlayer2D = GetNode<AudioStreamPlayer2D>("FriendlyBaseSFX/DestroySFXAudioStreamPlayer2D");

	}

	#region Destruction effect
	public void Destroy()
	{
		if (_friendlyBaseSprite2D.RegionRect == _friendlyBaseAliveRegionRect)
		{
			_friendlyBaseSprite2D.RegionRect = _friendlyBaseDestroyedRegionRect;
		}

		DestroyEffect();

		DestroySoundEffect();
	}

	public void DestroyEffect()
	{
		DestructionEffect destructionEffect = (DestructionEffect)DestructionEffect.Instantiate();

		destructionEffect.Position = GlobalPosition;

		_friendlyBaseParentNode.CallDeferred(Node2D.MethodName.AddChild, destructionEffect);
	}

	private void DestroySoundEffect()
	{
		if (!_destroySFXAudioStreamPlayer2D.Playing)
		{
			_destroySFXAudioStreamPlayer2D.Play();
		}
	}
	#endregion


	#endregion

	#endregion
}
