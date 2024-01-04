using AtlasLibMono.AtlasGraphics;
using BepuPhysics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjectTwo {
    public class Creature {
        Vector3 pos = new Vector3(0f, 1f, 0f);


        public Leg leg;
        public Leg leg1;
        public Creature() {
            leg = new Leg(2, 10f, new Vector3(0f, 10f, 0f));
            leg1 = new Leg(2, 10f, new Vector3(5f, 10f, 0f));
        }

        public void Draw(Shapes3D Shapes, GraphicsDevice Device) {
            Shapes.Sphere.Draw(pos, Vector3.Zero, 0.5f, Color.Red, Device);
            leg.Draw(Shapes, Device);
            leg1.Draw(Shapes, Device);
        }
        public void Update(KeyboardState ks) {
            SimpleControls(ks);
            leg.Update(pos);
            leg1.Update(pos + new Vector3(5f, 0f, 0f));
        }

        public void SimpleControls(KeyboardState ks) {
            float speed = 0.5f;

            if (ks.IsKeyDown(Keys.I)) {
                pos.Z -= speed;
            }
            if (ks.IsKeyDown(Keys.K)) {
                pos.Z += speed;
            }
            if (ks.IsKeyDown(Keys.J)) {
                pos.X -= speed;
            }
            if (ks.IsKeyDown(Keys.L)) {
                pos.X += speed;
            }
            if (ks.IsKeyDown(Keys.U)) {
                pos.Y -= speed;
            }
            if (ks.IsKeyDown(Keys.O)) {
                pos.Y += speed;
            }
        }

    }
}
