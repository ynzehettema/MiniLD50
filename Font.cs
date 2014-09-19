using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using MetalLib;
using MetalLib.GameStructure;
using MetalLib.GameWorld;
using MetalLib.Pencil.Gaming;

using Pencil.Gaming;
using Pencil.Gaming.Graphics;
using Pencil.Gaming.MathUtils;
using Pencil.Gaming.Audio;

namespace MiniLD50
{
    public class Text
    {
        public string _Text, Name;
        public Vector2 Position;
        public float Size;
        public bool OriginLeft;

        public Text(string text, string name, Vector2 position, float size, bool originLeft = false)
        {
            Name = name;
            _Text = text;
            Position = position;
            Size = size;
            OriginLeft = originLeft;
        }
    }
    public class Font
    {
        public static List<Text> TextList = new List<Text>();
        private static string fontTexture = "font";

        public static float CharWidth = 24, CharHeight = 30;


        public static void SetFontTexture(string texture)
        {
            fontTexture = texture;
        }

        public static void Draw()
        {
            Texture texture = ContentManager.GetTexture(fontTexture);
            if (texture != null)
            {
                CharHeight = texture.Height / 5f;
                CharWidth = 26;
                foreach (Text t in TextList)
                {
                    int bufferCharPositionX = 0;
                    for (int a = 0; a < t._Text.Length; a++)
                    {
                        char ch = t._Text[a];
                        int Xindex = 0;
                        int Yindex = 0;
                        if (char.IsLetter(ch) && char.IsUpper(ch))
                        {
                            Xindex = (int)ch - 65;
                            Yindex = 0;
                        }
                        else
                            if (char.IsLetter(ch) && char.IsLower(ch))
                            {
                                Xindex = (int)ch - 97;
                                Yindex = 1;
                            }
                            else
                                if (char.IsNumber(ch))
                                {
                                    Xindex = 10 + ((int)ch - 48);
                                    Yindex = 3;
                                }
                                else
                                {
                                    switch (ch)
                                    {
                                        case '!':
                                            Xindex = 0;
                                            Yindex = 3;
                                            break;
                                        case '"':
                                            Xindex = 1;
                                            Yindex = 3;
                                            break;
                                        case '%':
                                            Xindex = 2;
                                            Yindex = 3;
                                            break;
                                        case '&':
                                            Xindex = 3;
                                            Yindex = 3;
                                            break;
                                        case '`':
                                            Xindex = 4;
                                            Yindex = 3;
                                            break;
                                        case '[':
                                            Xindex = 5;
                                            Yindex = 3;
                                            break;
                                        case ']':
                                            Xindex = 6;
                                            Yindex = 3;
                                            break;
                                        case '+':
                                            Xindex = 7;
                                            Yindex = 3;
                                            break;
                                        case '-':
                                            Xindex = 8;
                                            Yindex = 3;
                                            break;
                                        case '/':
                                            Xindex = 9;
                                            Yindex = 3;
                                            break;
                                        case ':':
                                            Xindex = 20;
                                            Yindex = 3;
                                            break;
                                        case '=':
                                            Xindex = 21;
                                            Yindex = 3;
                                            break;
                                        case '<':
                                            Xindex = 22;
                                            Yindex = 3;
                                            break;
                                        case '>':
                                            Xindex = 23;
                                            Yindex = 3;
                                            break;
                                        case '?':
                                            Xindex = 25;
                                            Yindex = 3;
                                            break;

                                        case '@':
                                            Xindex = 0;
                                            Yindex = 4;
                                            break;
                                        case '\\':
                                            Xindex = 1;
                                            Yindex = 4;
                                            break;
                                        case '\'':
                                            Xindex = 2;
                                            Yindex = 4;
                                            break;
                                        case '~':
                                            Xindex = 3;
                                            Yindex = 4;
                                            break;
                                        case '^':
                                            Xindex = 4;
                                            Yindex = 4;
                                            break;
                                        case ',':
                                            Xindex = 5;
                                            Yindex = 4;
                                            break;
                                        case '.':
                                            Xindex = 6;
                                            Yindex = 4;
                                            break;
                                        default:
                                            break;
                                    }
                                }
                        if (ch != ' ')
                        {
                            int charPosX = Xindex, charPosY = Yindex;
                            int texturePosX = charPosX * (int)CharWidth;
                            int texturePosY = charPosY * (int)CharHeight;

                            int screenPosX = 0, screenPosY = 0;
                            for (int y = texturePosY; y < texturePosY + CharHeight; ++y)
                            {
                                screenPosY = (int)((float)(y - texturePosY) * t.Size) + (int)t.Position.Y;
                                for (int x = texturePosX; x < texturePosX + CharWidth; ++x)
                                {
                                    screenPosX = (int)((float)(x - texturePosX) * t.Size) + bufferCharPositionX + (int)t.Position.X;
                                    if (texture.Pixels[x, y] != 4294967295)
                                    ScreenBuffer.SetPixel(screenPosX, screenPosY, texture.Pixels[x, y]);
                                    
                                }
                                
                            }
                            bufferCharPositionX += (int)(CharWidth * t.Size);
                        }
                        else
                        {
                            bufferCharPositionX += (int)(CharWidth * t.Size) / 2;
                        }
                    }

                }
            }
        }

        public static float GetStringLength(string s)
        {
            float length = 0f;
            for (int a = 0; a < s.Length; a++)
            {
                if (s[a] == ' ')
                {
                    length += 0.5f;
                }
                else
                {
                    length++;
                }
            }
            return length;
        }

        public static void AddText(Text text)
        {
            TextList = TextList.Where(x => x.Name != text.Name).ToList();
            TextList.Add(text);
        }

        public static void RemoveText(string name)
        {
            TextList = TextList.Where(x => x.Name != name).ToList();
        }

        public static Text GetText(string name)
        {
            return TextList.First(x => x.Name == name);
        }
    }
}
