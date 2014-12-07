using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Windows.Forms;
//using System.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TetroXNA
{
    public class GameOverClass
    {
        private string playerName;
        private SpriteFont font;
        private StringInputClass nameClass = new StringInputClass();
        //private Form form = new Form();
        //private Label label = new Label();
       // private TextBox textBox = new TextBox();
       // private Button buttonOk = new Button();
        //private Button buttonCancel = new Button();
        private bool calledInputBox = false;

        public GameOverClass(SpriteFont f)
        {
            font = f;
        }

        public void Update(GameTime gameTime)
        {
            /*if (!calledInputBox)
            {
                form.Text = "Player's Name";
                label.Text = "What is your name?";
                textBox.Text = "";

                buttonOk.Text = "OK";
                buttonCancel.Text = "Cancel";
                buttonOk.DialogResult = DialogResult.OK;
                buttonCancel.DialogResult = DialogResult.Cancel;

                label.SetBounds(9, 20, 372, 13);
                textBox.SetBounds(12, 36, 372, 20);
                buttonOk.SetBounds(228, 72, 75, 23);
                buttonCancel.SetBounds(309, 72, 75, 23);

                label.AutoSize = true;
                textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
                buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
                buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

                form.ClientSize = new Size(396, 107);
                form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
                form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
                form.FormBorderStyle = FormBorderStyle.FixedDialog;
                form.StartPosition = FormStartPosition.CenterScreen;
                form.MinimizeBox = false;
                form.MaximizeBox = false;
                form.AcceptButton = buttonOk;
                form.CancelButton = buttonCancel;

                form.Visible = false;
                form.ShowDialog();
                //DialogResult dialogResult = form.ShowDialog();
                playerName = textBox.Text;
                //return dialogResult;

                calledInputBox = true;
            }*/

            nameClass.Update(gameTime);
            System.Console.WriteLine("Update Game Over");  
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            System.Console.WriteLine("Draw Game Over");
            spriteBatch.DrawString(font, "Game Over", new Vector2(150, 50), Color.White);
            spriteBatch.DrawString(font, "Enter your name:", new Vector2(150,200), Color.White);
            spriteBatch.DrawString(font, nameClass.getName(), new Vector2(150, 250), Color.Blue);
        }
    }
}
