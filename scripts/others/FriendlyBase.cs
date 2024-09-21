using Godot;

public partial class FriendlyBase : StaticBody2D
{
	#region Properties

	#region FriendlyBase
	private Sprite2D _friendlyBaseSprite2D;

	private Rect2 _friendlyBaseAliveRegionRect = new Rect2(48, 33, 16, 14);
	private Rect2 _friendlyBaseDestroyedRegionRect = new Rect2(64, 34, 16, 14);

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
		_friendlyBaseSprite2D = GetNode<Sprite2D>("FriendlyBaseSprite2D");
	}

	public void Destroy()
	{
		if (_friendlyBaseSprite2D.RegionRect == _friendlyBaseAliveRegionRect)
		{
			_friendlyBaseSprite2D.RegionRect = _friendlyBaseDestroyedRegionRect;
		}
	}
	#endregion

	#endregion
}
