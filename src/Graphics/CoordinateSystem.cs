using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System.Collections.Generic;

using RPG;

namespace Physics_Sim
{
    namespace Graphics
    {
        public class CoordinateSystem
        {
            private GraphicsDeviceManager graphics;
            private Vector3 cameraPosition;
            private Grid grid;
            private BasicEffect effect;

            private List<Character> characters;

            public CoordinateSystem(GraphicsDeviceManager graphics)
            {
                this.graphics = graphics;
                cameraPosition = new Vector3(-10, 15, 5f);

                grid = new Grid(10, 10);

                float aspectRatio = graphics.PreferredBackBufferWidth / (float)graphics.PreferredBackBufferHeight;
                effect = new BasicEffect(graphics.GraphicsDevice);
                effect.VertexColorEnabled = true;
                effect.View = Matrix.CreateLookAt(cameraPosition, new Vector3(grid.X/2, 0, grid.Z/2), Vector3.UnitY);
                effect.Projection = Matrix.CreatePerspectiveFieldOfView(0.50f, aspectRatio, 1, 200);

                characters = new List<Character>();
                characters.Add(new Character());
                characters[0].location = new Vector2(1, 3);
            }

            public void Update()
            {
                MouseState state = Mouse.GetState();
                Vector3 plane_normals = Vector3.Cross(new Vector3(grid.X, 0, 0), new Vector3(0, 0, grid.Z));
                Viewport viewport = graphics.GraphicsDevice.Viewport;

                Vector3 nearWorldPoint = viewport.Unproject(
                    new Vector3(state.X, state.Y, 0f), effect.Projection, effect.View, Matrix.Identity);
                Vector3 farWorldPoint = viewport.Unproject(
                    new Vector3(state.X, state.Y, 1), effect.Projection, effect.View, Matrix.Identity);

                Vector3 direction = farWorldPoint - nearWorldPoint;
                float plane_intersection = cameraPosition.Y / direction.Y;
                Vector3 selection = new Vector3(
                    (int)((-1 * direction.X * plane_intersection) + cameraPosition.X + 0.5f),
                    (int)((-1 * direction.Y * plane_intersection) + cameraPosition.Y + 0.5f),
                    (int)((-1 * direction.Z * plane_intersection) + cameraPosition.Z + 0.5f)
                );

                grid.Deselect();
                grid.Select((int)selection.X, (int)selection.Z, Colors.HIGHLIGHT);

                foreach (Character character in characters)
                {
                    grid.Select((int)character.location.X, (int)character.location.Y, Colors.ALLY);
                }
            }

            public void render()
            {
                List<VertexPositionColor> v = new List<VertexPositionColor>();
                for (int m = 0; m < grid.X; m++)
                {
                    for (int n = 0; n < grid.Z; n++)
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            v.Add(grid.Get(m, n).Next());
                        }
                    }
                }

                foreach (var pass in effect.CurrentTechnique.Passes)
                {
                    pass.Apply();

                    graphics.GraphicsDevice.DrawUserPrimitives(
                        PrimitiveType.LineList, v.ToArray(), 0,
                        grid.X * grid.Z * 4
                    );
                }

            }
        }
    }
}
