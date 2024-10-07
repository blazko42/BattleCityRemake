using Godot;

public partial class SoundManager : Node2D
{
	#region Properties

	#region SoundManager

	//music
	public static AudioStreamPlayer2D LevelStartSFXAudioStreamPlayer2D;

	//effects
	public static AudioStreamPlayer2D IdleSFXAudioStreamPlayer2D;
	public static AudioStreamPlayer2D MoveSFXAudioStreamPlayer2D;
	public static AudioStreamPlayer2D FireSFXAudioStreamPlayer2D;
	public static AudioStreamPlayer2D DestroySFXAudioStreamPlayer2D;
	public static AudioStreamPlayer2D CollideWithArmoredTankSFXAudioStreamPlayer2D;
	public static AudioStreamPlayer2D CollideWithBrickWallSFXAudioStreamPlayer2D;
	public static AudioStreamPlayer2D CollideWithUnbreakableWallSFXAudioStreamPlayer2D;
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
		//music
		LevelStartSFXAudioStreamPlayer2D = GetNode<AudioStreamPlayer2D>("Music/LevelStartSFXAudioStreamPlayer2D");

		//effects
		IdleSFXAudioStreamPlayer2D = GetNode<AudioStreamPlayer2D>("Effects/IdleSFXAudioStreamPlayer2D");
		MoveSFXAudioStreamPlayer2D = GetNode<AudioStreamPlayer2D>("Effects/MoveSFXAudioStreamPlayer2D");
		FireSFXAudioStreamPlayer2D = GetNode<AudioStreamPlayer2D>("Effects/FireSFXAudioStreamPlayer2D");
		DestroySFXAudioStreamPlayer2D = GetNode<AudioStreamPlayer2D>("Effects/DestroySFXAudioStreamPlayer2D");
		CollideWithArmoredTankSFXAudioStreamPlayer2D = GetNode<AudioStreamPlayer2D>("Effects/CollideWithArmoredTankSFXAudioStreamPlayer2D");
		CollideWithBrickWallSFXAudioStreamPlayer2D = GetNode<AudioStreamPlayer2D>("Effects/CollideWithBrickWallSFXAudioStreamPlayer2D");
		CollideWithUnbreakableWallSFXAudioStreamPlayer2D = GetNode<AudioStreamPlayer2D>("Effects/CollideWithUnbreakableWallSFXAudioStreamPlayer2D");
	}
	#endregion

	#region Signals

	#endregion

	#endregion
}
