using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class PrototypeCharacter : CharacterBody2D
{
	#region Generic
	private Node _characterParentNode;
	#endregion

	#region Character
	[Export]
	public float CharacterSpeed = 2000.0f;


	private Vector2 _characterDirection = Vector2.Zero;
	private Vector2 _characterLastNonZeroDirection = Vector2.Up;
	private float _characterMagnitude;
	private List<string> _inputBuffer = new();

	private AnimatedSprite2D _characterAnimatedSprite;
	private AnimationTree _characterAnimationTree;
	private Marker2D _characterMarker;
	#endregion

	#region Projectile
	[Export]
	public PackedScene Projectile;
	#endregion

	#region  Effects

	#region Spawn effect
	[Export]
	public PackedScene SpawnEffect;
	#endregion

	#region Invulnerability effect
	[Export]
	public PackedScene InvulnerabilityEffect;
	#endregion

	#endregion

	public override void _Ready()
	{
		ReadySceneNodes();
		SpawnCharacter();
	}

	public override void _Process(double delta)
	{
		HandleCharacterMovementWithBuffer();

		if (Input.IsActionJustPressed("shoot"))
		{
			Shoot();
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 currentVelocity = Velocity;

		currentVelocity = _characterDirection * CharacterSpeed * (float)delta;

		Velocity = currentVelocity;

		MoveAndSlide();
	}

	private void ReadySceneNodes()
	{
		_characterParentNode = GetParent<Node>();

		_characterAnimatedSprite = GetNode<AnimatedSprite2D>("PrototypeCharacterAnimatedSprite2D");

		_characterAnimationTree = GetNode<AnimationTree>("PrototypeCharacterAnimationTree");
		_characterAnimationTree.Active = true;

		_characterMarker = GetNode<Marker2D>("PrototypeCharacterMarker2D");
	}

	#region Character movement
	private void HandleCharacterMovementWithBuffer()
	{
		UpdateInputBuffer();

		UpdateCharacterDirection();

		UpdateCharacterAnimationTreeParameters();
	}

	private void UpdateInputBuffer()
	{
		if (Input.IsActionJustPressed(CharacterAction.Up))
		{
			_inputBuffer.Add(CharacterAction.Up);
		}
		else if (Input.IsActionJustPressed(CharacterAction.Right))
		{
			_inputBuffer.Add(CharacterAction.Right);
		}
		else if (Input.IsActionJustPressed(CharacterAction.Down))
		{
			_inputBuffer.Add(CharacterAction.Down);
		}
		else if (Input.IsActionJustPressed(CharacterAction.Left))
		{
			_inputBuffer.Add(CharacterAction.Left);
		}

		if (Input.IsActionJustReleased(CharacterAction.Up))
		{
			_inputBuffer.Remove(CharacterAction.Up);
		}
		else if (Input.IsActionJustReleased(CharacterAction.Right))
		{
			_inputBuffer.Remove(CharacterAction.Right);
		}
		else if (Input.IsActionJustReleased(CharacterAction.Down))
		{
			_inputBuffer.Remove(CharacterAction.Down);
		}
		else if (Input.IsActionJustReleased(CharacterAction.Left))
		{
			_inputBuffer.Remove(CharacterAction.Left);
		}
	}

	private void UpdateCharacterDirection()
	{
		string currentInputDirection = _inputBuffer.FirstOrDefault();

		if (currentInputDirection == CharacterAction.Up)
		{
			_characterDirection = Vector2.Up;
		}
		else if (currentInputDirection == CharacterAction.Right)
		{
			_characterDirection = Vector2.Right;
		}
		else if (currentInputDirection == CharacterAction.Down)
		{
			_characterDirection = Vector2.Down;
		}
		else if (currentInputDirection == CharacterAction.Left)
		{
			_characterDirection = Vector2.Left;
		}
		else
		{
			_characterDirection = Vector2.Zero;
		}

		if (_characterDirection != Vector2.Zero)
		{
			_characterLastNonZeroDirection = _characterDirection;
		}

		_characterMagnitude = _characterDirection.Length();
	}

	private void UpdateCharacterAnimationTreeParameters()
	{
		if (_characterMagnitude > 0)
		{
			_characterAnimationTree.Set("parameters/conditions/is_idle", false);
			_characterAnimationTree.Set("parameters/conditions/is_moving", true);
		}
		else
		{
			_characterAnimationTree.Set("parameters/conditions/is_idle", true);
			_characterAnimationTree.Set("parameters/conditions/is_moving", false);
		}

		if (_characterDirection != Vector2.Zero)
		{
			_characterAnimationTree.Set("parameters/idle/blend_position", _characterDirection);
			_characterAnimationTree.Set("parameters/move/blend_position", _characterDirection);
		}
	}

	//not used
	private void HandleCharacterMovementWithoutBuffer()
	{
		_characterDirection = Input.GetVector("left", "right", "up", "down").Normalized();

		if (_characterDirection.X != 0)
		{
			_characterDirection = new Vector2(Mathf.Sign(_characterDirection.X), 0);
		}
		else if (_characterDirection.Y != 0)
		{
			_characterDirection = new Vector2(0, Mathf.Sign(_characterDirection.Y));
		}
		else
		{
			_characterDirection = Vector2.Zero;
		}

		_characterMagnitude = _characterDirection.Length();
	}

	#endregion

	#region Character shooting
	public void Shoot()
	{
		Shell shellProjectile = (Shell)Projectile.Instantiate();

		shellProjectile.GlobalPosition = _characterMarker.GlobalPosition;
		shellProjectile.GlobalRotationDegrees = GlobalRotationDegrees;
		shellProjectile.ShellDirection = _characterLastNonZeroDirection;

		_characterParentNode.AddChild(shellProjectile);
	}
	#endregion

	#region Character spawning effect
	public void SpawnCharacter()
	{
		Spawn spawnEffect = (Spawn)SpawnEffect.Instantiate();

		spawnEffect.GlobalPosition = GlobalPosition;

		_characterParentNode.CallDeferred(Node2D.MethodName.AddChild, spawnEffect);

		Visible = true; // should also have the character not being able to shoot
	}
	#endregion

	#region Character invulnerability effect
	public void Invulnerability()
	{

	}
	#endregion
}
