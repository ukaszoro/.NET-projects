using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;

namespace Platformer2D;

enum TileCollision
{
    Passable = 0,
    Impassable = 1,
    Platform = 2,
}
class Tile
{
    public Texture2D _texture;
    public TileCollision _collision;

    public const int Width = 40;
    public const int Height = 40;
    
    public static readonly Vector2 Size = new Vector2(Width, Height);

    public Tile(Texture2D texture, TileCollision collision)
    {
        _texture = texture;
        _collision = collision;
    }
    public void Draw(SpriteBatch spriteBatch, int x, int y)
    {
        Vector2 tmp_pos = new(x,y);
        spriteBatch.Draw(
        _texture,
        tmp_pos * Size,
        null,
        Color.White,
        0f,
        Vector2.One,
        Vector2.One,
        SpriteEffects.None,
        0f
        );
    }
}
class Level
{
    private Tile[,] tiles;
    private Texture2D[] Layers;

    private const int EntityLayer = 2;

    private Vector2 start;
    private Point exit;
    ContentManager Content;
    EntityManager e_manager = new EntityManager();

    public Player player { get; set; }
    public int Score { get; }
    public bool reachedExit { get; }
    public TimeSpan TimeRemaining { get; }
    public Level(Stream fileStream, int LevelIndex, ContentManager content)
    {
        Content = content;
        TimeRemaining = TimeSpan.FromMinutes(2.0);

        LoadMapFile(fileStream);
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
                    throw new Exception(String.Format("The lenght of line {0} is different from previous lines", lines.Count));
                line = reader.ReadLine();
            }
        }

        tiles = new Tile[width, lines.Count]; // make a grid for the map

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                char tileType = lines[y][x];
                tiles[x, y] = LoadTile(tileType, x, y);
            }
        }
    }
    private Tile LoadTile(char tileType, int x, int y)
    {
        switch (tileType)
        {
            case '*':
                return null;
            case '#':
                return LoadBlock("block", TileCollision.Impassable);
            case '1':
                return LoadStartTile(x, y);

            default:
                throw new NotSupportedException(String.Format("Unsupported tile type character '{0}' at {1}, {2}", tileType, x, y));
        }

    }
    private Tile LoadBlock(string name, TileCollision collision)
    {
        return new Tile(Content.Load<Texture2D>(name), collision);
    }
    private Tile LoadStartTile(int x, int y)
    {
        if (player != null)
            throw new NotSupportedException("A lavel may only have one starting point.");

        player = new Player(x, y, ref Content);
        e_manager.AddEntity(player);

        return null;
    }

    public int Height { get { return tiles.GetLength(1); } }
    public int Width { get { return tiles.GetLength(0); } }
    public void Update(GameTime gameTime)
    {
        e_manager.Update(gameTime);
                
    }
    public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {

        e_manager.Draw(spriteBatch, gameTime);
        for (int i = 0; i < Width; i++)
            for (int j = 0; j < Height; j++)
            {
                if (!(tiles[i,j] == null))
                    tiles[i,j].Draw(spriteBatch, i, j);
            }
    }
}

