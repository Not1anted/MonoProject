using AtlasLibMono.Atlas3D;
using AtlasLibMono.AtlasGraphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace ProjectTwo {
    public class Game1 : Game {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch sb;
        private SpriteFont font;

        Effect effect;
        RenderTarget2D renderTarget;
        Shapes3D shapes;
        Creature c;
        Model model;

        Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 800f / 480f, 0.1f, 1000f);
        Camera cam = new Camera(15, 3);

        Matrix[] poss = new Matrix[] { Matrix.CreateTranslation(0f, 10f, 0f), Matrix.CreateTranslation(0f, 5f, 0f) };
        public Game1() {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
            Window.Title = "Gleam";
            IsFixedTimeStep = true;
            TargetElapsedTime = TimeSpan.FromMilliseconds(16);

            _graphics.PreferredBackBufferWidth = 1280;  // set this value to the desired width of your window
            _graphics.PreferredBackBufferHeight = 880;   // set this value to the desired height of your window
            _graphics.ApplyChanges();
        }

        protected override void Initialize() {
            renderTarget = new RenderTarget2D(
                GraphicsDevice,
                GraphicsDevice.PresentationParameters.BackBufferWidth,
                GraphicsDevice.PresentationParameters.BackBufferHeight,
                false,
                GraphicsDevice.PresentationParameters.BackBufferFormat,
                DepthFormat.Depth24);



            base.Initialize();
        }

        protected override void LoadContent() {
            sb = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("Fonts/font");
            model = Content.Load<Model>("Models/cone");
            effect = Content.Load<Effect>("Shaders/Pixelation");

            shapes = new Shapes3D(this, projection);
            c = new Creature();
        }

        protected override void Update(GameTime gameTime) {
            base.Update(gameTime);

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            c.Update(Keyboard.GetState());
            cam.MouseInput((float)gameTime.ElapsedGameTime.TotalSeconds, _graphics);
            cam.ProcessInput((float)gameTime.ElapsedGameTime.TotalSeconds);
            shapes.Update(cam);
        }

        protected override void Draw(GameTime gameTime) {
            base.Draw(gameTime);
            DrawSceneToTexture(renderTarget);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // 3d rendering
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            /*Vector3 directionToTarget = c.leg.segs[0].b - c.leg.segs[0].a;
            directionToTarget.Normalize();
            Quaternion rotationQuaternion = Quaternion.CreateFromRotationMatrix(Matrix.CreateLookAt(Vector3.Zero, directionToTarget, Vector3.Up));
            poss[0] = Matrix.CreateFromQuaternion(rotationQuaternion) * Matrix.CreateTranslation(c.leg.segs[0].b);
            poss[1] = Matrix.CreateTranslation(c.leg.segs[0].a);
            DrawModel(model, Matrix.CreateScale(new Vector3(1f, 1f, 1f)) * Matrix.CreateTranslation(new Vector3(0, 5, 0)), cam.GetViewMatrix(), projection);*/

            // 2d rendereing
            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearWrap, DepthStencilState.Default, RasterizerState.CullNone, effect);
            sb.Draw(renderTarget, new Rectangle(0, 0, 1280, 880), Color.White);
            sb.End();


            sb.Begin();
            sb.DrawString(font, "cam pos: " + cam.Position, new Vector2(10f, 0f), Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f);
            sb.End();
        }

        private void DrawModel(Model model, Matrix world, Matrix view, Matrix projection) {
            int i = 0;
            foreach (ModelMesh mesh in model.Meshes) {
                foreach (BasicEffect effect in mesh.Effects) {
                    effect.World = poss[i];
                    effect.View = view;
                    effect.Projection = projection;
                }

                mesh.Draw();
                i++;
            }
        }

        protected void DrawSceneToTexture(RenderTarget2D renderTarget) {
            // Set the render target
            GraphicsDevice.SetRenderTarget(renderTarget);

            GraphicsDevice.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };

            // Draw the scene
            GraphicsDevice.Clear(Color.CornflowerBlue);
            // draw face culled objects
            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.CullClockwiseFace;
            GraphicsDevice.RasterizerState = rasterizerState;
            c.Draw(shapes, GraphicsDevice);

            // Drop the render target
            GraphicsDevice.SetRenderTarget(null);
        }

        protected override void UnloadContent() {
            base.UnloadContent();

            
            sb.Dispose();
        }
    }
}
