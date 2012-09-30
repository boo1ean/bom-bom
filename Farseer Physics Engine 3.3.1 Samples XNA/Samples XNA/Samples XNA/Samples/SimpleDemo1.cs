using System.Text;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FarseerPhysics.SamplesFramework
{
    using System;
    using System.Linq;

    using Microsoft.Xna.Framework.Input;

    internal class SimpleDemo1 : PhysicsGameScreen, IDemoScreen
    {
        private Border _border;
        private Body _rectangle;
        private Sprite _rectangleSprite;
        private Body[] _obstacles = new Body[5];
        private Sprite _obstacle;

        private Body gamePad2;
        private Sprite gamePadSprite2;

        private Body gamePad1;

        private Sprite gamePadSprite1;

        #region IDemoScreen Members

        public string GetTitle()
        {
            return "Body with a single fixture";
        }

        public string GetDetails()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("This demo shows a single body with one attached fixture and shape.");
            sb.AppendLine("A fixture binds a shape to a body and adds material");
            sb.AppendLine("properties such as density, friction, and restitution.");
            sb.AppendLine(string.Empty);
            sb.AppendLine("GamePad:");
            sb.AppendLine("  - Rotate object: left and right triggers");
            sb.AppendLine("  - Move object: right thumbstick");
            sb.AppendLine("  - Move cursor: left thumbstick");
            sb.AppendLine("  - Grab object (beneath cursor): A button");
            sb.AppendLine("  - Drag grabbed object: left thumbstick");
            sb.AppendLine("  - Exit to menu: Back button");
            sb.AppendLine(string.Empty);
            sb.AppendLine("Keyboard:");
            sb.AppendLine("  - Rotate Object: left and right arrows");
            sb.AppendLine("  - Move Object: A,S,D,W");
            sb.AppendLine("  - Exit to menu: Escape");
            sb.AppendLine(string.Empty);
            sb.AppendLine("Mouse / Touchscreen");
            sb.AppendLine("  - Grab object (beneath cursor): Left click");
            sb.AppendLine("  - Drag grabbed object: move mouse / finger");
            return sb.ToString();
        }

        #endregion

        private void LoadObstacles()
        {
            //for (int i = 0; i < 5; ++i)
            //{
            //    // _obstacles[i] = BodyFactory.CreateRectangle(World, 5f, 1f, 1f);
            //    try
            //    {
            //        _obstacles[i] = BodyFactory.CreateEllipse(World, 5f, 1f, 20, 1f);
            //    }
            //    catch (Exception)
            //    {
                    
            //       // throw;
            //    }
                
            //    _obstacles[i].IsStatic = true;
            //    _obstacles[i].Restitution = 1f;
            //    _obstacles[i].Friction = 0f;
            //}

            //_obstacles[0].Position = new Vector2(-5f, 9f);
            //_obstacles[1].Position = new Vector2(15f, 6f);
            //_obstacles[2].Position = new Vector2(10f, -3f);
            //_obstacles[3].Position = new Vector2(-10f, -9f);
            //_obstacles[4].Position = new Vector2(-17f, 0f);

            //// create sprite based on body
            //_obstacle = new Sprite(ScreenManager.Assets.TextureFromShape(_obstacles[0].FixtureList[0].Shape,
            //                                                             MaterialType.Dots,
            //                                                             Color.SandyBrown, 0.8f));

            this.gamePad2 = BodyFactory.CreateEllipse(World, 1f, 3f, 20, 1f);
            this.gamePad2.IsStatic = true;
            this.gamePad2.Restitution = 1f;
            this.gamePad2.Friction = 0f;

            this.gamePad2.Position = new Vector2(20f, 0f);


            this.gamePadSprite2 = new Sprite(ScreenManager.Assets.TextureFromShape(this.gamePad2.FixtureList[0].Shape,
                                                                         MaterialType.Dots,
                                                                         Color.SandyBrown, 0.8f));
            
            this.gamePad1 = BodyFactory.CreateEllipse(World, 1f, 3f, 20, 1f);
            this.gamePad1.IsStatic = true;
            this.gamePad1.Restitution = 1f;
            this.gamePad1.Friction = 0f;
                        
            this.gamePad1.Position = new Vector2(-20f, 0f);


            this.gamePadSprite1 = new Sprite(ScreenManager.Assets.TextureFromShape(this.gamePad2.FixtureList[0].Shape,
                                                                         MaterialType.Dots,
                                                                         Color.SandyBrown, 0.8f));

        }

        public override void LoadContent()
        {
            base.LoadContent();

            World.Gravity = Vector2.Zero;

            _border = new Border(World, this, ScreenManager.GraphicsDevice.Viewport);
            

            // Body circle = BodyFactory.CreateCircle(World, 5f, 0f, 1f);

            _rectangle = BodyFactory.CreateCircle(World, 1f, 0f, 1f); //BodyFactory.CreateRectangle(World, 5f, 5f, 1f);
            _rectangle.BodyType = BodyType.Dynamic;

            LoadObstacles();
            

            SetUserAgent(_rectangle, 10f, 0f);
            

            // create sprite based on body
            _rectangleSprite = new Sprite(ScreenManager.Assets.TextureFromShape(_rectangle.FixtureList[0].Shape,
                                                                                MaterialType.Squares,
                                                                                Color.Orange, 1f));
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.SpriteBatch.Begin(0, null, null, null, null, null, Camera.View);
            ScreenManager.SpriteBatch.Draw(_rectangleSprite.Texture, ConvertUnits.ToDisplayUnits(_rectangle.Position),
                                           null,
                                           Color.White, _rectangle.Rotation, _rectangleSprite.Origin, 1f,
                                           SpriteEffects.None, 0f);

            //for (int i = 0; i < 5; ++i)
            //{
            //    ScreenManager.SpriteBatch.Draw(_obstacle.Texture, ConvertUnits.ToDisplayUnits(_obstacles[i].Position),
            //                                   null,
            //                                   Color.White, _obstacles[i].Rotation, _obstacle.Origin, 1f,
            //                                   SpriteEffects.None, 0f);
            //}



            ScreenManager.SpriteBatch.Draw(this.gamePadSprite2.Texture, ConvertUnits.ToDisplayUnits(this.gamePad2.Position),
                                               null,
                                               Color.White, this.gamePad2.Rotation, this.gamePadSprite2.Origin, 1f,
                                               SpriteEffects.None, 0f);

            ScreenManager.SpriteBatch.Draw(this.gamePadSprite1.Texture, ConvertUnits.ToDisplayUnits(this.gamePad1.Position),
                                               null,
                                               Color.White, this.gamePad1.Rotation, this.gamePadSprite1.Origin, 1f,
                                               SpriteEffects.None, 0f);

            ScreenManager.SpriteBatch.End();
            _border.Draw();
            base.Draw(gameTime);
        }

        public override void HandleInput(InputHelper input, GameTime gameTime)
        {
            Keys[] pressedKeys = input.KeyboardState.GetPressedKeys();

            float step = (float)0.75;

            if(pressedKeys.Any(x => x == Keys.Up))
            {
                this.gamePad2.Position = new Vector2((float)(this.gamePad2.Position.X), this.gamePad2.Position.Y - step);
            }
            else if (pressedKeys.Any(x => x == Keys.Down))
            {
                this.gamePad2.Position = new Vector2((float)(this.gamePad2.Position.X), this.gamePad2.Position.Y + step);
            }
            
            if(pressedKeys.Any(x => x == Keys.W))
            {
                this.gamePad1.Position = new Vector2((float)(this.gamePad1.Position.X), this.gamePad1.Position.Y - step);
            }
            else if (pressedKeys.Any(x => x == Keys.S))
            {
                this.gamePad1.Position = new Vector2((float)(this.gamePad1.Position.X), this.gamePad1.Position.Y + step);
            }



            base.HandleInput(input, gameTime);
        }
    }
}