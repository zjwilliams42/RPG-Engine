using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System.Collections.Generic;

namespace Physics_Sim
{
    namespace Graphics
{
        public static class Colors
        {
            public static Color DEFAULT = Color.LightGray;
            public static Color HIGHLIGHT = Color.LightBlue;
            public static Color ALLY = Color.LightGreen;
            public static Color HOSTILE = Color.Red;
        }

        public class Grid
        {
            public List<Vector3> vertices { get; }
            public List<Color> color { get; }
            public List<List<Cell>> cells { get; set; }
            public int X;
            public int Z;

            public Grid(int x, int z)
            {
                X = x;
                Z = z;

                vertices = new List<Vector3>();
                color = new List<Color>();

                cells = new List<List<Cell>>();
                for (int i = 0; i < x; i++)
                {
                    cells.Add(new List<Cell>());
                }

                for (int x_iter = 0; x_iter < x; x_iter++)
                {
                    for (int z_iter = 0; z_iter < z; z_iter++)
                    {
                        int a = Add(new Vector3(x_iter + 0.5f, 0, z_iter + 0.5f));
                        int b = Add(new Vector3(x_iter + 0.5f, 0, z_iter - 0.5f));
                        int c = Add(new Vector3(x_iter - 0.5f, 0, z_iter + 0.5f));
                        int d = Add(new Vector3(x_iter - 0.5f, 0, z_iter - 0.5f));

                        Cell cell = new Cell(a, b, c, d, this);
                        cells[x_iter].Insert(z_iter, cell);
                    }
                }

                for (int i = 0; i < vertices.Count; i++)
                {
                    color.Add(Colors.DEFAULT);
                }
            }

            private int Add(Vector3 vertice)
            {
                int index = vertices.IndexOf(vertice);
                if (index != -1) return index;

                index = vertices.Count;
                vertices.Add(vertice);
                return index;
            }

            public Cell Get(int x, int z) { return cells[x][z]; }
            public void Select(int x, int z, Color color)
            {
                if (x < 0 || x >= cells.Count) return;
                if (z < 0 || z >= cells[x].Count) return;
                cells[x][z].Select(color);
            }

            public void Deselect()
            {
                foreach (List<Cell> list in cells)
                {
                    foreach (Cell cell in list)
                    {
                        cell.Deselect();
                    }
                }
            }
        }

        public class Cell
        {
            private int[] vertices;
            private int iter;
            private Grid grid;

            public Cell(int a, int b, int c, int d, Grid grid)
            {
                iter = 0;
                this.grid = grid;

                vertices = new int[4];
                vertices[0] = a;
                vertices[1] = b;
                vertices[2] = c;
                vertices[3] = d;
            }

            public void Select(Color color)
            {
                grid.color[vertices[0]] = color;
                grid.color[vertices[1]] = color;
                grid.color[vertices[2]] = color;
                grid.color[vertices[3]] = color;
            }

            public void Deselect()
            {
                grid.color[vertices[0]] = Colors.DEFAULT;
                grid.color[vertices[1]] = Colors.DEFAULT;
                grid.color[vertices[2]] = Colors.DEFAULT;
                grid.color[vertices[3]] = Colors.DEFAULT;
            }

            public VertexPositionColor Next()
            {
                switch (iter++)
                {
                    case 0:
                        return new VertexPositionColor(grid.vertices[vertices[2]], grid.color[vertices[2]]);
                    case 1:
                        return new VertexPositionColor(grid.vertices[vertices[3]], grid.color[vertices[3]]);
                    case 2:
                        return new VertexPositionColor(grid.vertices[vertices[3]], grid.color[vertices[3]]);
                    case 3:
                        return new VertexPositionColor(grid.vertices[vertices[1]], grid.color[vertices[1]]);
                    case 4:
                        return new VertexPositionColor(grid.vertices[vertices[1]], grid.color[vertices[1]]);
                    case 5:
                        return new VertexPositionColor(grid.vertices[vertices[0]], grid.color[vertices[0]]);
                    case 6:
                        return new VertexPositionColor(grid.vertices[vertices[0]], grid.color[vertices[0]]);
                    case 7:
                        return new VertexPositionColor(grid.vertices[vertices[2]], grid.color[vertices[2]]);
                    default:
                        iter = 1;
                        return new VertexPositionColor(grid.vertices[vertices[2]], grid.color[vertices[2]]);
                }
            }


        }
    }
}
