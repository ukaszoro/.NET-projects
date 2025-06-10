using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;

namespace Platformer2D;

public class Level
{
    private ITile[,] tiles;
    private Texture2D[] Layers;

    private const int EntityLayer = 2;

    ContentManager Content;
    EntityManager e_manager = new EntityManager();
    CollisionManager c_manager;
    Hud Hud;
    Vector2 Camera2D;
    Song song;
    EntityFactory factory;

    Player player { get; set; }
    public int Score { get; set; }
    public bool reachedExit { get; }
    public float TimeRemaining { get; set; }
    public int Lives { get; set; }
    public int coins { get; set; }
    public bool restart;

    public Level(Stream fileStream, int LevelIndex, ContentManager content, int lives)
    {
        Content = content;
        factory = EntityFactory.Instance;
        factory.SetContent(ref content);
        TimeRemaining = 160;
        coins = 0;
        Lives = lives;
        LoadMapFile(fileStream);
        Hud = new(this, Content);
        song = content.Load<Song>("theme");
        MediaPlayer.Play(song);
        MediaPlayer.Volume = 0.1f;
        SoundEffect.MasterVolume = 0.2f;
    }

    private void LoadMapFile(Stream fileStream)
    {
        int width;
        List<string> lines = new List<string>();
        using (StreamReader reader = new StreamReader(fileStream))
        {
            string line = reader.ReadLine();
            width = line.Length;
            while (line != null)
            {
                lines.Add(line);
                if (line.Length != width)
                    throw new Exception(String.Format("The lenght of line {0} is different from previous lines",
                        lines.Count));
                line = reader.ReadLine();
            }
        }

        tiles = new ITile[width, lines.Count]; // make a grid for the map

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                char tileType = lines[y][x];
                tiles[x, y] = LoadTile(tileType, x, y);
            }
        }
    }

    private ITile LoadTile(char tileType, int x, int y)
    {
        switch (tileType)
        {
            case '*':
                return null;
            case '#':
                return LoadBrickTile((y < 18 ? "block" : "block_b"), TileCollision.Impassable, 0, x, y);
            case '=':
                return LoadBrickTile((y < 18 ? "brick" : "brick_b"), TileCollision.Breakable, 0, x, y);
            case '?':
                return LoadMysteryTile("mystery_block", TileCollision.Mysteryblock, 1, x, y); // The block with 1 coin
            case '/':
                return LoadMysteryTile("mystery_block", TileCollision.Mysteryblock, -1, x,
                    y); // The block with the power up
            case '$':
                return LoadCoinTile(x, y);
            case '&':
                return LoadBlock("stair", TileCollision.Impassable, x, y);
            case 'I':
                return LoadBlock("pipe_part", TileCollision.Impassable, x, y);
            case '^':
                return LoadBlock("pipe_end", TileCollision.Impassable, x, y);
            case '<':
                return LoadBlock("side_pipe_end", TileCollision.Impassable, x, y);
            case '.':
                return LoadBlock("invisible", TileCollision.Impassable, x, y);
            case '1':
                return LoadStartTile(x, y);
            case '@':
                return LoadBlock("scenary", TileCollision.Impassable, x, y);
            case 'G':
                return LoadGoombaTile(x, y);
            case 'K':
                return LoadKoopaTile(x, y);
            case ';':
                return LoadBlock("invisible", TileCollision.Deathzone, x, y);
            default:
                throw new NotSupportedException(String.Format("Unsupported tile type character '{0}' at {1}, {2}",
                    tileType, x, y));
        }
    }

    private ITile LoadBrickTile(string name, TileCollision collision, int block_content, int x, int y)
    {
        return new Brick(Content.Load<Texture2D>(name), collision, Content, new Vector2(x, y));
    }

    private ITile LoadMysteryTile(string name, TileCollision collision, int block_content, int x, int y)
    {
        return new MysteryBlock(Content.Load<Texture2D>(name), collision, Content, new Vector2(x, y), block_content);
    }

    private Tile LoadGoombaTile(int x, int y)
    {
        e_manager.AddEntity(factory.CreateGoomba(x, y));
        return null;
    }

    private Tile LoadKoopaTile(int x, int y)
    {
        e_manager.AddEntity(factory.CreateKoompa(x, y));
        return null;
    }

    private Tile LoadCoinTile(int x, int y)
    {
        e_manager.AddEntity(factory.CreateCoin(x, y));
        return null;
    }

    private Tile LoadBlock(string name, TileCollision collision, int x, int y)
    {
        return new Tile(Content.Load<Texture2D>(name), collision, new Vector2(x, y));
    }

    private Tile LoadStartTile(int x, int y)
    {
        if (player != null)
            throw new NotSupportedException("A lavel may only have one starting point.");
        player = factory.CreatePlayer(x, y, Lives);
        e_manager.AddEntity(player);

        return null;
    }

    public void Spawn_powerup(Vector2 Pos)
    {
        if (player.Health == Health.Small)
        {
            e_manager.AddEntity(factory.CreateMushroom(Pos));
        }
        else if (player.Health > Health.Small)
        {
            e_manager.AddEntity(factory.CreateFFlower(Pos));
        }
    }

    public int Height
    {
        get { return tiles.GetLength(1); }
    }

    public int Width
    {
        get { return tiles.GetLength(0); }
    }

    public void Update(GameTime gameTime)
    {
        for (int i = 0; i < Width; i++)
        for (int j = 0; j < Height; j++)
        {
            if (tiles[i, j] != null)
                if (tiles[i, j].Break == true)
                {
                    tiles[i, j]._texture = Content.Load<Texture2D>("brick_debre");
                }
        }

        e_manager.Update(gameTime, this);

        for (int i = 0; i < Width; i++)
        for (int j = 0; j < Height; j++)
        {
            if (!(tiles[i, j] == null))
                tiles[i, j].Update(gameTime);
        }

        c_manager = new(e_manager);
        c_manager.check_collision(ref tiles, this);
        if (player.Health < Health.Dead)
        {
            MediaPlayer.Stop();
        }

        if (!e_manager._entities.Contains(player))
        {
            restart = true;
        }

        TimeRemaining -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (TimeRemaining < 0)
            player.Health = Health.Dead;
    }

    public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        spriteBatch.Draw(Content.Load<Texture2D>("scenary"), -Camera2D, Color.White);
        e_manager.Draw(spriteBatch, gameTime, ref Camera2D);
        for (int i = 0; i < Width; i++)
        for (int j = 0; j < Height; j++)
        {
            if (!(tiles[i, j] == null))
                tiles[i, j].Draw(spriteBatch, Camera2D, gameTime);
        }

        Hud.Draw(spriteBatch, gameTime);
    }
}