using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;

namespace Platformer2D;

public class Goomba : IGameEntity
{
    public string Type { get; }
    public bool[] Collision { get; }
    public int DrawOrder { get; }
    public int UpdateOrder { get; }
    public float Pos_X { get; set; }
    public float Pos_Y { get; set; }
    public float walk_speed { get; set; }
    public Texture2D Texture { get; }
    public int Height { get; set; }
    public int Width { get; set; }

    float max_fall;
    float _world_gravity;
    float gravity_accel;
    int Anim_current;
    float Anim_timer;
    float Anim_threshold;
    float X;
    int rotation;
    Vector2 Dead_anim;
    Rectangle[] SourceRect;

    public Goomba(int x, int y, ref ContentManager Content)
    {
        Texture = Content.Load<Texture2D>("goomba");
        Type = "goomba";
        Collision = new bool[4];
        Pos_X = x * Tile.Size.X;
        Pos_Y = y * Tile.Size.Y;
        gravity_accel = 1f;
        _world_gravity = 10f;
        max_fall = 700;

        Anim_threshold = 180;
        X = 0;
        Dead_anim = new(0,0);
        walk_speed = -60;
        Anim_current = 0;

        SourceRect = new Rectangle[3];
        SourceRect[0] = new(0,0,40,40);
        SourceRect[1] = new(40,0,40,40);
        SourceRect[2] = new(80,0,40,40);
        
        Height = 40; Width = 40;
    }
    
    public void Update(GameTime gameTime)
    {

        float Elapsed_time = (float)gameTime.ElapsedGameTime.TotalSeconds;
        Pos_X += walk_speed * Elapsed_time;

        if (!Collision[0])
        {
            gravity_accel += (0.05f * gravity_accel) + _world_gravity;
            if (gravity_accel > max_fall * 1.86f)
                gravity_accel = max_fall * 1.86f;
        }
        if (Collision[0] || Collision[3])
        {
            gravity_accel = 0;
        }
        Pos_Y += gravity_accel * Elapsed_time;
    }
    public void Draw(SpriteBatch spriteBatch, GameTime gameTime, ref Vector2 Camera2D)
    {
        if (Anim_timer >= Anim_threshold)
        {
            Anim_current = (Anim_current + 1) % 2;
            Anim_timer = 0;
        }
        else
            Anim_timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

        spriteBatch.Draw(
        Texture,
        new Vector2(Pos_X, Pos_Y),
        SourceRect[Anim_current],
        Color.White,
        0f,
        Camera2D + Dead_anim,
        new Vector2(1, 1),
        (rotation == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally),
        0f
        );
    }
}