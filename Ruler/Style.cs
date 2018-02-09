using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenRuler
{
    class Style
    {
        StyleKind kind;
        public Style(StyleKind styleKind)
        {
            this.kind = styleKind;
        }
        public Color GetBack()
        {
            switch (kind)
            {
                case StyleKind.Black:
                    return Color.FromArgb(48, 48, 48);
                case StyleKind.White:
                    return Color.FromArgb(255, 255, 255);
                case StyleKind.Blue:
                    return Color.FromArgb(0, 122, 204);
                default:
                    throw new Exception("unhandled");
            }

        }
        public Color GetFore()
        {
            switch (kind)
            {
                case StyleKind.Black:
                    return Color.White;
                case StyleKind.White:
                    return Color.Black;
                case StyleKind.Blue:
                    return Color.White;
                default:
                    throw new Exception("unhandled");
            }

        }
        public Color GetLightFore()
        {
            switch (kind)
            {
                case StyleKind.Black:
                    return Color.FromArgb(224, 224, 224);
                case StyleKind.White:
                    return Color.FromArgb(32, 32, 32);
                case StyleKind.Blue:
                    return Color.FromArgb(224, 224, 224);
                default:
                    throw new Exception("unhandled");
            }
        }
        public Color GetUltraLightFore()
        {
            switch (kind)
            {
                case StyleKind.Black:
                    return Color.FromArgb(192, 192, 192);
                case StyleKind.White:
                    return Color.FromArgb(64, 64, 64);
                case StyleKind.Blue:
                    return Color.FromArgb(192, 192, 192);
                default:
                    throw new Exception("unhandled");
            }
        }
        public Color GetFormBorder()
        {
            switch (kind)
            {
                case StyleKind.Black:
                    return Color.FromArgb(128, 128, 128);
                case StyleKind.White:
                    return Color.FromArgb(192, 192, 192);
                case StyleKind.Blue:
                    return Color.FromArgb(202, 81, 0);
                default:
                    throw new Exception("unhandled");
            }
        }
    }
    enum StyleKind
    {
        White,
        Black,
        Blue
    }
}
