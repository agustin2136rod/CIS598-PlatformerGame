using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System.IO;
using Microsoft.Xna.Framework.Input;

namespace CollectTheCoins.Handlers
{
    public class LevelHandler : IDisposable
    {
        private Block[,] blocks;
        private SoundEffect coinCollected;
        private Texture2D[] backgrounds;
        ContentManager content;
        private const int EntityLayer = 3;
        Player player;
        private List<CoinHandler> coins = new List<CoinHandler>();
        private Vector2 startPosition;
        bool atExit;
        TimeSpan timeLeft;
        private Point exit = InvalidPosition;
        private static readonly Point InvalidPosition = new Point(-1, -1);

        public List<CoinHandler> Coins { get { return coins; } }

        public Player Player {  get { return player; } }

        private Random random = new Random(354668);

        public bool AtExit { get { return atExit; } }

        public TimeSpan TimeLeft { get { return timeLeft; } }

        public ContentManager Content { get { return content; } }

        #region Load
        public LevelHandler(IServiceProvider serviceProvider, Stream stream, int index)
        {
            content = new ContentManager(serviceProvider, "Content");

            timeLeft = TimeSpan.FromMinutes(1.0);

            LoadBlocks(stream);

            backgrounds = new Texture2D[4];
            backgrounds[0] = Content.Load<Texture2D>("backgrounds/Layer0_0");
            backgrounds[1] = Content.Load<Texture2D>("backgrounds/Layer0_1");
            backgrounds[2] = Content.Load<Texture2D>("backgrounds/Layer0_2");
            backgrounds[3] = Content.Load<Texture2D>("backgrounds/Layer0_3");
            coinCollected = Content.Load<SoundEffect>("sounds/coinPickup");
        }

        private void LoadBlocks(Stream stream)
        {
            int width;
            List<string> lines = new List<string>();
            using (StreamReader reader = new StreamReader(stream))
            {
                string line = reader.ReadLine();
                width = line.Length;
                while (line != null)
                {
                    lines.Add(line);
                    if (line.Length != width)
                        throw new Exception(String.Format("The length of line {0} is different from all preceeding lines.", lines.Count));
                    line = reader.ReadLine();
                }
            }

            blocks = new Block[width, lines.Count];

            for (int y = 0; y < Height; ++y)
            {
                for (int x = 0; x < Width; ++x)
                {
                    // to load each tile.
                    char tileType = lines[y][x];
                    blocks[x, y] = LoadBlock(tileType, x, y);
                }
            }
        }

        private Block LoadBlock(char type, int x, int y)
        {
            switch (type)
            {
                // Blank space
                case '.':
                    return new Block(null, BlockCollision.Passable);

                // Exit
                case 'X':
                    return LoadExitBlock(x, y);

                // Gem
                case 'C':
                    return LoadCoinBlock(x, y);

                // Platform block
                case '~':
                    return LoadVarietyBlock("BlockB", 2, BlockCollision.Platform);

                // Passable block
                case ':':
                    return LoadVarietyBlock("BlockB", 2, BlockCollision.Passable);

                // Player 1 start point
                case '1':
                    return LoadStartBlock(x, y);

                // Impassable block
                case '#':
                    return LoadVarietyBlock("BlockA", 7, BlockCollision.Impassable);

                // Unknown tile type character
                default:
                    throw new NotSupportedException(String.Format("Unsupported block type character '{0}' at position {1}, {2}.", type, x, y));
            }
        }

        private Block LoadBlock(string blockName, BlockCollision blockCollision)
        {
            return new Block(Content.Load<Texture2D>("sprites/blocks/" + blockName), blockCollision);
        }

        private Block LoadVarietyBlock(string name, int count, BlockCollision blockCollision)
        {
            int index = random.Next(count);
            return LoadBlock(name + index, blockCollision);
        }

        private Block LoadStartBlock(int x, int y)
        {
            startPosition = RectangleExtensionHandler.GetBottomCenter(GetBounds(x, y));
            player = new Player(this, startPosition);
            return new Block(null, BlockCollision.Passable);
        }

        private Block LoadExitBlock(int x, int y)
        {
            exit = GetBounds(x, y).Center;

            return LoadBlock("FinishFlag", BlockCollision.Passable);
        }

        private Block LoadCoinBlock(int x, int y)
        {
            Point position = GetBounds(x, y).Center;
            coins.Add(new CoinHandler(this, new Vector2(position.X, position.Y)));
            return new Block(null, BlockCollision.Passable);
        }

        public void Dispose()
        {
            Content.Unload();
        }
        #endregion

        public BlockCollision GetCollision(int x, int y)
        {
            if (x < 0 || x >= Width)
            {
                return BlockCollision.Impassable;
            }
            if (y < 0 || y >= Height)
            {
                return BlockCollision.Passable;
            }
            return blocks[x, y].Collision;
        }

        public Rectangle GetBounds(int x, int y)
        {
            return new Rectangle(x * Block.Width, y * Block.Height, Block.Width, Block.Height);
        }

        public int Width { get { return blocks.GetLength(0); } }

        public int Height { get { return blocks.GetLength(1); } }

        public void Update (GameTime gameTime, KeyboardState keyboardState, DisplayOrientation orientation)
        {
            if (TimeLeft == TimeSpan.Zero)
            {
                Player.ApplyPhysics(gameTime);
            }
            else if (atExit)
            {
                int secondsLeft = Math.Min((int)Math.Round(gameTime.ElapsedGameTime.TotalSeconds * 100.0f), (int)Math.Ceiling(TimeLeft.TotalSeconds));
                timeLeft -= TimeSpan.FromSeconds(secondsLeft);
            }
            else
            {
                timeLeft -= gameTime.ElapsedGameTime;
                Player.Update(gameTime, keyboardState, orientation);
                UpdateCoins(gameTime);

                if (Player.Alive && Player.OnGround && Player.BoundingRectangle.Contains(exit))
                {
                    ExitReached();
                }
            }

            if (timeLeft < TimeSpan.Zero)
            {
                timeLeft = TimeSpan.Zero;
            }
        }

        public void UpdateCoins(GameTime gameTime)
        {
            for (int i = 0; i < coins.Count; i++)
            {
                CoinHandler coin = coins[i];
                coin.Update(gameTime);

                if (coin.BoundingCircle.CollidesWith(Player.BoundingRectangle))
                {
                    coinCollected.Play();
                    coins.RemoveAt(i--);
                }
            }
        }

        private void ExitReached()
        {
            if (coins.Count == 0)
            {
                Player.ReachedExit();
                atExit = true;
            }
        }

        public void Start()
        {
            Player.Reset(startPosition);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int i = 0; i <= EntityLayer; i++)
            {
                spriteBatch.Draw(backgrounds[i], Vector2.Zero, Color.White);
            }
            DrawBlocks(spriteBatch);

            foreach (CoinHandler coin in coins)
            {
                coin.Draw(gameTime, spriteBatch);
            }

            Player.Draw(gameTime, spriteBatch);

            for (int i = EntityLayer + 1; i < backgrounds.Length; i++)
            {
                spriteBatch.Draw(backgrounds[i], Vector2.Zero, Color.White);
            }
        }

        public void DrawBlocks(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    Texture2D texture = blocks[j, i].Texture;
                    if (texture != null)
                    {
                        Vector2 position = new Vector2(j, i) * Block.Size;
                        spriteBatch.Draw(texture, position, Color.White);
                    }
                }
            }
        }

    }
}
