using Godot;

public partial class Mob : RigidBody2D
{

    public override void _Ready()
    {
        var animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        string[] mobTypes = animatedSprite.SpriteFrames.GetAnimationNames();
        animatedSprite.Animation = mobTypes[GD.Randi() % mobTypes.Length];//Choose a random animation sprite for each mob instance
    }

    private void OnScreenExited()
    {
        QueueFree();
    }
}
