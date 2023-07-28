using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;

namespace Platformer2D;

public class Mushroom : IGameEntity
{
    public string Type { get; set; }
    public bool[] Collision { get; }
    public int DrawOrder { get; }
    public int UpdateOrder { get; }
    public float Pos_X { get; set; }
    public float Pos_Y { get; set; }
    public float walk_speed { get; set; }
    public Texture2D Texture { get; }
    public int Height { get; set; }
    public int Width { get; set; }
    public bool Hurt { get; set; }
    public bool Hurt_up { get; set; }
    public float jump_speed { get; set; }
    public float gravity_accel { get; set; }
    public bool remove { get; set; }
    public Health Health { get; set; }

    float Spawn_timer;
    Vector2 Spawn_anim;
    float max_fall;
    float _world_gravity;
    SoundEffect[] Sound;
    
    public Mushroom(Vector2 Pos, ContentManager Content)
    {
        Type = "ded";
        Collision = new bool[4];
        walk_speed = 0;
        Height = 40;
        Width = 40;
        Spawn_anim.Y = -Height;
        
        Texture = Content.Load<Texture2D>("mushroom");
        Sound = new SoundEffect[2];
        Sound[0] = Content.Load<SoundEffect>("powerup_appear");
        Sound[1] = Content.Load<SoundEffect>("powerup_take");
        Pos_X = Pos.X;
        Pos_Y = Pos.Y - Height;
        Spawn_timer = 1000;
        max_fall = 400;
        _world_gravity = 10;
    }
    public void Update(GameTime gameTime, EntityManager e_manager)
    {
        float Elapsed_time = (float)gameTime.ElapsedGameTime.TotalSeconds;
        Pos_X += walk_speed * Elapsed_time;

        if (Spawn_timer == 1000)
            Sound[0].Play();
        if (remove == true)
            Sound[1].Play();
        if (!Collision[0] && Spawn_timer < 0)
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
        
        if (Spawn_timer > 0 && Spawn_anim.Y < 0)
        {
            Spawn_anim.Y += 2.5f;
        }
        else
        {
            Spawn_timer = 0;
            if (Type == "ded")
                walk_speed = 60;
            Type = "mushroom";
        }
        Spawn_timer -= Elapsed_time;
    }
    public void Draw(SpriteBatch spriteBatch, GameTime gameTime, ref Vector2 Camera2D)
    {
        spriteBatch.Draw(
        Texture,
        new Vector2(Pos_X, Pos_Y),
        null,
        Color.White,
        0f,
        Camera2D + Spawn_anim,
        new Vector2(1, 1),
        SpriteEffects.None,
        0f
        );
    }
}

public class FFlower : IGameEntity
{
    public string Type { get; set; }
    public bool[] Collision { get; }
    public int DrawOrder { get; }
    public int UpdateOrder { get; }
    public float Pos_X { get; set; }
    public float Pos_Y { get; set; }
    public float walk_speed { get; set; }
    public Texture2D Texture { get; }
    public int Height { get; set; }
    public int Width { get; set; }
    public bool Hurt { get; set; }
    public bool Hurt_up { get; set; }
    public float jump_speed { get; set; }
    public float gravity_accel { get; set; }
    public bool remove { get; set; }
    public Health Health { get; set; }

    float Spawn_timer;
    Vector2 Spawn_anim;
    Rectangle[] Sourcerect;
    int Anim_current;
    float Anim_timer;
    SoundEffect[] Sound;
    
    public FFlower(Vector2 Pos, ContentManager Content)
    {
        Type = "ded";
        Collision = new bool[4];
        walk_speed = 0;
        Height = 40;
        Width = 40;
        Spawn_anim.Y = -Height;
        
        Texture = Content.Load<Texture2D>("flower");
        Sound = new SoundEffect[2];
        Sound[0] = Content.Load<SoundEffect>("powerup_appear");
        Sound[1] = Content.Load<SoundEffect>("powerup_take");
        Pos_X = Pos.X;
        Pos_Y = Pos.Y - Height;
        Spawn_timer = 1000;

        Sourcerect = new Rectangle[4];
        Sourcerect[0] = new(0,0,40,40);
        Sourcerect[1] = new(40,0,40,40);
        Sourcerect[2] = new(80,0,40,40);
        Sourcerect[3] = new(120,0,40,40);
        Anim_current = 0;
        Anim_timer = 0;
    }
    public void Update(GameTime gameTime, EntityManager e_manager)
    {
        float Elapsed_time = (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (remove == true)
            Sound[1].Play();
        if (Spawn_timer == 1000)
            Sound[0].Play();
        if (Spawn_timer > 0 && Spawn_anim.Y < 0)
        {
            Spawn_anim.Y += 2.5f;
        }
        else
        {
            Spawn_timer = 0;
            if (Type == "ded")
                walk_speed = 60;
            Type = "fflower";
        }
        Spawn_timer -= Elapsed_time;
    }
    public void Draw(SpriteBatch spriteBatch, GameTime gameTime, ref Vector2 Camera2D)
    {
        if (Anim_timer > 80)
        {
            Anim_timer = 0;
            Anim_current = (Anim_current + 1) % 4;
        }
        else
            Anim_timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
        
        spriteBatch.Draw(
        Texture,
        new Vector2(Pos_X, Pos_Y),
        Sourcerect[Anim_current],
        Color.White,
        0f,
        Camera2D + Spawn_anim,
        new Vector2(1, 1),
        SpriteEffects.None,
        0f
        );
    }
}

public class Fireball : IGameEntity
{
    public string Type { get; set; }
    public bool[] Collision { get; }
    public int DrawOrder { get; }
    public int UpdateOrder { get; }
    public float Pos_X { get; set; }
    public float Pos_Y { get; set; }
    public float walk_speed { get; set; }
    public Texture2D Texture { get; }
    public int Height { get; set; }
    public int Width { get; set; }
    public bool Hurt { get; set; }
    public bool Hurt_up { get; set; }
    public float jump_speed { get; set; }
    public float gravity_accel { get; set; }
    public bool remove { get; set; }
    public Health Health { get; set; }

    float max_fall;
    float _world_gravity;
    Rectangle[] Sourcerect;
    float Anim_timer;
    int Anim_current;
    int rotation;
    float Explosion_timer;
    float Time_alive;
    
    public Fireball(Vector2 Pos, ContentManager Content, int rotation)
    {
        Type = "fireball";
        Collision = new bool[4];

        Height = 40;
        Width = 40;
        
        Texture = Content.Load<Texture2D>("fireball");
        Sourcerect = new Rectangle[4];
        Sourcerect[0] = new(0,0,20,20);
        Sourcerect[1] = new(20,0,20,20);
        Sourcerect[2] = new(40,0,20,20);
        Sourcerect[3] = new(60,0,20,20);
        
        Pos_X = Pos.X;
        Pos_Y = Pos.Y;
        max_fall = 400;
        _world_gravity = 6;
        Explosion_timer = 1.2f;
        if (rotation == 1)
        {
            walk_speed = 200;
            Pos_X += 20;
        }
        else if (rotation == 0)
        {
            walk_speed = -200;
            Pos_X -= 20;
        }
    }
    public void Update(GameTime gameTime, EntityManager e_manager)
    {
        float Elapsed_time = (float)gameTime.ElapsedGameTime.TotalSeconds;
        Time_alive += Elapsed_time;
        if (Time_alive > 3)
            Hurt = true;
        Pos_X += walk_speed * Elapsed_time;
        if (Hurt == true || Hurt_up == true)
        {
            Type = "ded";
            walk_speed = 0;
            jump_speed = 0;
            Sourcerect[0] = new(80, 0, 40, 40);
            Sourcerect[1] = new(120, 0, 40, 40);
            Sourcerect[2] = new(160, 0, 40, 40);
            Sourcerect[3] = new(160, 0, 40, 40);

            Explosion_timer -= Elapsed_time;
        }
        if (Explosion_timer < 0)
            remove = true;
        if (Anim_timer > 100)
        {
            Anim_timer = 0;
            Anim_current = (Anim_current + 1) % 4;
        }
        else
            Anim_timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            
        if (!Collision[0])
        {
            gravity_accel += (0.05f * gravity_accel) + _world_gravity;
            if (gravity_accel > max_fall * 1.86f)
                gravity_accel = max_fall * 1.86f;
        }
        if (Collision[0])
        {
            jump_speed = 300;
            gravity_accel = 0;
        }
        if (Collision[3])
        {
            gravity_accel = 0;
            jump_speed = 0;
        }
        Pos_Y += gravity_accel * Elapsed_time;

        if (Collision[1] || Collision[2])
        {    
            walk_speed = -walk_speed;
        }

        Pos_X += walk_speed * Elapsed_time;

        if (!Collision[0])
        {
            gravity_accel += (0.05f * gravity_accel) + _world_gravity;
            if (gravity_accel > max_fall * 1.86f)
                gravity_accel = max_fall * 1.86f;
        }
        
        Pos_Y -= (jump_speed - gravity_accel) * Elapsed_time;
    }
    public void Draw(SpriteBatch spriteBatch, GameTime gameTime, ref Vector2 Camera2D)
    {
        spriteBatch.Draw(
        Texture,
        new Vector2(Pos_X, Pos_Y),
        Sourcerect[Anim_current],
        Color.White,
        0f,
        Camera2D + new Vector2(0,-10),
        new Vector2(1, 1),
        SpriteEffects.None,
        0f
        );
    }
}

public class Coin : IGameEntity
{
    public string Type { get; set; }
    public bool[] Collision { get; }
    public int DrawOrder { get; }
    public int UpdateOrder { get; }
    public float Pos_X { get; set; }
    public float Pos_Y { get; set; }
    public float walk_speed { get; set; }
    public Texture2D Texture { get; }
    public int Height { get; set; }
    public int Width { get; set; }
    public bool Hurt { get; set; }
    public bool Hurt_up { get; set; }
    public float jump_speed { get; set; }
    public float gravity_accel { get; set; }
    public bool remove { get; set; }
    public Health Health { get; set; }

    Rectangle[] Sourcerect;
    int Anim_current;
    float Anim_timer;
    SoundEffect Sound;
    
    public Coin(int x, int y, ContentManager Content)
    {
        Collision = new bool[4];
        Type = "coin";
        Height = 40;
        Width = 40;
        
        Texture = Content.Load<Texture2D>("coin");
        Sound = Content.Load<SoundEffect>("coin_sound");
        
        Pos_X = x * Width;
        Pos_Y = y * Height;

        Sourcerect = new Rectangle[4];
        Sourcerect[0] = new(0,0,40,40);
        Sourcerect[1] = new(40,0,40,40);
        Sourcerect[2] = new(80,0,40,40);
        Sourcerect[3] = new(120,0,40,40);
        Anim_current = 0;
        Anim_timer = 0;
    }
    public void Update(GameTime gameTime, EntityManager e_manager)
    {
        float Elapsed_time = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
        if (remove == true)
        {
            Sound.Play();
        }
        if (Anim_timer > 120)
        {
            Anim_timer = 0;
            Anim_current = (Anim_current + 1) % 3;
        }
        else
        {
            Anim_timer += Elapsed_time;
        }
    }
    public void Draw(SpriteBatch spriteBatch, GameTime gameTime, ref Vector2 Camera2D)
    {        
        spriteBatch.Draw(
        Texture,
        new Vector2(Pos_X, Pos_Y),
        Sourcerect[Anim_current],
        Color.White,
        0f,
        Camera2D,
        new Vector2(1, 1),
        SpriteEffects.None,
        0f
        );
    }
}