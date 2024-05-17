using Godot;
using System;

public partial class Player : Area2D
{
    [Signal]
    public delegate void HitEventHandler();

    public void OnBodyEntered(Node2D body)
    {
        EmitSignal(SignalName.Hit);
        Hide();
        GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
    }
    public override void _Ready()
    {
        animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        screenSize = GetViewportRect().Size;
    }

    public override void _Process(double delta)
    {
        Vector2 velocity = Vector2.Zero;

        if(Input.IsActionPressed("move_up"))
        {
            velocity.Y -= 1;
        }

        if(Input.IsActionPressed("move_down"))
        {
            velocity.Y += 1;
        }

        if(Input.IsActionPressed("move_left"))
        {
            velocity.X -= 1;
        }

        if(Input.IsActionPressed("move_right"))
        {
            velocity.X += 1;
        }

        if(velocity.Length() > 0)
        {
            velocity = velocity.Normalized() * Speed;
            animatedSprite.Play();
        }
        else
        {
            animatedSprite.Stop();
        }

        Position += velocity * (float)delta;
        Position = new Vector2(
            x: Mathf.Clamp(Position.X, 0, screenSize.X),
            y: Mathf.Clamp(Position.Y, 0, screenSize.Y)
        );

        if(velocity.X != 0)
        {
            animatedSprite.Animation = "walk";
            animatedSprite.FlipH = velocity.X < 0;
            animatedSprite.FlipV = false;
        }
        else if(velocity.Y != 0)
        {
            animatedSprite.Animation = "swim";
            animatedSprite.FlipH = false;
            animatedSprite.FlipV = velocity.Y > 0;
        }
    }

    public void Start(Vector2 pos)
    {
        Position = pos;
        GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
        Show();
    }

    [Export]
    public int Speed {get; set;}

    private Vector2 screenSize;
    private AnimatedSprite2D animatedSprite;
}
