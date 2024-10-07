using Godot;

public partial class PrototyeLevelV2 : Node2D
{
	public override void _Ready()
	{
		SoundManager.LevelStartSFXAudioStreamPlayer2D.Play();
	}

	public override void _Process(double delta)
	{
	}
}
