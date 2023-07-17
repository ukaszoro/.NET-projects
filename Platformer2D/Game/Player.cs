using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Platformer2D;

public class Player : IGameEntity
{
    public int DrawOrder { get; set; }
    public int UpdateOrder { get; set; }
    
    public Texture2D texture;
    public Vector2 pos;
    public float walk_speed;
    public float jump_speed;
    public float gravity_accel;
    public float max_jump;
    public bool jumped;
    public bool collision_g;
    public bool collision_w;
    float _world_gravity;
    
    GraphicsDeviceManager _graphics;
    
    public Player(ref GraphicsDeviceManager graphics, float world_gravity)
    {
        _graphics = graphics;
        pos = new Vector2(_graphics.PreferredBackBufferWidth / 2,
        _graphics.PreferredBackBufferHeight / 2);
        walk_speed = 500f;
        jump_speed = 0f;
        max_jump = 700f;
        gravity_accel = 0f;
        _world_gravity = world_gravity;
    }

    public void HandleInput(Player player)
    {
        var kstate = Keyboard.GetState();
        
                
        if (kstate.IsKeyDown(Keys.Up) && player.collision_g == true) {
            player.jumped = true;
            player.collision_g = false;
            player.jump_speed = player.max_jump;
                
                        
        }
        // if (kstate.IsKeyUp(Keys.Up) && player1.jump_speed < 0.4f * player1.max_jump && player1.jumped) {
        //     player1.jump_speed = 0.1f * player1.max_jump;
        //     player1.jumped = false;
        // }
                   
        // if (kstate.IsKeyDown(Keys.Down)) {
        //     player1.pos.Y += player1.walk_speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        // }
        if (kstate.IsKeyDown(Keys.Left)) {
            player.walk_speed -= (Math.Abs(0.2f * player.walk_speed) + 15);
        }
        if (kstate.IsKeyDown(Keys.Right)) {
            player.walk_speed += Math.Abs(0.2f * player.walk_speed) + 15;
        }
        if (!kstate.IsKeyDown(Keys.Right) && !kstate.IsKeyDown(Keys.Left)){
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
        spriteBatch.Begin();
        spriteBatch.Draw(
        this.texture, 
        this.pos,
        null, 
        Color.White, 
        0f, 
        new Vector2(this.texture.Width / 2, this.texture.Height / 2),
        Vector2.One,
        SpriteEffects.None,
        0f
        );
        spriteBatch.End();
        
    }
    public void Update(GameTime gameTime)
    {
        this.HandleInput(this);
        
        this.collision_g = false;
        if(this.pos.X > _graphics.PreferredBackBufferWidth - this.texture.Width / 2) {
            this.pos.X = _graphics.PreferredBackBufferWidth - this.texture.Width / 2;
            this.collision_w = true;
            this.walk_speed = 0;
        }
        else if(this.pos.X < this.texture.Width / 2) {
            this.pos.X = this.texture.Width / 2;
            this.collision_w = true;
            this.walk_speed = 0;
        }
        if(this.pos.Y > _graphics.PreferredBackBufferHeight - this.texture.Height / 2) {
            this.pos.Y = _graphics.PreferredBackBufferHeight - this.texture.Height / 2;
            this.collision_g = true;

                    }
        else if(this.pos.Y < this.texture.Height / 2) {
            this.pos.Y = this.texture.Height / 2;
        }
        if (this.walk_speed > 500f)
            this.walk_speed = 500f;
        if (this.walk_speed < -500f)
            this.walk_speed = -500f;
            
        this.pos.X += this.walk_speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        
        if (!this.collision_g) {
            this.gravity_accel += (0.05f * this.gravity_accel) + _world_gravity;
            if (this.gravity_accel > this.max_jump * 2)
                this.gravity_accel = this.max_jump * 2;
        } 
        else {    
        this.gravity_accel = 0;
        this.jump_speed = 0;
        this.jumped = false;
        }
        if (collision_w && !collision_g) {
            
        }
        
        //Console.WriteLine($"{this.jump_speed}  {this.gravity_accel}");
        this.pos.Y -= (this.jump_speed - this.gravity_accel) * (float)gameTime.ElapsedGameTime.TotalSeconds;
        // if (this.jumped)
        // {
        //     this.jump_speed -= Math.Abs(0.05f * this.jump_speed) + 2;
        //     if (this.jump_speed < 20)
        //         this.jump_speed = 0;
        // }
        
    }
}
