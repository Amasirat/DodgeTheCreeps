using Godot;
using System;

public partial class World : Node
{

    public override void _Ready()
    {
        hudElement = GetNode<Hud>("HUD");
        music = GetNode<AudioStreamPlayer2D>("Music");
        deathSound = GetNode<AudioStreamPlayer2D>("DeathSound");
    }

    public void GameOver()
    {
        music.Stop();
        deathSound.Play();
        GetNode<Timer>("MobTimer").Stop();
        GetNode<Timer>("ScoreTimer").Stop();
        hudElement.ShowGameOver();
    }

    public void NewGame()
    {
        GetTree().CallGroup("Mobs", Node.MethodName.QueueFree);
        music.Play();
        score = 0;
        hudElement.UpdateScore(score);
        hudElement.ShowMessage("Get Ready!");

        var player = GetNode<Player>("Player");
        var startPosition = GetNode<Marker2D>("StartPosition");

        player.Start(startPosition.Position);
        GetNode<Timer>("StartTimer").Start();
    }

    private void OnScoreTimerTimeout()
    {
        score++;
        hudElement.UpdateScore(score);

    }

    private void OnStartTimerTimeout()
    {
        GetNode<Timer>("MobTimer").Start();
        GetNode<Timer>("ScoreTimer").Start();
    }
    
    private void OnMobTimerTimeout()
    {

        if(MobScene == null)
        {
            throw new NullReferenceException("Mob Scene is empty");
        }

        const float PI = Mathf.Pi;

        Mob mobenemy = MobScene.Instantiate<Mob>();

        var MobSpawnLocation = GetNode<PathFollow2D>("MobPath/MobSpawnLocation");
        MobSpawnLocation.ProgressRatio = GD.Randf();

        float direction = MobSpawnLocation.Rotation + PI / 2;

        mobenemy.Position = MobSpawnLocation.Position;

        direction += (float)GD.RandRange(- PI/4, PI/4);

        mobenemy.Rotation = direction;

        Vector2 velocity = new Vector2((float)GD.RandRange(150.0, 250.0) , 0);
        mobenemy.LinearVelocity = velocity.Rotated(direction);

        AddChild(mobenemy);
    }

    [Export]
    public PackedScene MobScene { get; set; }

    private Hud hudElement;

    private AudioStreamPlayer2D music;
    private AudioStreamPlayer2D deathSound;

    private int score;
}
