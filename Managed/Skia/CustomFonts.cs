using SkiaSharp;
using System.Reflection;

namespace LoneWebRadar.Managed.Skia
{
    internal static class CustomFonts
    {
        /// <summary>
        /// Neo Sans Std Regular
        /// </summary>
        public static SKTypeface SKFontFamilyRegular { get; }
        /// <summary>
        /// Neo Sans Std Bold
        /// </summary>
        public static SKTypeface SKFontFamilyBold { get; }
        /// <summary>
        /// Neo Sans Std Italic
        /// </summary>
        public static SKTypeface SKFontFamilyItalic { get; }
        /// <summary>
        /// Neo Sans Std Medium
        /// </summary>
        public static SKTypeface SKFontFamilyMedium { get; }

        static CustomFonts()
        {
            try
            {
                byte[] fontFamilyRegular, fontFamilyBold, fontFamilyItalic, fontFamilyMedium;
                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("LoneWebRadar.Resources.NeoSansStdRegular.otf"))
                {
                    fontFamilyRegular = new byte[stream.Length];
                    stream.Read(fontFamilyRegular);
                }
                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("LoneWebRadar.Resources.NeoSansStdBold.otf"))
                {
                    fontFamilyBold = new byte[stream.Length];
                    stream.Read(fontFamilyBold);
                }
                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("LoneWebRadar.Resources.NeoSansStdItalic.otf"))
                {
                    fontFamilyItalic = new byte[stream.Length];
                    stream.Read(fontFamilyItalic);
                }
                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("LoneWebRadar.Resources.NeoSansStdMedium.otf"))
                {
                    fontFamilyMedium = new byte[stream.Length];
                    stream.Read(fontFamilyMedium);
                }
                SKFontFamilyRegular = SKTypeface.FromStream(new MemoryStream(fontFamilyRegular, false));
                SKFontFamilyBold = SKTypeface.FromStream(new MemoryStream(fontFamilyBold, false));
                SKFontFamilyItalic = SKTypeface.FromStream(new MemoryStream(fontFamilyItalic, false));
                SKFontFamilyMedium = SKTypeface.FromStream(new MemoryStream(fontFamilyMedium, false));
            }
            catch (Exception ex)
            {
                throw new ApplicationException("ERROR Loading Custom Fonts!", ex);
            }
        }
    }
}
