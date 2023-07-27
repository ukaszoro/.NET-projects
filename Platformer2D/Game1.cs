using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace Platformer2D;

public class Game1 : Game
{
    Player player;
    Level level;
    int Lives;

    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        Window.AllowUserResizing = true;
        _graphics.PreferredBackBufferWidth = 800;
        _graphics.PreferredBackBufferHeight = 600;
        _graphics.ApplyChanges();
        Lives = 4;
    }

    protected override void Initialize()
    {
        Lives --;
        if (Lives >= 0)
        level = new(File.Open("./Maps/Map0", FileMode.Open), 0, Content, Lives);
        else 
        {
            GraphicsDevice.Clear(Color.Black);
            Exit();
            // TODO:Game over, 
        }
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
    }
    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        if (level.restart == true)
            Initialize();
        
        level.Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(new Color(0,0,0));

        _spriteBatch.Begin();
        level.Draw(_spriteBatch, gameTime);
        _spriteBatch.End();
        base.Draw(gameTime);
    }
}
