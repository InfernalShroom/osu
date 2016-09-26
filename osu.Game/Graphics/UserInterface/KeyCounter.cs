﻿//Copyright (c) 2007-2016 ppy Pty Ltd <contact@ppy.sh>.
//Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using OpenTK;
using OpenTK.Graphics;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;

namespace osu.Game.Graphics.UserInterface
{
    public abstract class KeyCounter : Container
    {
        private Sprite buttonSprite;
        private Sprite glowSprite;
        private Container textLayer;
        private SpriteText countSpriteText;

        public override string Name { get; }
        public bool IsCounting { get; set; }
        public int Count { get; private set; }

        private bool isLit;
        public bool IsLit
        {
            get { return isLit; }
            protected set
            {
                if (isLit != value)
                {
                    isLit = value;
                    UpdateGlowSprite();
                    if (value && IsCounting)
                        IncreaseCount();
                }
            }
        }

        //further: change default values here and in KeyCounterCollection if needed, instead of passing them in every constructor
        public Color4 KeyDownTextColor { get; set; } = Color4.DarkGray;
        public Color4 KeyUpTextColor { get; set; } = Color4.White;
        public int FadeTime { get; set; } = 0;

        protected KeyCounter(string name)
        {
            Name = name;
        }

        public override void Load()
        {
            base.Load();
            Children = new Drawable[]
            {
                buttonSprite = new Sprite
                {
                    Texture = Game.Textures.Get(@"KeyCounter/key-up"),
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                },
                glowSprite = new Sprite
                {
                    Texture = Game.Textures.Get(@"KeyCounter/key-glow"),
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Alpha = 0
                },
                textLayer = new Container
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Children = new Drawable[]
                    {
                        new SpriteText
                        {
                            Text = Name,
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Position = new Vector2(0, -buttonSprite.Height / 4),
                            Colour = KeyUpTextColor
                        },
                        countSpriteText = new SpriteText
                        {
                            Text = Count.ToString(@"#,0"),
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Position = new Vector2(0, buttonSprite.Height / 4),
                            Colour = KeyUpTextColor
                        }
                    }
                }
            };
            //Set this manually because an element with Alpha=0 won't take it size to AutoSizeContainer,
            //so the size can be changing between buttonSprite and glowSprite.
            Height = buttonSprite.Height;
            Width = buttonSprite.Width;
        }

        private void UpdateGlowSprite()
        {
            if (IsLit)
            {
                glowSprite.FadeIn(FadeTime);
                textLayer.FadeColour(KeyDownTextColor, FadeTime);
            }
            else
            {
                glowSprite.FadeOut();
                textLayer.FadeColour(KeyUpTextColor, FadeTime);
            }
        }

        private void IncreaseCount()
        {
            Count++;
            countSpriteText.Text = Count.ToString(@"#,0");
        }
    }
}
