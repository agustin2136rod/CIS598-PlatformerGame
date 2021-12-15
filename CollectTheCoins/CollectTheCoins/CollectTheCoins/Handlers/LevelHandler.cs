/* LevelHandler.cs
 * Author: Agustin Rodriguez
 */
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System.IO;
using Microsoft.Xna.Framework.Input;
using CollectTheCoins.GameAssets;

namespace CollectTheCoins.Handlers
{
    /// <summary>
    /// class to represent each level
    /// </summary>
    public class LevelHandler : IDisposable
    {
        //set up all variables
        private Block[,] blocks;
        private SoundEffect coinCollected;
        private Texture2D[] backgrounds;
        private Vector2 startPosition;
        private List<SpikesSprite> spikes = new List<SpikesSprite>();
        private bool atExit;
        private Point exit = InvalidPosition;
        private static readonly Point InvalidPosition = new Point(-1, -1);
        private ContentManager content;
        private const int EntityLayer = 3;
        private Player player;
        private List<CoinHandler> coins = new List<CoinHandler>();
        private BatSprite[] bats = null;
        private DragonSprite dragon = null;
        private DragonSprite dragon2 = null;
        private MinotaurSprite minotaur = null;
        private WarriorSprite warrior = null;
        private WarriorSprite warrior2 = null;

        private TimeHandler time;
        private static LevelTimes timesForLevels = new LevelTimes();
        private int[] levelTimes = timesForLevels.TimesForLevels;
        private bool isTimePaused = false;
        private bool timerRunning = false;
        private bool enemyCollidedWithCharacter = false;
        private VolumeHandler gameVolume;

        /// <summary>
        /// getter to determine whether a character collided with an enemy 
        /// </summary>
        public bool EnemyCollidedWithCharacter { get { return enemyCollidedWithCharacter; } }

        /// <summary>
        /// getter for the coins
        /// </summary>
        public List<CoinHandler> Coins { get { return coins; } }

        /// <summary>
        /// getter to see if the timer is currently running 
        /// </summary>
        public bool TimerRunning { get { return timerRunning; } }

        /// <summary>
        /// getter for the player
        /// </summary>
        public Player Player { get { return player; } }

        /// <summary>
        /// random generator
        /// </summary>
        private Random random = new Random(354668);

        /// <summary>
        /// getter for the at exit
        /// </summary>
        public bool AtExit { get { return atExit; } }

        /// <summary>
        /// getter to check if the game timer is paused
        /// </summary>
        public bool IsTimePaused { get { return isTimePaused; } }

        /// <summary>
        /// getter for the time left in a level
        /// </summary>
        public TimeSpan TimeLeft { get { return time.RemainingTime; } }

        /// <summary>
        /// getter for the content manager
        /// </summary>
        public ContentManager Content { get { return content; } }

        /// <summary>
        /// getter for the width
        /// </summary>
        public int Width { get { return blocks.GetLength(0); } }

        /// <summary>
        /// getter for the height
        /// </summary>
        public int Height { get { return blocks.GetLength(1); } }

        #region Load
        /// <summary>
        /// construcor for the class
        /// </summary>
        /// <param name="serviceProvider">the service provider</param>
        /// <param name="stream">Stream service</param>
        /// <param name="index">the index</param>
        public LevelHandler(IServiceProvider serviceProvider, Stream stream, int index)
        {
            content = new ContentManager(serviceProvider, "Content");
            time = new TimeHandler(TimeSpan.FromSeconds(levelTimes[index]));
            LoadBlocks(stream);

            backgrounds = new Texture2D[4];
            backgrounds[0] = Content.Load<Texture2D>("backgrounds/Layer0_0");
            backgrounds[1] = Content.Load<Texture2D>("backgrounds/Layer0_1");
            backgrounds[2] = Content.Load<Texture2D>("backgrounds/Layer0_2");
            backgrounds[3] = Content.Load<Texture2D>("backgrounds/Layer0_3");
            coinCollected = Content.Load<SoundEffect>("sounds/coinPickup");

            if (index == 1)
            {
                minotaur = new MinotaurSprite() { Position = new Vector2(725, 387), Direction = MinotaurDirection.Left };
                minotaur.LoadContent(Content, new Vector2(725, 387), new Vector2(59, 387));
            }

            if (index == 2)
            {
                bats = new BatSprite[]
                {
                    new BatSprite() {Position = new Vector2(515, 160), Direction = BatDirection.Down },
                    new BatSprite() {Position = new Vector2(165, 250), Direction = BatDirection.Up},
                    new BatSprite() {Position = new Vector2(715, 155), Direction = BatDirection.Left}
                };
                foreach (var bat in bats) bat.LoadContent(Content);
            }

            if (index == 3)
            {
                warrior = new WarriorSprite() { Position = new Vector2(725, 387), Direction = WarriorDirection.Right };
                warrior.LoadContent(Content, new Vector2(725, 387), new Vector2(59, 387));
            }

            if (index == 4)
            {
                minotaur = new MinotaurSprite() { Position = new Vector2(725, 387), Direction = MinotaurDirection.Left };
                minotaur.LoadContent(Content, new Vector2(725, 387), new Vector2(59, 387));
                warrior = new WarriorSprite() { Position = new Vector2(355, 291), Direction = WarriorDirection.Right };
                warrior.LoadContent(Content, new Vector2(370, 291), new Vector2(2, 387));
                warrior2 = new WarriorSprite() { Position = new Vector2(400, 67), Direction = WarriorDirection.Right };
                warrior2.LoadContent(Content, new Vector2(745, 67), new Vector2(365, 67));
                
            }

            if (index == 5)
            {
                dragon = new DragonSprite() { Position = new Vector2(400, 145), Direction = DragonDirection.Down };
                dragon.LoadContent(Content);
            }

            if (index == 7)
            {
                bats = new BatSprite[]
                {
                    new BatSprite() {Position = new Vector2(515, 160), Direction = BatDirection.Down },
                    new BatSprite() {Position = new Vector2(165, 250), Direction = BatDirection.Up},
                    new BatSprite() {Position = new Vector2(715, 155), Direction = BatDirection.Left},
                    new BatSprite() {Position = new Vector2(215, 200), Direction = BatDirection.Right},
                };
                foreach (var bat in bats) bat.LoadContent(Content);
            }

            if (index == 8)
            {
                minotaur = new MinotaurSprite() { Position = new Vector2(725, 387), Direction = MinotaurDirection.Left };
                minotaur.LoadContent(Content, new Vector2(725, 387), new Vector2(59, 387));
                bats = new BatSprite[]
                {
                    new BatSprite() {Position = new Vector2(515, 160), Direction = BatDirection.Down },
                    new BatSprite() {Position = new Vector2(165, 250), Direction = BatDirection.Up},
                };
                foreach (var bat in bats) bat.LoadContent(Content);
            }

            if (index == 9)
            {
                dragon = new DragonSprite() { Position = new Vector2(500, 100), Direction = DragonDirection.Down };
                dragon.LoadContent(Content);
                dragon2 = new DragonSprite() { Position = new Vector2(100, 200), Direction = DragonDirection.Up };
                dragon2.LoadContent(Content);
            }

            if (index == 10)
            {
                warrior = new WarriorSprite() { Position = new Vector2(725, 387), Direction = WarriorDirection.Left };
                warrior.LoadContent(Content, new Vector2(725, 387), new Vector2(59, 387));
                warrior2 = new WarriorSprite() { Position = new Vector2(400, 67), Direction = WarriorDirection.Right };
                warrior2.LoadContent(Content, new Vector2(745, 67), new Vector2(365, 67));
                minotaur = new MinotaurSprite() { Position = new Vector2(400, 195), Direction = MinotaurDirection.Left };
                minotaur.LoadContent(Content, new Vector2(500, 195), new Vector2(2, 387));
                bats = new BatSprite[]
                {
                    new BatSprite() {Position = new Vector2(515, 160), Direction = BatDirection.Down },
                    new BatSprite() {Position = new Vector2(165, 200), Direction = BatDirection.Up},
                };
                foreach (var bat in bats) bat.LoadContent(Content);
            }
        }

        /// <summary>
        /// Method to start the level timer
        /// </summary>
        public void StartLevelTimer()
        {
            time.Start();
            timerRunning = true;
        }

        /// <summary>
        /// Method to pause the level timer
        /// </summary>
        public void PauseLevelTimer()
        {
            time.Pause();
            isTimePaused = true;
        }

        /// <summary>
        /// Method to resume the level timer
        /// </summary>
        public void ResumeLevelTimer()
        {
            time.Resume();
            isTimePaused = false;
        }

        /// <summary>
        /// Methhod to load the blocks for the current level
        /// </summary>
        /// <param name="stream">the stream service</param>
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

        /// <summary>
        /// Method to load a block 
        /// </summary>
        /// <param name="type">name of the block</param>
        /// <param name="x">x position</param>
        /// <param name="y">y position</param>
        /// <returns>Returns a block </returns>
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

                // spike
                case 'S':
                    return LoadSpikeBlock(x, y);

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

        /// <summary>
        /// method to load the block 
        /// </summary>
        /// <param name="blockName">the name of the block </param>
        /// <param name="blockCollision">the block collision type </param>
        /// <returns>The loaded Block</returns>
        private Block LoadBlock(string blockName, BlockCollision blockCollision)
        {
            return new Block(Content.Load<Texture2D>("sprites/blocks/" + blockName), blockCollision);
        }

        /// <summary>
        /// Method to load a random block 
        /// </summary>
        /// <param name="name">name of block </param>
        /// <param name="count">number for the random generator</param>
        /// <param name="blockCollision">block collision type</param>
        /// <returns>Block </returns>
        private Block LoadVarietyBlock(string name, int count, BlockCollision blockCollision)
        {
            int index = random.Next(count);
            return LoadBlock(name + index, blockCollision);
        }

        /// <summary>
        /// Method to load the starting block 
        /// </summary>
        /// <param name="x">x position</param>
        /// <param name="y">y position</param>
        /// <returns>The starting block </returns>
        private Block LoadStartBlock(int x, int y)
        {
            startPosition = RectangleExtensionHandler.GetBottomCenter(GetBounds(x, y));
            player = new Player(this, startPosition);
            return new Block(null, BlockCollision.Passable);
        }

        /// <summary>
        /// Method to load the exit block 
        /// </summary>
        /// <param name="x">x position</param>
        /// <param name="y">y position</param>
        /// <returns>the exit block </returns>
        private Block LoadExitBlock(int x, int y)
        {
            exit = GetBounds(x, y).Center;
            return LoadBlock("FinishFlag", BlockCollision.Passable);
        }

        /// <summary>
        /// Method to load a block with a coin on it
        /// </summary>
        /// <param name="x">x position</param>
        /// <param name="y">y position</param>
        /// <returns>the block with a coin on it</returns>
        private Block LoadCoinBlock(int x, int y)
        {
            Point position = GetBounds(x, y).Center;
            coins.Add(new CoinHandler(this, new Vector2(position.X, position.Y)));
            return new Block(null, BlockCollision.Passable);
        }

        private Block LoadSpikeBlock(int x, int y)
        {
            Point position = GetBounds(x, y).Center;
            spikes.Add(new SpikesSprite(Content, new Vector2(position.X - (float) 17.5, position.Y - 4)));
            return new Block(null, BlockCollision.Passable);
        }

        /// <summary>
        /// Method to dispose the level
        /// </summary>
        public void Dispose()
        {
            Content.Unload();
            warrior = null;
            minotaur = null;
            dragon = null;
            dragon2 = null;
            bats = null;
            warrior2 = null;
            enemyCollidedWithCharacter = false;
        }
        #endregion

        /// <summary>
        /// Method to get a collision
        /// </summary>
        /// <param name="x">x position</param>
        /// <param name="y">y position</param>
        /// <returns>BlockCollision</returns>
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

        /// <summary>
        /// Method to get the bounds of a rectangle
        /// </summary>
        /// <param name="x">x position</param>
        /// <param name="y">y position</param>
        /// <returns></returns>
        public Rectangle GetBounds(int x, int y)
        {
            return new Rectangle(x * Block.Width, y * Block.Height, Block.Width, Block.Height);
        }

        /// <summary>
        /// Method to update the level
        /// </summary>
        /// <param name="gameTime">elapsed game time</param>
        /// <param name="keyboardState">which keys are being pressed</param>
        /// <param name="orientation">the screen orientation</param>
        public void Update (GameTime gameTime, KeyboardState keyboardState, VolumeHandler volume)
        {
            gameVolume = volume;
            if (time.RemainingTime == TimeSpan.Zero)
            {
                Player.ApplyPhysics(gameTime);
            }
            else if (atExit)
            {
                //int secondsLeft = Math.Min((int)Math.Round(gameTime.ElapsedGameTime.TotalSeconds * 100.0f), (int)Math.Ceiling(TimeLeft.TotalSeconds));
                //timeLeft -= TimeSpan.FromSeconds(secondsLeft);
                time.SetDuration(TimeSpan.Zero);
            }
            else
            {
                if (!isTimePaused)
                {
                    Player.Update(gameTime, keyboardState, gameVolume);
                    if (bats != null)
                    {
                        foreach (var bat in bats) bat.Update(gameTime);
                        UpdateBats();
                    }

                    UpdateCoins(gameTime);

                    if (dragon != null)
                    {
                        dragon.Update(gameTime);
                        UpdateDragon();
                    }

                    if (dragon2 != null)
                    {
                        dragon2.Update(gameTime);
                        UpdateDragon2();
                    }

                    if (minotaur != null)
                    {
                        minotaur.Update(gameTime);
                        UpdateMinotaur();
                    }

                    if (warrior != null)
                    {
                        warrior.Update(gameTime);
                        UpdateWarrior();
                    }

                    if (warrior2 != null)
                    {
                        warrior2.Update(gameTime);
                        UpdateWarrior2();
                    }

                    if (spikes != null)
                    {
                        UpdateSpikes();
                    }

                    if (Player.Alive && Player.OnGround && Player.BoundingRectangle.Contains(exit))
                    {
                        ExitReached();
                    }
                }
            }
        }

        /// <summary>
        /// method to check if a bat collides with the player
        /// </summary>
        public void UpdateBats()
        {
            foreach (BatSprite bat in bats)
            {
                if (bat.BoundingRectangle.CollidesWith(Player.PlayerRectangle))
                {
                    enemyCollidedWithCharacter = true;
                    time.SetDuration(TimeSpan.Zero);
                }
            }
        }

        /// <summary>
        /// Method to check if the dragon collides with the player
        /// </summary>
        public void UpdateDragon()
        {
            if (dragon.BoundingRectangle.CollidesWith(Player.PlayerRectangle))
            {
                enemyCollidedWithCharacter = true;
                time.SetDuration(TimeSpan.Zero);
            }
        }

        /// <summary>
        /// Method to check if the second dragon collides with the player
        /// </summary>
        public void UpdateDragon2()
        {
            if (dragon2.BoundingRectangle.CollidesWith(Player.PlayerRectangle))
            {
                enemyCollidedWithCharacter = true;
                time.SetDuration(TimeSpan.Zero);
            }
        }

        /// <summary>
        /// Method to check if the minotaur collides with the player
        /// </summary>
        public void UpdateMinotaur()
        {
            if (minotaur.BoundingRectangle.CollidesWith(Player.PlayerRectangle))
            {
                enemyCollidedWithCharacter = true;
                time.SetDuration(TimeSpan.Zero);
            }
        }

        /// <summary>
        /// Method to check if the minotaur collides with the player
        /// </summary>
        public void UpdateWarrior()
        {
            if (warrior.BoundingRectangle.CollidesWith(Player.PlayerRectangle))
            {
                enemyCollidedWithCharacter = true;
                time.SetDuration(TimeSpan.Zero);
            }
        }

        /// <summary>
        /// Method to check if the minotaur collides with the player
        /// </summary>
        public void UpdateWarrior2()
        {
            if (warrior2.BoundingRectangle.CollidesWith(Player.PlayerRectangle))
            {
                enemyCollidedWithCharacter = true;
                time.SetDuration(TimeSpan.Zero);
            }
        }

        /// <summary>
        /// Method to check if a player collides with a set of spikes
        /// </summary>
        public void UpdateSpikes()
        {
            foreach (SpikesSprite spike in spikes)
            {
                if (spike.BoundingRectangle.CollidesWith(Player.PlayerRectangle))
                {
                    enemyCollidedWithCharacter = true;
                    time.SetDuration(TimeSpan.Zero);
                }
            }
        }

        /// <summary>
        /// Method to update the coins for the level
        /// </summary>
        /// <param name="gameTime">elapsed game time</param>
        public void UpdateCoins(GameTime gameTime)
        {
            for (int i = 0; i < coins.Count; i++)
            {
                CoinHandler coin = coins[i];
                coin.Update(gameTime);

                if (coin.BoundingCircle.CollidesWith(Player.BoundingRectangle))
                {
                    coinCollected.Play(gameVolume.Volume, 0f, 0f);
                    coins.RemoveAt(i--);
                }
            }
        }

        /// <summary>
        /// method that is triggered when the player reaches the exit
        /// </summary>
        private void ExitReached()
        {
            if (coins.Count == 0)
            {
                Player.ReachedExit();
                atExit = true;
            }
        }

        /// <summary>
        /// Method to start the level
        /// </summary>
        public void Start()
        {
            Player.Reset(startPosition);
        }

        /// <summary>
        /// Method to draw all the content for the level
        /// </summary>
        /// <param name="gameTime">elapsed game time</param>
        /// <param name="spriteBatch">the sprite batch</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int i = 0; i <= EntityLayer; i++)
            {
                spriteBatch.Draw(backgrounds[i], Vector2.Zero, Color.White);
            }
            
            DrawBlocks(spriteBatch);

            if (!isTimePaused)
            {
                foreach (CoinHandler coin in coins)
                {
                    coin.Draw(gameTime, spriteBatch);
                }

                if (spikes != null)
                {
                    foreach (SpikesSprite spike in spikes)
                    {
                        spike.Draw(gameTime, spriteBatch);
                    }
                }

                Player.Draw(gameTime, spriteBatch);

                if (bats != null) foreach (var bat in bats) bat.Draw(gameTime, spriteBatch);

                if (dragon != null) dragon.Draw(gameTime, spriteBatch);

                if (dragon2 != null) dragon2.Draw(gameTime, spriteBatch);

                if (minotaur != null) minotaur.Draw(gameTime, spriteBatch);

                if (warrior != null) warrior.Draw(gameTime, spriteBatch);

                if (warrior2 != null) warrior2.Draw(gameTime, spriteBatch);
            }

            for (int i = EntityLayer + 1; i < backgrounds.Length; i++)
            {
                spriteBatch.Draw(backgrounds[i], Vector2.Zero, Color.White);
            }
        }

        /// <summary>
        /// method to draw the blocks 
        /// </summary>
        /// <param name="spriteBatch">the spriteb batch</param>
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
