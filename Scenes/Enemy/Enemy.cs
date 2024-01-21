using Godot;
using System;

public class Enemy : KinematicBody2D
{
    private PackedScene expScene;
    private const float Speed = 50.0f;
    private const int MaxHealth = 2;
    private int currentHealth = MaxHealth;
    private Player player;

    public override void _Ready()
    {
        expScene = GD.Load<PackedScene>("res://Scenes/Exp/Exp.tscn");
    }

    public override void _Process(float delta)
    {
        if (player == null)
            player = GetNode<Player>("/root/Level/Player");
        else
        {
            LookAt(player.GlobalPosition);
            MoveAndSlide(new Vector2(Speed, 0).Rotated(Rotation));
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Exp expInstance = (Exp) expScene.Instance();
            GetNode<Node2D>("/root/Level").AddChild(expInstance);
            expInstance.GlobalPosition = GlobalPosition;
            QueueFree();
        }
    }
}
