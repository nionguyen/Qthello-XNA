using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace WindowsPhoneGame2
{
    public class Card
    {
        int TILE = 70;
        int EXTRATILE = 76;
        Vector2 EXTRAPOS = new Vector2(18, 88);
        
        public CardState m_state = CardState.Down;
        public CardType m_type;

        Texture2D m_spriteDown;
        Texture2D m_spriteUp;

        Vector2 m_position;
        int m_indexI,indexJ;
        Rectangle rect;

        public Card(ContentManager content, int _indexI, int _indexJ,CardType _type)
        {
            m_type = _type;
            m_indexI = _indexI;
            indexJ = _indexJ;

            m_position = new Vector2(indexJ * EXTRATILE + EXTRAPOS.X, m_indexI * EXTRATILE + EXTRAPOS.Y);
            rect = new Rectangle((int)m_position.X,(int)m_position.Y, TILE, TILE);

            InitSprite(content);
        }

        private void InitSprite(ContentManager content)
        {
            m_spriteDown = content.Load<Texture2D>("defaultCard");
            switch (m_type)
            {
                case CardType.Type01:
                    m_spriteUp = content.Load<Texture2D>("card01");
                    break;
                case CardType.Type02:
                    m_spriteUp = content.Load<Texture2D>("card02");
                    break;
                case CardType.Type03:
                    m_spriteUp = content.Load<Texture2D>("card03");
                    break;
                case CardType.Type04:
                    m_spriteUp = content.Load<Texture2D>("card04");
                    break;
                case CardType.Type05:
                    m_spriteUp = content.Load<Texture2D>("card05");
                    break;
                case CardType.Type06:
                    m_spriteUp = content.Load<Texture2D>("card06");
                    break;
                case CardType.Type07:
                    m_spriteUp = content.Load<Texture2D>("card07");
                    break;
                case CardType.Type08:
                    m_spriteUp = content.Load<Texture2D>("card08");
                    break;
                case CardType.Type09:
                    m_spriteUp = content.Load<Texture2D>("card09");
                    break;
                case CardType.Type10:
                    m_spriteUp = content.Load<Texture2D>("card10");
                    break;
                case CardType.Type11:
                    m_spriteUp = content.Load<Texture2D>("card11");
                    break;
                case CardType.Type12:
                    m_spriteUp = content.Load<Texture2D>("card12");
                    break;
            }
        }

        public bool IsContain(Vector2 posTap)
        {
            Point tap = new Point((int)posTap.X,(int)posTap.Y);
            return rect.Contains(tap);
        }

        public void ChangeState()
        {
            if (m_state == CardState.Down)
            {
                m_state = CardState.Up;
                return;
            }
            if (m_state == CardState.Up)
            {
                m_state = CardState.Down;
                return;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(m_state == CardState.Down)
                spriteBatch.Draw(m_spriteDown, m_position, Color.White);
            if (m_state == CardState.Up || m_state == CardState.Done) 
                spriteBatch.Draw(m_spriteUp, m_position, Color.White);
        }
    }
}
