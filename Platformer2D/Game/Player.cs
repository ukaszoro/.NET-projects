using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;

namespace Platformer2D;

public class Player : IGameEntity
{
    public string Type { get; }
    public bool[] Collision { get; set;}
    public int DrawOrder { get; set; }
    public int UpdateOrder { get; set; }

    public Texture2D Texture { get; }
    //public Vector2 Pos { get; set; }
    public float Pos_X { get; set; }
    public float Pos_Y { get; set; }
    public float walk_speed { get; set; }
    public float jump_speed;
    public float gravity_accel;
    public float max_jump;
    //public bool jumped;
    float _world_gravity;

    //GraphicsDeviceManager _graphics;

    public Player(int x, int y, ref ContentManager Content)
    {
        // tmp = new(texture.Width / 2, texture.Heighti); //to make sure pos shows the down middle point of the texture
        Texture = Content.Load<Texture2D>("player");
        Type = "player";
        Collision = new bool[4];
        //Pos = new Vector2(x, y) * Tile.Size;
        Pos_X = x * Tile.Size.X;
        Pos_Y = y * Tile.Size.Y;
        walk_speed = 500f;
        jump_speed = 0f;
        max_jump = 700f;
        gravity_accel = 0f;
        _world_gravity = 10f;
    }

    public void HandleInput(Player player)
    {
        var kstate = Keyboard.GetState();


        if (kstate.IsKeyDown(Keys.Up) && player.Collision[0] == true && !player.Collision[3])
        {
            player.Collision[0] = false;
            player.jump_speed = player.max_jump;


        }
        // if (kstate.IsKeyUp(Keys.Up) && player1.jump_speed < 0.4f * player1.max_jump && player1.jumped) {
        //     player1.jump_speed = 0.1f * player1.max_jump;
        //     player1.jumped = false;
        // }

        if (kstate.IsKeyDown(Keys.Down)) {
            Pos_Y += 10f; 
        }
        if (kstate.IsKeyDown(Keys.Left))
        {
            player.walk_speed -= (Math.Abs(0.2f * player.walk_speed) + 15);
        }
        if (kstate.IsKeyDown(Keys.Right))
        {
            player.walk_speed += Math.Abs(0.2f * player.walk_speed) + 15;
        }
        if (!kstate.IsKeyDown(Keys.Right) && !kstate.IsKeyDown(Keys.Left))
        {
            if (player.walk_speed > 10)
                player.walk_speed -= Math.Abs(0.15f * player.walk_speed) + 10;
            if (player.walk_speed < -10)
                player.walk_speed += Math.Abs(0.15f * player.walk_speed) + 10;
            if (player.walk_speed > -10 && player.walk_speed < 10)
                player.walk_speed = 0;
        }
    }

    public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        spriteBatch.Draw(
        this.Texture,
        new Vector2(Pos_X, Pos_Y),
        null,
        Color.White,
        0f,
        Vector2.One,
        Vector2.One,
        SpriteEffects.None,
        0f
        );

    }
    public void Update(GameTime gameTime)
    {
        this.HandleInput(this);


        float Elapsed_time = (float)gameTime.ElapsedGameTime.TotalSeconds;
        
        //Dead collision detection, TODO make sure it doesn't use _graphics after completing the Level class
        // if(this.pos.X > _graphics.PreferredBackBufferWidth - this.texture.Width / 2) {
        //     this.pos.X = _graphics.PreferredBackBufferWidth - this.texture.Width / 2;
        //     this. = true;
        //     this.walk_speed = 0;
        // }
        // else if(this.pos.X < this.texture.Width / 2) {
        //     this.pos.X = this.texture.Width / 2;
        //     this. = true;
        //     this.walk_speed = 0;
        // }
        // if(this.pos.Y > _graphics.PreferredBackBufferHeight - this.texture.Height / 2) {
        //     this.pos.Y = _graphics.PreferredBackBufferHeight - this.texture.Height / 2;
        //     this.Collision[0] = true;
        // }
        // else if(this.pos.Y < this.texture.Height / 2) {
        //     this.pos.Y = this.texture.Height / 2;
        // }

        if (this.walk_speed > 500f)
            this.walk_speed = 500f;
        if (this.walk_speed < -500f)
            this.walk_speed = -500f;
        if (Collision[1] || Collision[2])
            walk_speed = 0;

        this.Pos_X += this.walk_speed * Elapsed_time;

        if (!this.Collision[0])
        {
            this.gravity_accel += (0.05f * this.gravity_accel) + _world_gravity;
            if (this.gravity_accel > this.max_jump * 2)
                this.gravity_accel = this.max_jump * 2;
        }
        if (this.Collision[0])
        {
            this.gravity_accel = 0;
            this.jump_speed = 0;
        }
        
        this.Pos_Y -= (this.jump_speed - this.gravity_accel) * Elapsed_time;
    }
}
