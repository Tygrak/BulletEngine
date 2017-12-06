using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace BulletEngine{
    public class BGame : Game{
        private SpriteFont font;
        private Texture2D playerTexture;
        private Texture2D circleTexture;
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private int score = 0;
        private Vector2 playerPos = new Vector2(0, 0);
        private Vector2[] bulletsPos = new Vector2[169];
        private Random random = new Random();
        private bool paused = false;

        public BGame(){
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 950;
            graphics.PreferredBackBufferHeight = 950;
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize(){
            // TODO: Add your initialization logic here
            for (int i = 0; i < bulletsPos.Length; i++){
                bulletsPos[i] = new Vector2((float) random.NextDouble() * 200f-100f, (float) random.NextDouble() * 190f+100f);
            }
            base.Initialize();
        }

        protected override void LoadContent(){
            spriteBatch = new SpriteBatch(GraphicsDevice);
            circleTexture = Content.Load<Texture2D>("circle");
            playerTexture = Content.Load<Texture2D>("circleMiddle");
            font = Content.Load<SpriteFont>("NovaMono");

            // TODO: use this.Content to load your game content here
        }

        public void Reset(){
            score = 0;
            playerPos = new Vector2(0, 0);
            for (int i = 0; i < bulletsPos.Length; i++){
                bulletsPos[i] = new Vector2((float) random.NextDouble() * 200f-100f, (float) random.NextDouble() * 190f+100f);
            }
            paused = false;
        }

        protected override void Update(GameTime gameTime){
            KeyboardState ks = Keyboard.GetState();
            if (paused){
                if (ks.IsKeyDown(Keys.Enter)) Reset();
                base.Update(gameTime);
                return;
            }
            float moveX = 0;
            float moveY = 0;
            if (ks.IsKeyDown(Keys.W) || ks.IsKeyDown(Keys.Up)) moveY = 1;
            if (ks.IsKeyDown(Keys.S) || ks.IsKeyDown(Keys.Down)) moveY = -1;
            if (ks.IsKeyDown(Keys.D) || ks.IsKeyDown(Keys.Right)) moveX = 1;
            if (ks.IsKeyDown(Keys.A) || ks.IsKeyDown(Keys.Left)) moveX = -1;
            if (ks.IsKeyDown(Keys.LeftShift) || ks.IsKeyDown(Keys.RightShift)){
                moveX *= 0.5f;
                moveY *= 0.5f;
            }
            if (playerPos.X + gameTime.ElapsedGameTime.Milliseconds*0.05f*moveX > -100 && playerPos.X + gameTime.ElapsedGameTime.Milliseconds*0.05f*moveX < 100){
                playerPos.X += gameTime.ElapsedGameTime.Milliseconds*0.05f*moveX;
            }
            if (playerPos.Y + gameTime.ElapsedGameTime.Milliseconds*0.05f*moveY > -100 && playerPos.Y + gameTime.ElapsedGameTime.Milliseconds*0.05f*moveY < 100){
                playerPos.Y += gameTime.ElapsedGameTime.Milliseconds*0.05f*moveY;
            }
            for (int i = 0; i < bulletsPos.Length; i++){
                bulletsPos[i].Y -= gameTime.ElapsedGameTime.Milliseconds*0.05f*(1+score/2500f);
                //Console.WriteLine(Vector2.Distance(bulletsPos[i], playerPos));
                if (Vector2.Distance(bulletsPos[i], playerPos) < 3.5f){
                    Console.WriteLine("Lol you died!");
                    paused = true;
                }
                if (bulletsPos[i].Y < -105f){
                    bulletsPos[i] = new Vector2((float) random.NextDouble() * 200f-100f, (float) random.NextDouble() * 40f+100f);
                    score++;
                }
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime){
            GraphicsDevice.Clear(Color.Black);
            
            spriteBatch.Begin();
            spriteBatch.DrawString(font, "Score: " + score, new Vector2(10, 10), Color.White);
            Vector2 pPos = new Vector2(MathExtensions.Map(playerPos.X, -100f, 100f, 0, graphics.GraphicsDevice.PresentationParameters.BackBufferWidth),
                                       MathExtensions.Map(playerPos.Y, 100f, -100f, 0, graphics.GraphicsDevice.PresentationParameters.BackBufferHeight));
            spriteBatch.Draw(playerTexture, pPos, new Rectangle(0, 0, playerTexture.Width, playerTexture.Height), Color.White,
             0f, new Vector2(playerTexture.Width/2, playerTexture.Height/2), new Vector2(0.08f, 0.08f), SpriteEffects.None, 1.0f);
            for (int i = 0; i < bulletsPos.Length; i++){
                Vector2 bPos = new Vector2(MathExtensions.Map(bulletsPos[i].X, -100f, 100f, 0, graphics.GraphicsDevice.PresentationParameters.BackBufferWidth),
                                       MathExtensions.Map(bulletsPos[i].Y, 100f, -100f, 0, graphics.GraphicsDevice.PresentationParameters.BackBufferHeight));
                spriteBatch.Draw(circleTexture, bPos, new Rectangle(0, 0, circleTexture.Width, circleTexture.Height), Color.Red,
                 0f, new Vector2(circleTexture.Width/2, circleTexture.Height/2), new Vector2(0.0675f, 0.0675f), SpriteEffects.None, 1.0f);
            }
            if (paused){
                spriteBatch.DrawString(font, "Score: " + score, new Vector2(10, 10), Color.White);
                spriteBatch.DrawString(font, "Lol, you died!\nPress 'Enter' to reset.", new Vector2(graphics.GraphicsDevice.PresentationParameters.BackBufferWidth/2-100, 
                 graphics.GraphicsDevice.PresentationParameters.BackBufferWidth/2), Color.White);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
