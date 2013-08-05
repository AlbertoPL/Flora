#region File Description
//-----------------------------------------------------------------------------
// BackgroundScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
#endregion

namespace GameStateManagement
{
    /// <summary>
    /// Pic->pic->dialogue->how to play->menu
    /// </summary>
    class InstructionScreen : GameScreen
    {
        #region Fields
        System.Timers.Timer timer;
        Boolean isScreenChangeTime = false;
        Boolean isDone = false;
        Boolean dialogueOpen = false;

        int screenNum;
        DialogueBoxScreen dialogue;
        ContentManager c;

        Texture2D bg;
        Texture2D tutorialDrops;
        Texture2D tutorialBucket;

        
        #endregion

        #region Initialization
        


        /// <summary>
        /// Constructor.
        /// </summary>
        public InstructionScreen(int number)
        {
            
            screenNum = number;

            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            timer = new System.Timers.Timer(1500);
            timer.Elapsed += new System.Timers.ElapsedEventHandler(NextPicture);
            
        }

        public void NextPicture(object o,System.Timers.ElapsedEventArgs timedEvent)
        {
            timer.Enabled = false;
            isScreenChangeTime = true;
           
        }

        /// <summary>
        /// Loads graphics content for this screen. The background texture is quite
        /// big, so we use our own local ContentManager to load it. This allows us
        /// to unload before going from the menus into the game itself, wheras if we
        /// used the shared ContentManager provided by the Game class, the content
        /// would remain loaded forever.
        /// </summary>
        public override void LoadContent()
        {
            if (c == null)
                c = new ContentManager(ScreenManager.Game.Services, "Content");
            if (screenNum == 1)
            {
                bg = c.Load<Texture2D>("Background\\Instructions_1");
                timer.Enabled = true;
            }
            else if (screenNum == 2)
            {
                bg = c.Load<Texture2D>("Background\\Instructions_2");
                List<String> listOfMessages = new List<string>();
                listOfMessages.Add("Flora:  Child of the forest...awaken!");

                dialogue = new DialogueBoxScreen(this, listOfMessages, screenNum, true);
                dialogueOpen = true;
            }
            else if (screenNum == 3)
            {
                bg = c.Load<Texture2D>("Background\\Instructions_2");
                timer.Enabled = true;
            }
            else if (screenNum == 4)
            {
                bg = c.Load<Texture2D>("Background\\Instructions_3");
                List<String> listOfMessages = new List<string>();
                listOfMessages.Add("Puck:   Ah! How now spirit?  Whither wander you? ");
                listOfMessages.Add("Flora:  I am in want of thine assistance.  My shepherds of  the forest, the ents, are under siege from the evil fire god. ");
                listOfMessages.Add("Flora:  He has created an army of fire golems to destroy the ents and lay waste to my forest. ");
                listOfMessages.Add("Flora:  You are my only child fast enough to overcome them. ");

                listOfMessages = FormatDialogueList(listOfMessages);

                dialogue = new DialogueBoxScreen(this, listOfMessages, screenNum, false);
                dialogueOpen = true;
            }
            else if (screenNum == 5)
            {
                bg = c.Load<Texture2D>("Background\\Instructions3_3");
                List<String> listOfMessages = new List<string>();
                listOfMessages.Add("Puck:   That's a lot of monsters out there. How can I defend against all of them? ");
                listOfMessages.Add("Flora:  Be patient youngling, for I shall explain. ");
                listOfMessages = FormatDialogueList(listOfMessages);

                dialogue = new DialogueBoxScreen(this, listOfMessages, screenNum, false);
                dialogueOpen = true;
            }
            else if (screenNum == 6)
            {
                bg = c.Load<Texture2D>("Background\\Instructions3_4"); // has no bucket
                List<String> listOfMessages = new List<string>();
                listOfMessages.Add("Flora:   I have summoned for thee, noble Puck, a celestial raincloud.  The water is imbued with the magic thou requires to innervate the ents. ");
                listOfMessages.Add("Puck:   But the rain is here and the flaming beasts are far away, as are the ents. If we wait for them to get rained on, we are already defeated. ");
                listOfMessages = FormatDialogueList(listOfMessages);

                dialogue = new DialogueBoxScreen(this, listOfMessages, screenNum, false);
                dialogueOpen = true;
            }
            else if (screenNum == 7)
            {
                bg = c.Load<Texture2D>("Background\\WaterScreenBackground"); //has a bucket
                tutorialBucket = c.Load<Texture2D>("Sprite\\tutorialBucket");
                List<String> listOfMessages = new List<string>();
                listOfMessages.Add("Flora:  Take the Bucket of Holding. It will allow you to ferry water to the battle of the fire golems and the cradle of the ents.");
                listOfMessages.Add("Puck:   What weapon is this? How can anyone douse an army with a pail so small?");
                listOfMessages.Add("Flora:  You hold no ordinary bucket of water. By my powers it will grow larger, though only on the inside. Position the container with the left and right arrows to catch rain drops. ");
                listOfMessages.Add("Puck:   Is that all there is to it? Certainly this can't be so easy. ");
                listOfMessages = FormatDialogueList(listOfMessages);

                dialogue = new DialogueBoxScreen(this, listOfMessages, screenNum, false);
                dialogueOpen = true;
            }
            else if (screenNum == 8)
            {
                bg = c.Load<Texture2D>("Background\\tutorialWaterScreen"); //has a green drop
                List<String> listOfMessages = new List<string>();
                listOfMessages.Add("Flora:  When you have collected enough water, sprint to the necessary pasture using the [A] and [D] keys to extinguish their army or to nourish our own. Blast your target using the [Space Bar] ");
                listOfMessages.Add("Puck:   Some of these drops have a special hue. What powers do they hold? ");
                listOfMessages = FormatDialogueList(listOfMessages);

                dialogue = new DialogueBoxScreen(this, listOfMessages, screenNum, false);
                dialogueOpen = true;
            }
            else if (screenNum == 9)
            {
                bg = c.Load<Texture2D>("Background\\WaterScreenBackground"); //has a red drop
                tutorialDrops = c.Load<Texture2D>("Sprite\\Tutorial");
                List<String> listOfMessages = new List<string>();
                listOfMessages.Add("Flora:  The magic is very fickle.  Sometimes this raincloud will gift you special colored drops.  The red ones douse fire quicker and the green ones grow ents faster.");
                listOfMessages.Add("Flora:  But be warned:  These drops have terrible consequences shouldst thou use them incorrectly.");
                listOfMessages.Add("Puck:   And what of the one that shines like a rainbow? What does it mean? ");
                listOfMessages.Add("Flora:  A mystery for you to uncover. ");
                listOfMessages = FormatDialogueList(listOfMessages);

                dialogue = new DialogueBoxScreen(this, listOfMessages, screenNum, false);
                dialogueOpen = true;
            }
            else if (screenNum == 10)
            {
                bg = c.Load<Texture2D>("Background\\Instructions_3");
                List<String> listOfMessages = new List<string>();
                listOfMessages.Add("Flora:  While the fire is in check, focus on nourishing the ents.");
                listOfMessages.Add("Flora:  You must water each sapling enough times to transform it into an ent, which will create more saplings around itself for you to water.");
                listOfMessages.Add("Flora:  But don't neglect the fire, lest it blaze out of control. Once the ents are sufficiently numerous to fend for themselves, you must move onto the next area.");
                listOfMessages.Add("Flora:  Dost thou comprehend?");
                listOfMessages.Add("Puck:   Catch raindrops, douse fires, and water plants? It's as easy as searching for flowers. ");
                listOfMessages.Add("Flora:   Now make haste! Journey to the forest to amend this crisis and save thine brothers and sisters. I shall be watching over thou. ");
                listOfMessages = FormatDialogueList(listOfMessages);

                dialogue = new DialogueBoxScreen(this, listOfMessages, screenNum, false);
                dialogueOpen = true;
            }
 /*           else if (screenNum == 11)
            {
                bg = c.Load<Texture2D>("Background\\ForestScreenBackground");
                List<String> listOfMessages = new List<string>();
                listOfMessages.Add("Puck:   ...ok, what's next? ");
                listOfMessages.Add("Flora:  While the fire is in check, you should use the water to nourish the ents; once you grow enough they can take down the fire golem and save this part of the forest. ");
                listOfMessages.Add("Puck:   Is there more bouncing water? ");
                listOfMessages.Add("Flora:  Yes. You must water each seed enough times to turn it into an ent, which will create more seeds for you to water. ");
                listOfMessages.Add("Flora:  Once there are enough ents, they will stomp out the rest of the remaining fire golems as well. ");
                listOfMessages = FormatDialogueList(listOfMessages); 
                
                dialogue = new DialogueBoxScreen(this, listOfMessages, screenNum, false);
                dialogueOpen = true;
            }
            else if (screenNum == 12)
            {
                bg = c.Load<Texture2D>("Background\\Instructions_3");
                List<String> listOfMessages = new List<string>();
                listOfMessages.Add("Flora:  Once you save this area of the forest, you will need to save the rest of it. The fire is weakest here, it will get stronger as you advance. ");
                listOfMessages.Add("Flora:  However, I will do my best to assist you with these more difficult enemies. ");
                listOfMessages.Add("Flora:  Dost thou comprehend? ");
                listOfMessages = FormatDialogueList(listOfMessages); 
                
                dialogue = new DialogueBoxScreen(this, listOfMessages, screenNum, false);
                dialogueOpen = true;
            }
            else if (screenNum == 13)
            {
                bg = c.Load<Texture2D>("Background\\Instructions_3");
                List<String> listOfMessages = new List<string>();
                listOfMessages.Add("Puck:   It's not that complicated, I can handle it. Collect water, kill fire, water ents. ");
                listOfMessages.Add("Puck:   Are you sure it has to be a bucket? ");
                listOfMessages.Add("Flora:  PUCK, GO! ");
                listOfMessages = FormatDialogueList(listOfMessages); 
                
                dialogue = new DialogueBoxScreen(this, listOfMessages, screenNum, false);
                dialogueOpen = true;
            }*/
            else
            {
                bg = c.Load<Texture2D>("Background\\Instructions_3");
                isDone = true;
            }
            
        }

        private List<string> FormatDialogueList(List<string> theDialogues)
        {
            int lineLength = 65;
            List<string> toReturn = new List<string>();
            for (int i = 0; i < theDialogues.Count; i++)
            {
                String currentDialogue = "";
                while (theDialogues[i].Length > lineLength) // for each part of the string that's too long
                {
                    //Grab part of it, remove that part from the original string to continue working with
                    currentDialogue += theDialogues[i].Substring(0, lineLength);
                    theDialogues[i] = theDialogues[i].Remove(0, lineLength);
                    //If in the middle of a word, grab the rest of the word
                    if (theDialogues[i].Contains(" "))
                    {
                        currentDialogue += theDialogues[i].Substring(0, theDialogues[i].IndexOf(' '));
                        theDialogues[i] = theDialogues[i].Remove(0, theDialogues[i].IndexOf(' '));
                    }
                    //Enter a new line if necessary
                    if (theDialogues[i].Length > 0)
                        currentDialogue += "\n";
                }
                //Add the leftovers of any line not big enough
                if (theDialogues[i].Length > 0)
                {
                    currentDialogue += theDialogues[i];
                }
                toReturn.Add(currentDialogue);
            }
            return toReturn;
        }

        /// <summary>
        /// Unloads graphics content for this screen.
        /// </summary>
        public override void UnloadContent()
        {
           c.Unload();
        }


        #endregion

        #region Update and Draw


        /// <summary>
        /// Updates the background screen. Unlike most screens, this should not
        /// transition off even if it has been covered by another screen: it is
        /// supposed to be covered, after all! This overload forces the
        /// coveredByOtherScreen parameter to false in order to stop the base
        /// Update method wanting to transition off.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);
           
            if (isDone)
            {
                isDone = false;
                LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(), new MainMenuScreen());
                this.ExitScreen();
            }
            else if (isScreenChangeTime)
            {
                isScreenChangeTime = false;
                ScreenManager.AddScreen(new InstructionScreen(++screenNum), PlayerIndex.One);
                this.ExitScreen();
            }
            else if (dialogueOpen)
            {
                dialogueOpen = false;
                ScreenManager.AddScreen(dialogue, PlayerIndex.One);
            }
        }


        /// <summary>
        /// Draws the background screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            Rectangle viewport = Resolution.GetVirtualResolution();
            Rectangle fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);
            Rectangle position = new Rectangle(viewport.Width / 2 - bg.Width / 2, viewport.Height / 2 - bg.Height / 2, bg.Width, bg.Height);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Resolution.getTransformationMatrix());

            ScreenManager.GraphicsDevice.Clear(Color.Black);
            spriteBatch.Draw(bg, fullscreen,
                                 Color.White);
            if (tutorialDrops != null)
            {
                Rectangle dropPosition = new Rectangle(viewport.Width / 2 - tutorialDrops.Width / 2, 0, tutorialDrops.Width, tutorialDrops.Height);
                spriteBatch.Draw(tutorialDrops, dropPosition,
                                 new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));
            }

            else if (tutorialBucket != null)
            {
                spriteBatch.Draw(tutorialBucket, fullscreen,
                                 new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));
            }
            spriteBatch.End();
        }


        #endregion
    }
}
