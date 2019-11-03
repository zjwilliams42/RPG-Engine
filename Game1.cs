using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Physics_Sim.Graphics;

namespace RPG_Game_Engine
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        CoordinateSystem system;
        Model tabletop;

        private Matrix world;
        private Matrix view;
        private Matrix projection;

        private Vector3 cameraPosition;
        private int grid_size_x = 10;
        private int grid_size_z = 10;

        public Game1()
        {
            int width = 1024;
            int height = 768;
            float aspectRatio = (float) width / height;

            //cameraPosition = new Vector3(-10, 15, 5f);
            cameraPosition = new Vector3(10, 15, -100);

            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = width;
            graphics.PreferredBackBufferHeight = height; 
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            world = Matrix.CreateTranslation(new Vector3(5, 0, 5));
            view = Matrix.CreateLookAt(cameraPosition, new Vector3(grid_size_x/2, 0, grid_size_z/2), Vector3.UnitY);
            projection = Matrix.CreatePerspectiveFieldOfView(0.5f, aspectRatio, 1, 200);
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            system = new CoordinateSystem(graphics, cameraPosition, view, projection, grid_size_x, grid_size_z);

            //string save_path = "resources\\saves\\save1\\";
            RPG.CharCard charcard0 = new RPG.CharCard(0);
            RPG.CharCard charcard1 = new RPG.CharCard(1);
            //charcard0.Save(save_path);
            //charcard1.Save(save_path);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            tabletop = Content.Load<Model>("models\\table");
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            system.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);


            foreach (ModelMesh mesh in tabletop.Meshes)
            {
                foreach(BasicEffect effect in mesh.Effects)
                {
                    effect.World = world;
                    effect.View = view;
                    effect.Projection = projection;
                }
                mesh.Draw();
            }
                            

            // TODO: Add your drawing code here
            system.render();

            base.Draw(gameTime);
        }
    }
}
