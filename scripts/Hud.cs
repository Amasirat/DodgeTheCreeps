using Godot;

public partial class Hud : CanvasLayer
{
    [Signal]
    public delegate void StartGameEventHandler();

    public override void _Ready()
    {
        message = GetNode<Label>("Message");
    }

    public void ShowMessage(string text)
    {
        message.Text = text;

        message.Show();
        GetNode<Timer>("MessageTimer").Start();
    }

    public async void ShowGameOver()
    {
        ShowMessage("Game Over");

        var messageTimer = GetNode<Timer>("MessageTimer");


        await ToSignal(GetTree().CreateTimer(1.0), SceneTreeTimer.SignalName.Timeout);

        ShowMessage("Score: " + GetNode<Label>("ScoreLabel").Text);

        await ToSignal(messageTimer, Timer.SignalName.Timeout);

        ShowTitle();

        await ToSignal(GetTree().CreateTimer(1.0), SceneTreeTimer.SignalName.Timeout);
        GetNode<Button>("StartButton").Show();
    }

    public void ShowTitle()
    {
        message.Text = "Dodge The Creeps!";

        message.Show();
    }

    public void UpdateScore(int score)
    {
        GetNode<Label>("ScoreLabel").Text = score.ToString();
    }

    private void OnMessageTimerTimeout()
    {
        GetNode<Label>("Message").Hide();
    }

    private void OnStartButtonPressed()
    {
        GetNode<Button>("StartButton").Hide();
        EmitSignal(SignalName.StartGame);
    }

    private Label message;
}
