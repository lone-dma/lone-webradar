using LoneWebRadar.Managed.Services;
using SkiaSharp;
using System.Net;
using System.Numerics;

namespace LoneWebRadar.Managed
{
    public static class UnicodeSubstitutions
    {
        public static readonly IReadOnlyList<string> L = new List<string>
        {
            "𝐋", "𝑳", "𝖫", "𝓛", "𝕃", "𝗟", "ℒ"
        };

        public static readonly IReadOnlyList<string> o = new List<string>
        {
            "𝐨", "𝑜", "𝗈", "𝑜", "𝕠", "𝗼", "𝑜"
        };

        public static readonly IReadOnlyList<string> n = new List<string>
        {
            "𝐧", "𝑛", "𝗇", "𝓷", "𝕟", "𝗻", "𝓷"
        };

        public static readonly IReadOnlyList<string> e = new List<string>
        {
            "𝐞", "𝑒", "𝖾", "𝓮", "𝕖", "𝗲", "𝓮"
        };

        public static readonly IReadOnlyList<string> apostrophe = new List<string>
        {
            "'", "‘", "’", "‘", "’", "‘", "’"
        };

        public static readonly IReadOnlyList<string> s = new List<string>
        {
            "𝐬", "𝑠", "𝗌", "𝓼", "𝕤", "𝘀", "𝓼"
        };

        public static readonly IReadOnlyList<string> W = new List<string>
        {
            "𝐖", "𝑊", "𝗐", "𝓦", "𝕎", "𝗪", "𝓦"
        };

        public static readonly IReadOnlyList<string> b = new List<string>
        {
            "𝐛", "𝑏", "𝖻", "𝓫", "𝕓", "𝗯", "𝓫"
        };

        public static readonly IReadOnlyList<string> R = new List<string>
        {
            "𝐑", "𝑅", "𝗋", "𝓡", "ℝ", "𝗥", "𝓡"
        };

        public static readonly IReadOnlyList<string> a = new List<string>
        {
            "𝐚", "𝑎", "𝖺", "𝓪", "𝕒", "𝗮", "𝓪"
        };

        public static readonly IReadOnlyList<string> d = new List<string>
        {
            "𝐝", "𝑑", "𝖽", "𝓭", "𝕕", "𝗱", "𝓭"
        };

        public static readonly IReadOnlyList<string> r = new List<string>
        {
            "𝐫", "𝑟", "𝗋", "𝓻", "𝕣", "𝗿", "𝓻"
        };
        public static readonly IReadOnlyList<string> Symbols = new List<string>
        {
            "§", // Section Sign
            "¶", // Pilcrow Sign
            "†", // Dagger
            "‡", // Double Dagger
            "‾", // Overline
            "‽", // Interrobang
            "⁂", // Asterism
            "‰", // Per Mille Sign
            "‱", // Per Ten Thousand Sign
            "№", // Numero Sign
            "⅊", // Property Line
            "‡", // Double Dagger
            "⁒", // Commercial Minus Sign
            "℧", // Ohm Sign
            "™", // Trade Mark Sign
            "℅", // Care Of
            "℮", // Estimated Sign
            "⅍", // Account Of
            "∴", // Therefore
            "∵"  // Because
        };
    }
    public static class Utils
    {
        /// <summary>
        /// Formats an IP Host string for use in a URL.
        /// </summary>
        /// <param name="host">IP/Hostname to check/format.</param>
        /// <returns>Formatted IP, or original string if no formatting is needed.</returns>
        public static string FormatIPForURL(string host)
        {
            if (host is null)
                return null;
            if (IPAddress.TryParse(host, out var ip) && ip.AddressFamily is System.Net.Sockets.AddressFamily.InterNetworkV6)
                return $"[{host}]";
            return host;
        }

        /// <summary>
        /// Get Random Unicode Window Title for this Application.
        /// </summary>
        /// <returns></returns>
        public static string GetRandomTitle()
        {
            string title = "";
            title += AppendChar(UnicodeSubstitutions.L);
            title += AppendChar(UnicodeSubstitutions.o);
            title += AppendChar(UnicodeSubstitutions.n);
            title += AppendChar(UnicodeSubstitutions.e);
            title += AppendChar(UnicodeSubstitutions.apostrophe);
            title += AppendChar(UnicodeSubstitutions.s);
            title += " ";
            title += AppendChar(UnicodeSubstitutions.W);
            title += AppendChar(UnicodeSubstitutions.e);
            title += AppendChar(UnicodeSubstitutions.b);
            title += " ";
            title += AppendChar(UnicodeSubstitutions.R);
            title += AppendChar(UnicodeSubstitutions.a);
            title += AppendChar(UnicodeSubstitutions.d);
            title += AppendChar(UnicodeSubstitutions.a);
            title += AppendChar(UnicodeSubstitutions.r);
            title += "  ";
            title += AppendChar(UnicodeSubstitutions.Symbols);
            title += AppendChar(UnicodeSubstitutions.Symbols);
            title += AppendChar(UnicodeSubstitutions.Symbols);

            return title;

            static string AppendChar(IReadOnlyList<string> list) =>
                list[Random.Shared.Next(list.Count)];
        }
    }
    public static class Extensions
    {
        #region Generic Extensions

        /// <summary>
        /// Converts 'Degrees' to 'Radians'.
        /// </summary>
        public static double ToRadians(this float degrees) =>
            Math.PI / 180 * degrees;
        /// <summary>
        /// Converts 'Radians' to 'Degrees'.
        /// </summary>
        public static double ToDegrees(this float radians) =>
            180 / Math.PI * radians;
        /// <summary>
        /// Converts 'Degrees' to 'Radians'.
        /// </summary>
        public static double ToRadians(this double degrees) =>
            Math.PI / 180 * degrees;
        /// <summary>
        /// Converts 'Radians' to 'Degrees'.
        /// </summary>
        public static double ToDegrees(this double radians) =>
            180 / Math.PI * radians;
        /// <summary>
        /// Converts 'Radians' to 'Degrees'.
        /// </summary>
        public static Vector2 ToDegrees(this Vector2 radians) =>
            180f / (float)Math.PI * radians;
        /// <summary>
        /// Converts 'Radians' to 'Degrees'.
        /// </summary>
        public static Vector3 ToDegrees(this Vector3 radians) =>
            180f / (float)Math.PI * radians;
        /// <summary>
        /// Converts 'Degrees' to 'Radians'.
        /// </summary>
        public static Vector2 ToRadians(this Vector2 degrees) =>
            (float)Math.PI / 180f * degrees;
        /// <summary>
        /// Converts 'Degrees' to 'Radians'.
        /// </summary>
        public static Vector3 ToRadians(this Vector3 degrees) =>
            (float)Math.PI / 180f * degrees;

        #endregion

        #region GUI Extensions
        /// <summary>
        /// Convert Unity Position (X,Y,Z) to an unzoomed Map Position (Z = Height).
        /// </summary>
        /// <param name="vector">Unity Vector3</param>
        /// <param name="map">Current Map</param>
        /// <returns>Unzoomed 3D Map Position (Z = Height).</returns>
        public static Vector3 ToMapPos(this Vector3 vector, RadarService.Map map) =>
            new()
            {
                X = map.ConfigFile.X + vector.X * map.ConfigFile.Scale,
                Y = map.ConfigFile.Y - vector.Z * map.ConfigFile.Scale,
                Z = vector.Y // Height
            };

        /// <summary>
        /// Convert an Unzoomed Map Position to a Zoomed Map Position ready for 2D Drawing.
        /// </summary>
        /// <param name="mapPos3D">Unzoomed Map Position.</param>
        /// <param name="mapParams">Current Map Parameters.</param>
        /// <returns>Zoomed 2D Map Position.</returns>
        public static SKPoint ToZoomedPos(this Vector3 mapPos3D, RadarService.MapParameters mapParams) =>
            new SKPoint()
            {
                X = (mapPos3D.X - mapParams.Bounds.Left) * mapParams.XScale,
                Y = (mapPos3D.Y - mapParams.Bounds.Top) * mapParams.YScale
            };

        /// <summary>
        /// Gets a drawable 'Up Arrow'. IDisposable. Applies UI Scaling internally.
        /// </summary>
        public static SKPath GetUpArrow(this SKPoint point, float size = 6, float offsetX = 0, float offsetY = 0)
        {
            float x = point.X + offsetX;
            float y = point.Y + offsetY;

            size *= RadarService.Scale;
            var path = new SKPath();
            path.MoveTo(x, y);
            path.LineTo(x - size, y + size);
            path.LineTo(x + size, y + size);
            path.Close();

            return path;
        }

        /// <summary>
        /// Gets a drawable 'Down Arrow'. IDisposable. Applies UI Scaling internally.
        /// </summary>
        public static SKPath GetDownArrow(this SKPoint point, float size = 6, float offsetX = 0, float offsetY = 0)
        {
            float x = point.X + offsetX;
            float y = point.Y + offsetY;

            size *= RadarService.Scale;
            var path = new SKPath();
            path.MoveTo(x, y);
            path.LineTo(x - size, y - size);
            path.LineTo(x + size, y - size);
            path.Close();

            return path;
        }

        #endregion
    }
}
