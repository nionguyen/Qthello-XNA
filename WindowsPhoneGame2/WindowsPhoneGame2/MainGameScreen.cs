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
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System.Collections.Generic;
using System.Threading;
#endregion

namespace WindowsPhoneGame2
{
    class MainGameScreen : GameScreen
    {
        Texture2D backgroundTexture;
        Texture2D helpTexture;
        Texture2D chessBlue;
        Texture2D chessPink;
        Texture2D scoreBlue;
        Texture2D scorePink;
        Texture2D musicOff;
        Texture2D noMoveTexture;
        Texture2D pinkWin;
        Texture2D blueWin;
        Texture2D playerWin;
        Texture2D computerWin;
        Texture2D gameDraw;
        Texture2D gameMode;

        SpriteFont spriteFont;

        bool m_isHelpClicked;
        bool m_isNoMoreMove;
        bool m_isMusicOn = true;
        bool m_isChosingMode = true;
        int[,] m_chessArray;
        int m_gameState = (int)GameState.GamePause;
        int m_gameMode = -1;
        Rectangle fullscreen;
        Rectangle popup = new Rectangle(170, 97, 460, 280);
        Rectangle chessRect;
        Rectangle scoreBlueRect = new Rectangle(12, 12, 135, 145);
        Rectangle scorePinkRect = new Rectangle(653, 323, 135, 145);
        Rectangle popUpNoMove = new Rectangle(227, 136, 345, 208);
        Rectangle musicButton = new Rectangle(12, 312, 50, 50);
        Rectangle popUpGameResult = new Rectangle(227, 136, 345, 208);
        Rectangle popUpGameMode = new Rectangle(207, 97, 387, 285);
        int m_thisTurn;
        int m_blueNum = 0;
        int m_pinkNum = 0;

        private GestureSample Gestures;



        public MainGameScreen()
        {
            SoundManager.PlaySong(ESong.Background);
        }

        public override void LoadContent()
        {
            base.LoadContent();
            spriteFont = this.screenContent.Load<SpriteFont>("ScoreFont");
            backgroundTexture = screenContent.Load<Texture2D>("Resource/BG_Game");
            helpTexture = screenContent.Load<Texture2D>("Resource/PU_Help");

            chessPink = screenContent.Load<Texture2D>("Resource/G_Chess_Pink");
            chessBlue = screenContent.Load<Texture2D>("Resource/G_Chess_Blue");

            scoreBlue = screenContent.Load<Texture2D>("Resource/G_Score_Blue");
            scorePink = screenContent.Load<Texture2D>("Resource/G_Score_Pink");

            musicOff = screenContent.Load<Texture2D>("Resource/SBT_MusicOff");
            noMoveTexture = screenContent.Load<Texture2D>("Resource/PU_NoMove");

            pinkWin = screenContent.Load<Texture2D>("Resource/PU_Pink_Win");
            blueWin = screenContent.Load<Texture2D>("Resource/PU_Blue_Win");
            playerWin = screenContent.Load<Texture2D>("Resource/PU_You_Win");
            computerWin = screenContent.Load<Texture2D>("Resource/PU_You_Lose");
            gameDraw = screenContent.Load<Texture2D>("Resource/PU_Draw_Game");

            gameMode = screenContent.Load<Texture2D>("Resource/PU_Mode");

            m_chessArray = new int[8, 8];
            m_chessArray[3, 3] = 1;
            m_chessArray[3, 4] = -1;
            m_chessArray[4, 3] = -1;
            m_chessArray[4, 4] = 1;

            m_thisTurn = (int)GameTurn.Player_Pink;
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }




        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            if (Global.Back == true)
            {
                SoundManager.PlaySound(ESound.SelectButton);
                MenuScreen menuScreen = new MenuScreen();
                ScreenManager.AddScreen(menuScreen);
                this.ExitScreen();
                return;
            }
            if (TouchPanel.IsGestureAvailable)
            {
                Gestures = TouchPanel.ReadGesture();
            }
            else
            {
                Gestures = new GestureSample();
            }

            if (m_gameMode == (int)GameMode.AI)
            {
                if (m_thisTurn == (int)GameTurn.Player_Blue)
                {
                    //AI
                    MinimaxAI();
                }
            }
            if (Gestures.GestureType == GestureType.Tap)
            {
                Rectangle replayButton = new Rectangle(12, 365, 50, 50);
                Rectangle menuButton = new Rectangle(12, 418, 50, 50);
                Rectangle helpButton = new Rectangle(12, 259, 50, 50);
                Rectangle PvP_mode = new Rectangle(254, 246, 292, 120);
                Rectangle AI_mode = new Rectangle(254, 114, 292, 120);
                Rectangle boardArea = new Rectangle(188, 24, 424, 424);
                Point posTap = new Point((int)Gestures.Position.X, (int)Gestures.Position.Y);

                //////////////////////////////////////////////////////////////

                /// Dang xuat hien bang no move
                /// 
                if (helpButton.Contains(posTap))
                {
                    SoundManager.PlaySound(ESound.SelectButton);
                    
                    m_isHelpClicked = !m_isHelpClicked;
                    if (m_isHelpClicked)
                        m_gameState = (int)GameState.GamePause;
                    else
                        m_gameState = (int)GameState.GameOn;
                    return;
                }

                if (m_isHelpClicked)
                {
                    SoundManager.PlaySound(ESound.SelectButton);
                    m_isHelpClicked = false;
                    m_gameState = (int)GameState.GameOn;
                    return;
                }

                if (m_isNoMoreMove && popUpNoMove.Contains(posTap))
                {
                    SoundManager.PlaySound(ESound.SelectButton);
                    SwitchTurn();
                    m_gameState = (int)GameState.GameOn;
                    m_isNoMoreMove = false;
                    return;
                }

                /// Dang xuat hien bang result
                if (m_gameState == (int)GameState.GameEnd && popUpGameResult.Contains(posTap))
                {
                    //m_gameState = (int)GameState.GameOn;
                    SoundManager.PlaySound(ESound.SelectButton);
                    ResetBoard();
                    return;
                }

                if (m_isChosingMode == true)
                {
                    if (PvP_mode.Contains(posTap))
                    {
                        m_gameMode = (int)GameMode.PvP;
                        m_gameState = (int)GameState.GameOn;
                        m_isChosingMode = false;
                        return;
                    }
                    if (AI_mode.Contains(posTap))
                    {
                        m_gameMode = (int)GameMode.AI;
                        m_gameState = (int)GameState.GameOn;
                        m_isChosingMode = false;
                        return;
                    }

                }
                if (replayButton.Contains(posTap))
                {
                    SoundManager.PlaySound(ESound.SelectButton);
                    ResetBoard();
                    return;
                }

                if (menuButton.Contains(posTap))
                {
                    SoundManager.PlaySound(ESound.SelectButton);
                    MenuScreen menuScreen = new MenuScreen();
                    ScreenManager.AddScreen(menuScreen);
                    this.ExitScreen();
                    return;
                }

                
                if (musicButton.Contains(posTap))
                {
                    SoundManager.PlaySound(ESound.SelectButton);
                    if (Global.SOUND == true)
                    {
                        Global.SOUND = false;
                        SoundManager.StopSongs();
                    }
                    else
                    {
                        Global.SOUND = true;
                        SoundManager.PlaySong(ESong.Background);
                    }
                    m_isMusicOn = !m_isMusicOn;
                    return;
                }

                /// Neu game k pause
                if (m_gameMode == (int)GameMode.PvP)
                {
                    if (boardArea.Contains(posTap) && m_gameState == (int)GameState.GameOn)
                    {
                        
                        ChessDown(posTap);
                        return;
                    }
                }
                if (m_gameMode == (int)GameMode.AI && m_thisTurn == (int)GameTurn.Player_Pink)
                {

                    if (boardArea.Contains(posTap) && m_gameState == (int)GameState.GameOn)
                    {
                        
                        ChessDown(posTap);
                        return;
                    }


                }




                

            }

            base.Update(gameTime, otherScreenHasFocus, false);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);

            ////////////////////////////////////////////////////////////////
            spriteBatch.Begin();

            // Ve background
            spriteBatch.Draw(backgroundTexture, fullscreen, new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));

            if (m_isChosingMode == true)
            {
                spriteBatch.Draw(gameMode, popUpGameMode, new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));
                
            }
            else
            {

                // Ve co
                DrawChessArray(spriteBatch);

                // Ve diem
                DrawScore(spriteBatch);

                // Ve Help pop up
                if (m_isHelpClicked)
                    spriteBatch.Draw(helpTexture, popup, new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));

                // Ve no move available
                if (m_isNoMoreMove)
                {
                    spriteBatch.Draw(noMoveTexture, popUpNoMove, new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));

                }

                if (m_gameState == (int)GameState.GameEnd)
                {
                    if (m_gameMode == (int)GameMode.PvP)
                    {
                        if (m_pinkNum > m_blueNum)
                        {
                            //Ve pink Win
                            spriteBatch.Draw(pinkWin, popUpGameResult, new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));
                        }
                        else if (m_pinkNum < m_blueNum)
                        {
                            //Ve blua win)
                            spriteBatch.Draw(blueWin, popUpGameResult, new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));
                        }
                        else
                        {
                            //Ve hoa nhau
                            spriteBatch.Draw(gameDraw, popUpGameResult, new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));
                        }
                    }
                    if (m_gameMode == (int)GameMode.AI)
                    {
                        if (m_pinkNum > m_blueNum)
                        {
                            //Ve pink Win
                            spriteBatch.Draw(playerWin, popUpGameResult, new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));
                        }
                        else if (m_pinkNum < m_blueNum)
                        {
                            //Ve blua win)
                            spriteBatch.Draw(computerWin, popUpGameResult, new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));
                        }
                        else
                        {
                            //Ve hoa nhau
                            spriteBatch.Draw(gameDraw, popUpGameResult, new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));
                        }
                    }

                }
            }
            // Ve Music button
            if (!m_isMusicOn)
                spriteBatch.Draw(musicOff, musicButton, new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));

            /////////////////////////////////////////////////////////////////
            spriteBatch.End();
        }

        public void ChessDown(Point posTap)
        {
            Point temp = posTap;
            temp.X -= 184;
            temp.Y -= 24;

            int col = temp.X / 53;
            int row = temp.Y / 53;

            if (m_chessArray[col, row] == 0)
            {

                if (IsMoveValid(col, row))
                {
                    m_chessArray[col, row] = m_thisTurn;
                    SoundManager.PlaySound(ESound.Success);
                    FlipChess(col, row);
                    if (m_pinkNum + m_blueNum == 64)
                    {
                        m_gameState = (int)GameState.GameEnd;
                        SoundManager.PlaySound(ESound.Win);
                    }
                    else
                    {
                        SwitchTurn();

                        // Doi phuong k cos luot di
                        if (!HasPoisibleMove())
                        {
                            //check minh cung k co luot di
                            SwitchTurn();
                            if (HasPoisibleMove() == false)
                            {
                                m_gameState = (int)GameState.GameEnd;
                                SoundManager.PlaySound(ESound.Win);
                            }
                            else//co luot di --> doi lai minh --> thong bao het duong
                            {
                                SwitchTurn();


                                m_isNoMoreMove = true;
                                m_gameState = (int)GameState.GamePause;
                                //SwitchTurn();
                            }
                        }


                    }
                }
            }
        }
        public void FlipChess(int col, int row)
        {
            bool isFlipDone = false;
            int m, n;
            for (int i = row - 1; i <= row + 1; i++)
                for (int j = col - 1; j <= col + 1; j++)
                {
                    if (i < 8 && j < 8 && i >= 0 && j >= 0 && m_chessArray[j, i] == -m_thisTurn)
                    {
                        for (m = i, n = j; ; m += (i - row), n += (j - col))
                        {
                            if (isFlipDone)
                            {
                                isFlipDone = false;
                                break;
                            }
                            if (m >= 8 || n >= 8 || m < 0 || n < 0 || m_chessArray[n, m] == 0)
                                break;
                            if (m_chessArray[n, m] == m_thisTurn)
                            {
                                for (int q = i, p = j; ; q += (i - row), p += (j - col))
                                {
                                    if (q == m && p == n)
                                    {
                                        isFlipDone = true;
                                        break;
                                    }

                                    m_chessArray[p, q] *= -1;
                                    //fxChessDown->AddParticle(q + (float)BOARD_UNIT / 2, 0.3, p + (float)BOARD_UNIT / 2);
                                }
                            }
                        }
                    }
                }
        }
        public bool IsMoveValid(int col, int row)
        {
            int m, n;
            //row = m_selectionPos.X;
            //col = m_selectionPos.Z;
            if (m_chessArray[col, row] != 0)
                return false;
            for (int i = row - 1; i <= row + 1; i++)
                for (int j = col - 1; j <= col + 1; j++)
                {
                    if (i < 8 && j < 8 && i >= 0 && j >= 0)
                    {
                        if (m_chessArray[j, i] == -m_thisTurn)
                        {
                            for (m = i, n = j; ; m += (i - row), n += (j - col))
                            {
                                if (m >= 8 || n >= 8 || m < 0 || n < 0)
                                    break;
                                if (m_chessArray[n, m] == 0)
                                    break;
                                if (m_chessArray[n, m] == m_thisTurn)
                                    return true;
                            }
                        }
                    }
                }
            return false;
        }

        public void SwitchTurn()
        {
            m_thisTurn *= -1;
        }
        public bool HasPoisibleMove()
        {
            int i, j;
            for (i = 0; i < 8; i++)
            {
                for (j = 0; j < 8; j++)
                {
                    if (IsMoveValid(i, j))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void MinimaxAI()
        {

            int i, j, m, n;
            int maxFlip = 0;
            int minFlip = 64;//minChessCanFlipNextTurn
            int choseCol = -1;
            int choseRow = -1;
            //choseRow = -1;
            //choseCol = -1;
            int countFlip = -1;//chessCanFlipThisMove

            for (i = 0; i < 8; i++)// i = row
            {
                for (j = 0; j < 8; j++)
                {
                    /*m_selectionPos.X = i;
                    m_selectionPos.Z = j;*/
                    if (IsMoveValid(j, i))
                    {
                        //i,j la bien luu lai vi tri co
                        m_chessArray[j, i] = m_thisTurn;
                        SwitchTurn();
                        for (m = 0; m < 8; m++)
                        {
                            for (n = 0; n < 8; n++)
                            {
                                /*m_selectionPos.X = m;
                                m_selectionPos.Z = n;*/
                                if (IsMoveValid(n, m))
                                {
                                    countFlip = NumberOfChessCanFlip(n, m);
                                    if (countFlip > maxFlip)
                                    {
                                        maxFlip = countFlip;
                                    }
                                }

                            }
                        }
                        if (maxFlip < minFlip)
                        {
                            minFlip = maxFlip;
                            choseRow = i;
                            choseCol = j;
                        }
                        m_chessArray[j, i] = 0;
                        SwitchTurn();
                    }
                }
            }
            if (choseRow == -1 || choseCol == -1)
                SwitchTurn();
            else
            {
                //m_selectionPos.X = choseRow;
                //m_selectionPos.Z = choseCol;
                //ChessDown(new Point(choseCol, choseRow));
                m_chessArray[choseCol, choseRow] = (int)GameTurn.Player_Blue;
                
                SoundManager.PlaySound(ESound.Success);
                FlipChess(choseCol, choseRow);

                if (m_pinkNum + m_blueNum == 64)
                {
                    m_gameState = (int)GameState.GameEnd;
                    if (m_pinkNum > m_blueNum)
                    {
                        SoundManager.PlaySound(ESound.Win);
                    }
                    else
                    {
                        SoundManager.PlaySound(ESound.Lose);
                    }

                }
                else
                {
                    SwitchTurn();

                    // Doi turn cho player
                    if (!HasPoisibleMove())//player k co luot
                    {


                        //check AI cung k co luot di --> End
                        SwitchTurn();
                        if (HasPoisibleMove() == false)
                        {
                            m_gameState = (int)GameState.GameEnd;
                            if (m_pinkNum > m_blueNum)
                            {
                                SoundManager.PlaySound(ESound.Win);
                            }
                            else
                            {
                                SoundManager.PlaySound(ESound.Lose);
                            }
                        }
                        else//AI co luot di --> doi lai player --> thong bao het duong
                        {
                            SwitchTurn();// doi sang cho nguoi choi


                            m_isNoMoreMove = true;
                            m_gameState = (int)GameState.GamePause;
                            //SwitchTurn();
                        }
                    }


                }
                //SwitchTurn();
            }

        }

        public int NumberOfChessCanFlip(int col, int row)
        {
            bool isCountDone = false;
            int m, n, counter = 0;
            for (int i = row - 1; i <= row + 1; i++)
                for (int j = col - 1; j <= col + 1; j++)
                {
                    if (i < 8 && j < 8 && i >= 0 && j >= 0 && m_chessArray[j, i] == -m_thisTurn)
                    {
                        for (m = i, n = j; ; m += (i - row), n += (j - col))
                        {
                            if (isCountDone)
                            {
                                isCountDone = false;
                                break;
                            }
                            if (m >= 8 || n >= 8 || m < 0 || n < 0 || m_chessArray[n, m] == 0)
                                break;
                            if (m_chessArray[n, m] == m_thisTurn)
                            {
                                for (int q = i, p = j; ; q += (i - row), p += (j - col))
                                {
                                    if (q == m && p == n)
                                    {
                                        isCountDone = true;
                                        break;
                                    }
                                    if (m_chessArray[p, q] == -m_thisTurn)
                                        counter++;
                                }
                            }
                        }
                    }
                }
            return counter;
        }

        public void DrawChessArray(SpriteBatch spriteBatch)
        {
            m_pinkNum = m_blueNum = 0;
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    if (m_chessArray[i, j] != 0)
                    {

                        chessRect = new Rectangle(189 + i * 54, 29 + j * 54, 44, 44);
                        if (m_chessArray[i, j] == 1)
                        {
                            spriteBatch.Draw(chessBlue, chessRect, new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));
                            m_blueNum++;
                        }
                        else
                        {
                            spriteBatch.Draw(chessPink, chessRect, new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));
                            m_pinkNum++;
                        }
                    }
                }


        }

        public void DrawScore(SpriteBatch spriteBatch)
        {
            if (m_thisTurn == (int)GameTurn.Player_Blue)
            {
                spriteBatch.Draw(scoreBlue, scoreBlueRect, new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));
                spriteBatch.Draw(scorePink, scorePinkRect, new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha) * 0.3f);
            }
            else
            {
                spriteBatch.Draw(scoreBlue, scoreBlueRect, new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha) * 0.3f);
                spriteBatch.Draw(scorePink, scorePinkRect, new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));
            }

            spriteBatch.DrawString(spriteFont, m_blueNum.ToString(), new Vector2(35, 25), Color.White);
            spriteBatch.DrawString(spriteFont, m_pinkNum.ToString(), new Vector2(705, 380), Color.White);
        }


        public void ResetBoard()
        {
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    m_chessArray[i, j] = 0;
                }

            m_chessArray[3, 3] = 1;
            m_chessArray[3, 4] = -1;
            m_chessArray[4, 3] = -1;
            m_chessArray[4, 4] = 1;
            m_thisTurn = (int)GameTurn.Player_Pink;
            m_isChosingMode = true;
            m_gameState = (int)GameState.GamePause;
        }
    }
}
