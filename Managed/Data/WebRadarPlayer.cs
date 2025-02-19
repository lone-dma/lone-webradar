using AncientMountain.Managed.Services;
using AncientMountain.Managed.Skia;
using MessagePack;
using SkiaSharp;
using System.Numerics;

namespace AncientMountain.Managed.Data
{
    [MessagePackObject]
    public sealed class WebRadarPlayer
    {
        /// <summary>
        /// Player Name.
        /// </summary>
        [Key(0)]
        public string Name { get; init; }
        /// <summary>
        /// Player Type (PMC, Scav,etc.)
        /// </summary>
        [Key(1)]
        public WebPlayerType Type { get; init; }
        /// <summary>
        /// True if player is active, otherwise False.
        /// </summary>
        [Key(2)]
        public bool IsActive { get; init; }
        /// <summary>
        /// True if player is alive, otherwise False.
        /// </summary>
        [Key(3)]
        public bool IsAlive { get; init; }
        /// <summary>
        /// Unity World Position.
        /// </summary>
        [Key(4)]
        public System.Numerics.Vector3 Position { get; init; }
        /// <summary>
        /// Unity World Rotation.
        /// </summary>
        [Key(5)]
        public System.Numerics.Vector2 Rotation { get; init; }
        /// <summary>
        /// Player has exfil'd/left the raid.
        /// </summary>
        [IgnoreMember]
        public bool HasExfild => !IsActive && IsAlive;

        /// <summary>
        /// Player's Map Rotation (with 90 degree correction applied).
        /// </summary>
        [IgnoreMember]
        public float MapRotation
        {
            get
            {
                float mapRotation = Rotation.X; // Cache value
                mapRotation -= 90f;
                while (mapRotation < 0f)
                    mapRotation += 360f;

                return mapRotation;
            }
        }

        public void Draw(SKCanvas canvas, SKImageInfo info, RadarService.MapParameters mapParams, WebRadarPlayer localPlayer)
        {
            try
            {
                var point = Position.ToMapPos(mapParams.Map).ToZoomedPos(mapParams);
                if (point.X < info.Rect.Left - 15 || point.X > info.Rect.Right + 15 ||
                    point.Y < info.Rect.Top - 15 || point.Y > info.Rect.Bottom + 15)
                    return; // Player is outside of the map bounds (not visible)
                if (!IsAlive) // Player Dead -- Draw 'X' death marker and move on
                {
                    DrawDeathMarker(canvas, point);
                }
                else
                {
                    DrawPlayerMarker(canvas, localPlayer, point);
                    if (this == localPlayer)
                        return;
                    var height = Position.Y - localPlayer.Position.Y;
                    var dist = Vector3.Distance(localPlayer.Position, Position);

                    var lines = new string[2]
                    {
                        this.Name,
                        $"H: {(int)Math.Round(height)} D: {(int)Math.Round(dist)}"
                    };

                    DrawPlayerText(canvas, localPlayer, point, lines);
                }
            }
            catch
            {
                //Debug.WriteLine($"WARNING! Player Draw Error: {ex}");
            }
        }

        /// <summary>
        /// Draws Player Text on this location.
        /// </summary>
        private void DrawPlayerText(SKCanvas canvas, WebRadarPlayer localPlayer, SKPoint point, string[] lines)
        {
            var paints = GetPaints(localPlayer);
            var spacing = 3 * RadarService.Scale;
            point.Offset(9 * RadarService.Scale, spacing);
            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line?.Trim()))
                    continue;

                canvas.DrawText(line, point, SKPaints.TextOutline); // Draw outline
                canvas.DrawText(line, point, paints.Item2); // draw line text
                point.Offset(0, 12 * RadarService.Scale);
            }
        }

        /// <summary>
        /// Draws a Player Marker on this location.
        /// </summary>
        private void DrawPlayerMarker(SKCanvas canvas, WebRadarPlayer localPlayer, SKPoint point)
        {
            var radians = MapRotation.ToRadians();
            var paints = GetPaints(localPlayer);

            SKPaints.ShapeOutline.StrokeWidth = paints.Item1.StrokeWidth + 2f * RadarService.Scale;

            var size = 6 * RadarService.Scale;
            canvas.DrawCircle(point, size, SKPaints.ShapeOutline); // Draw outline
            canvas.DrawCircle(point, size, paints.Item1); // draw LocalPlayer marker

            int aimlineLength = this == localPlayer ? 
                1500 : 15;

            var aimlineEnd = GetAimlineEndpoint(point, radians, aimlineLength);
            canvas.DrawLine(point, aimlineEnd, SKPaints.ShapeOutline); // Draw outline
            canvas.DrawLine(point, aimlineEnd, paints.Item1); // draw LocalPlayer aimline
        }

        /// <summary>
        /// Gets the point where the Aimline 'Line' ends. Applies UI Scaling internally.
        /// </summary>
        private static SKPoint GetAimlineEndpoint(SKPoint start, double radians, float aimlineLength)
        {
            aimlineLength *= RadarService.Scale;
            return new SKPoint((float)(start.X + Math.Cos(radians) * aimlineLength),
                (float)(start.Y + Math.Sin(radians) * aimlineLength));
        }

        /// <summary>
        /// Draws a Death Marker on this location.
        /// </summary>
        private static void DrawDeathMarker(SKCanvas canvas, SKPoint point)
        {
            var length = 6 * RadarService.Scale;
            canvas.DrawLine(new SKPoint(point.X - length, point.Y + length),
                new SKPoint(point.X + length, point.Y - length), SKPaints.PaintDeathMarker);
            canvas.DrawLine(new SKPoint(point.X - length, point.Y - length),
                new SKPoint(point.X + length, point.Y + length), SKPaints.PaintDeathMarker);
        }

        private ValueTuple<SKPaint, SKPaint> GetPaints(WebRadarPlayer localPlayer)
        {
            if (this == localPlayer)
                return new ValueTuple<SKPaint, SKPaint>(SKPaints.PaintLocalPlayer, SKPaints.TextLocalPlayer);
            switch (this.Type)
            {
                case WebPlayerType.LocalPlayer:
                    return new ValueTuple<SKPaint, SKPaint>(SKPaints.PaintTeammate, SKPaints.TextTeammate);
                case WebPlayerType.Teammate:
                    return new ValueTuple<SKPaint, SKPaint>(SKPaints.PaintTeammate, SKPaints.TextTeammate);
                case WebPlayerType.Player:
                    return new ValueTuple<SKPaint, SKPaint>(SKPaints.PaintPlayer, SKPaints.TextPlayer);
                case WebPlayerType.PlayerScav:
                    return new ValueTuple<SKPaint, SKPaint>(SKPaints.PaintPlayerScav, SKPaints.TextPlayerScav);
                case WebPlayerType.Bot:
                    return new ValueTuple<SKPaint, SKPaint>(SKPaints.PaintBot, SKPaints.TextBot);
                default:
                    return new ValueTuple<SKPaint, SKPaint>(SKPaints.PaintBot, SKPaints.TextBot);
            }
        }
    }
}
