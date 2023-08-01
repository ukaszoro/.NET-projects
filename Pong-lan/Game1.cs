using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Text;

namespace Pong_lan;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private EntityManager E_manager;
    public CollisionManager C_manager;

    StringBuilder myTextBoxDisplayCharacters = new StringBuilder();

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        Window.AllowUserResizing = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        E_manager = new();
        C_manager = new(E_manager._entities);
        
        
        Player player = new(_graphics,Window ,1);
        E_manager.AddEntity(player);
        player = new(_graphics,Window,2);
        E_manager.AddEntity(player);
        Ball ball = new(_graphics, Window);
        E_manager.AddEntity(ball);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        
        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        // TODO: Add your update logic here
        E_manager.Update(gameTime);
        C_manager.check_collision();
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(new Color(0,0,0));
        _spriteBatch.Begin();
        // Read_textbox();
        // TODO: Add your drawing code here
        E_manager.Draw(gameTime, _spriteBatch);
        _spriteBatch.End();
        base.Draw(gameTime);
    }

    protected void Read_textbox()
    {
        SpriteFont font = Content.Load<SpriteFont>("font");
        StringBuilder S_builder = new StringBuilder();
        KeyboardState state;
        for (;;)
        {
            state = Keyboard.GetState();
            foreach(var key in state.GetPressedKeys())
            {
                S_builder.Append(key);
            }
            _spriteBatch.Begin();
            _spriteBatch.DrawString(font, S_builder.ToString(), new Vector2((Window.ClientBounds.Width / 3), Window.ClientBounds.Height / 2), Color.White);
            _spriteBatch.End();
            // Console.WriteLine(S_builder.ToString());
        }
    }
}
