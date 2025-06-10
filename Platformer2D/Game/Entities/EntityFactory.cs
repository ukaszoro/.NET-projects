using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Platformer2D;

public sealed class EntityFactory
{
    private ContentManager Content;

    private static readonly Lazy<EntityFactory> _instance =
        new(() => new EntityFactory());

    public static EntityFactory Instance => _instance.Value;


    public EntityFactory()
    {
    }

    public void SetContent(ref ContentManager content)
    {
        Content = content;
    }

    public Goomba CreateGoomba(int x, int y)
    {
        Goomba goomba = new(x, y, Content.Load<Texture2D>("goomba"), Content.Load<SoundEffect>("stomp"));
        return goomba;
    }

    public Koopa CreateKoompa(int x, int y)
    {
        Koopa koopa = new(x, y, Content.Load<Texture2D>("koopa"), Content.Load<SoundEffect>("stomp"));
        return koopa;
    }

    public Coin CreateCoin(int x, int y)
    {
        Coin coin = new(x, y, Content.Load<Texture2D>("coin"), Content.Load<SoundEffect>("coin_sound"));
        return coin;
    }

    public Mushroom CreateMushroom(Vector2 vect)
    {
        Mushroom mushroom = new(vect, Content.Load<Texture2D>("mushroom"), Content.Load<SoundEffect>("powerup_appear"),
            Content.Load<SoundEffect>("powerup_take"));
        return mushroom;
    }

    public FFlower CreateFFlower(Vector2 vect)
    {
        FFlower flower = new(vect, Content.Load<Texture2D>("flower"), Content.Load<SoundEffect>("powerup_appear"),
            Content.Load<SoundEffect>("powerup_take"));
        return flower;
    }

    public Player CreatePlayer(int x, int y, int Lives)
    {
        Player player = new(x, y, Content.Load<Texture2D>("player"), Content.Load<SoundEffect>("jump"),
            Content.Load<SoundEffect>("mario_die"), Content.Load<SoundEffect>("pipe_travel"), Lives);
        return player;
    }

    public Fireball CreateFireball(Vector2 vect, int rotation)
    {
        Fireball fireball = new(vect, Content.Load<Texture2D>("fireball"), rotation);
        return fireball;
    }
}