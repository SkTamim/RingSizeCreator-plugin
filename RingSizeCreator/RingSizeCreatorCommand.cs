using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Rhino;
using Rhino.Commands;
using Rhino.DocObjects;
using Rhino.Geometry;
using Rhino.Input;
using Rhino.Input.Custom;

namespace RingSizeCreator
{
    /// <summary>
    /// This class contains all the logic for our command.
    /// </summary>
    public class RingSizeCreatorCommand : Command
    {
        // --- Data Section ---
        private class RingSizeEntry
        {
            public double Indian { get; set; }
            public double US { get; set; }
            public double DiameterMM { get; set; }
        }

        private readonly List<RingSizeEntry> _ringSizeChart = new List<RingSizeEntry>
        {
          new RingSizeEntry { Indian = 0.5, US = 1.25, DiameterMM = 12.6},
          new RingSizeEntry { Indian = 0.75, US = 1.5, DiameterMM = 12.9},
          new RingSizeEntry { Indian = 1, US = 1.75, DiameterMM = 13.1},
          new RingSizeEntry { Indian = 2, US = 2, DiameterMM = 13.3},
          new RingSizeEntry { Indian = 2.5, US = 2.25, DiameterMM = 13.5},
          new RingSizeEntry { Indian = 3, US = 2.5, DiameterMM = 13.7},
          new RingSizeEntry { Indian = 4, US = 2.75, DiameterMM = 13.9},
          new RingSizeEntry { Indian = 4.5, US = 3, DiameterMM = 14.1},
          new RingSizeEntry { Indian = 5, US = 3.25, DiameterMM = 14.3},
          new RingSizeEntry { Indian = 5.5, US = 3.5, DiameterMM = 14.5},
          new RingSizeEntry { Indian = 6, US = 3.75, DiameterMM = 14.7},
          new RingSizeEntry { Indian = 6.5, US = 4, DiameterMM = 14.9},
          new RingSizeEntry { Indian = 7, US = 4.25, DiameterMM = 15.1},
          new RingSizeEntry { Indian = 8, US = 4.5, DiameterMM = 15.3},
          new RingSizeEntry { Indian = 9, US = 4.75, DiameterMM = 15.5},
          new RingSizeEntry { Indian = 9.5, US = 5, DiameterMM = 15.7},
          new RingSizeEntry { Indian = 10, US = 5.25, DiameterMM = 15.9},
          new RingSizeEntry { Indian = 10.5, US = 5.5, DiameterMM = 16.1},
          new RingSizeEntry { Indian = 11, US = 5.75, DiameterMM = 16.3},
          new RingSizeEntry { Indian = 12, US = 6, DiameterMM = 16.5},
          new RingSizeEntry { Indian = 12.5, US = 6.25, DiameterMM = 16.7},
          new RingSizeEntry { Indian = 13, US = 6.5, DiameterMM = 16.9},
          new RingSizeEntry { Indian = 13.5, US = 6, DiameterMM = 17.1},
          new RingSizeEntry { Indian = 14, US = 7, DiameterMM = 17.3},
          new RingSizeEntry { Indian = 15, US = 7.25, DiameterMM = 17.5},
          new RingSizeEntry { Indian = 15.5, US = 7.5, DiameterMM = 17.7},
          new RingSizeEntry { Indian = 16, US = 7.75, DiameterMM = 17.9},
          new RingSizeEntry { Indian = 17, US = 8, DiameterMM = 18.1},
          new RingSizeEntry { Indian = 17.5, US = 8.25, DiameterMM = 18.3},
          new RingSizeEntry { Indian = 18, US = 8.5, DiameterMM = 18.5},
          new RingSizeEntry { Indian = 19, US = 8.75, DiameterMM = 18.7},
          new RingSizeEntry { Indian = 19.5, US = 9, DiameterMM = 18.9},
          new RingSizeEntry { Indian = 20, US = 9.25, DiameterMM = 19.2},
          new RingSizeEntry { Indian = 21, US = 9.5, DiameterMM = 19.4},
          new RingSizeEntry { Indian = 21.5, US = 9.75, DiameterMM = 19.6},
          new RingSizeEntry { Indian = 22, US = 10, DiameterMM = 19.8},
          new RingSizeEntry { Indian = 23, US = 10.25, DiameterMM = 20},
          new RingSizeEntry { Indian = 23.5, US = 10.5, DiameterMM = 20.2},
          new RingSizeEntry { Indian = 24, US = 10.75, DiameterMM = 20.4},
          new RingSizeEntry { Indian = 25, US = 11, DiameterMM = 20.6},
          new RingSizeEntry { Indian = 25.5, US = 11.25, DiameterMM = 20.8},
          new RingSizeEntry { Indian = 26, US = 11.5, DiameterMM = 21},
          new RingSizeEntry { Indian = 26.5, US = 11.75, DiameterMM = 21.2},
          new RingSizeEntry { Indian = 27, US = 12, DiameterMM = 21.4},
          new RingSizeEntry { Indian = 28, US = 12.25, DiameterMM = 21.6},
          new RingSizeEntry { Indian = 28.5, US = 12.5, DiameterMM = 21.8},
          new RingSizeEntry { Indian = 29, US = 12.75, DiameterMM = 22},
          new RingSizeEntry { Indian = 30, US = 13, DiameterMM = 22.2},
          new RingSizeEntry { Indian = 30.5, US = 13.25, DiameterMM = 22.4},
          new RingSizeEntry { Indian = 31, US = 13.5, DiameterMM = 22.6},
          new RingSizeEntry { Indian = 32, US = 13.75, DiameterMM = 22.8},
          new RingSizeEntry { Indian = 32.5, US = 14, DiameterMM = 23},
          new RingSizeEntry { Indian = 33, US = 14.25, DiameterMM = 23.2},
          new RingSizeEntry { Indian = 33.5, US = 14.5, DiameterMM = 23.4},
          new RingSizeEntry { Indian = 34, US = 14.75, DiameterMM = 23.6},
          new RingSizeEntry { Indian = 35, US = 15, DiameterMM = 23.8},
          new RingSizeEntry { Indian = 35.5, US = 15.25, DiameterMM = 24},
          new RingSizeEntry { Indian = 36, US = 15.5, DiameterMM = 24.2},
          new RingSizeEntry { Indian = 36.5, US = 15.75, DiameterMM = 24.4},
          new RingSizeEntry { Indian = 37, US = 16, DiameterMM = 24.6},
        };

        public override string EnglishName => "RingSizeCreator";

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            string currentMode = "indian";
            double value = 0.0;

            _ringSizeChart.Sort((a, b) => a.Indian.CompareTo(b.Indian));

            while (true)
            {
                var gn = new GetNumber();
                gn.AcceptNothing(true);

                if (currentMode == "indian")
                {
                    gn.SetCommandPrompt("Enter Indian Size");
                    gn.AddOption("Diameter");
                }
                else
                {
                    gn.SetCommandPrompt("Enter Diameter in mm");
                    gn.AddOption("IndianRingSize");
                }

                var result = gn.Get();

                if (result == GetResult.Number)
                {
                    value = gn.Number();
                    break;
                }

                if (result == GetResult.Option)
                {
                    currentMode = (currentMode == "indian") ? "diameter" : "indian";
                    continue;
                }

                RhinoApp.WriteLine("Command cancelled.");
                return Result.Cancel;
            }

            if (currentMode == "indian")
            {
                CreateRingByIndianSize(value);
            }
            else
            {
                CreateRingByDiameter(value);
            }

            return Result.Success;
        }

        private void CreateRingByIndianSize(double indianSizeInput)
        {
            var foundSize = _ringSizeChart.FirstOrDefault(s => s.Indian == indianSizeInput);
            double diameter;
            string msg;

            if (foundSize != null)
            {
                diameter = foundSize.DiameterMM;
                msg = $"Created ring rail for Indian Size {indianSizeInput}.\nUS Size: {foundSize.US}\nDiameter: {diameter} mm";
            }
            else
            {
                var firstSize = _ringSizeChart[0];
                var lastSize = _ringSizeChart[_ringSizeChart.Count - 1];

                if (indianSizeInput < firstSize.Indian)
                {
                    msg = $"Input size is too small. The smallest size in the chart is Indian {firstSize.Indian} ({firstSize.DiameterMM} mm).";
                    Rhino.UI.Dialogs.ShowMessageBox(msg, "Input Size Too Small");
                    return;
                }
                else if (indianSizeInput > lastSize.Indian)
                {
                    msg = $"Input size is too large. The largest size in the chart is Indian {lastSize.Indian} ({lastSize.DiameterMM} mm).";
                    Rhino.UI.Dialogs.ShowMessageBox(msg, "Input Size Too Large");
                    return;
                }
                else
                {
                    var lowerNeighbor = _ringSizeChart.Last(s => s.Indian < indianSizeInput);
                    var upperNeighbor = _ringSizeChart.First(s => s.Indian > indianSizeInput);

                    double sizeRange = upperNeighbor.Indian - lowerNeighbor.Indian;
                    double posInRange = indianSizeInput - lowerNeighbor.Indian;
                    double factor = posInRange / sizeRange;

                    double diameterRange = upperNeighbor.DiameterMM - lowerNeighbor.DiameterMM;
                    double calculatedDiameter = lowerNeighbor.DiameterMM + (diameterRange * factor);

                    diameter = Math.Round(calculatedDiameter, 2);

                    double approxUsSize = Math.Round(lowerNeighbor.US + (upperNeighbor.US - lowerNeighbor.US) * factor, 2);

                    var nearestStandardSize = _ringSizeChart.OrderBy(s => Math.Abs(s.DiameterMM - diameter)).First();

                    msg = $"Size {indianSizeInput} not a Standard Ring size. A precise diameter was calculated.\n\n" +
                          $"Approx. US Size: ~{approxUsSize}\n" +
                          $"Calculated Diameter: {diameter} mm\n\n" +
                          $"---\nNearest Standard Size:\n" +
                          $"Indian: {nearestStandardSize.Indian}, US: {nearestStandardSize.US}, Diameter: {nearestStandardSize.DiameterMM} mm";
                }
            }

            CreateGeometry(diameter, msg);
        }

        private void CreateRingByDiameter(double diameterInput)
        {
            var firstSize = _ringSizeChart[0];
            var lastSize = _ringSizeChart[_ringSizeChart.Count - 1];

            if (diameterInput < firstSize.DiameterMM)
            {
                string msg = $"Input diameter is too small. The smallest diameter in the chart is {firstSize.DiameterMM} mm (Indian Size {firstSize.Indian}).";
                Rhino.UI.Dialogs.ShowMessageBox(msg, "Input Diameter Too Small");
                return;
            }

            if (diameterInput > lastSize.DiameterMM)
            {
                string msg = $"Input diameter is too large. The largest diameter in the chart is {lastSize.DiameterMM} mm (Indian Size {lastSize.Indian}).";
                Rhino.UI.Dialogs.ShowMessageBox(msg, "Input Diameter Too Large");
                return;
            }

            var exactMatch = _ringSizeChart.FirstOrDefault(s => s.DiameterMM == diameterInput);
            if (exactMatch != null)
            {
                string msg = $"Created ring rail for Diameter {exactMatch.DiameterMM} mm.\nIndian Size: {exactMatch.Indian}\nUS Size: {exactMatch.US}";
                CreateGeometry(exactMatch.DiameterMM, msg);
                return;
            }

            var closestSize = _ringSizeChart.OrderBy(s => Math.Abs(s.DiameterMM - diameterInput)).First();
            if (closestSize != null)
            {
                double standardDiameter = closestSize.DiameterMM;
                string msg = $"{diameterInput} is not a standard size.\nCreated ring rail with nearest Standard size.\nIndian Size: {closestSize.Indian}\nUS Size: {closestSize.US}\nDiameter: {standardDiameter} mm";
                CreateGeometry(standardDiameter, msg);
            }
        }

        private void CreateGeometry(double diameter, string primaryMessage)
        {
            var allCommandNames = Command.GetCommandNames(true, true);
            bool gvCommandExists = allCommandNames.Contains("gvRingRail", StringComparer.InvariantCultureIgnoreCase);

            if (gvCommandExists && diameter <= 24.0)
            {
                string commandString = $"_-gvRingRail D {diameter} _Enter";

                EventHandler onIdle = null;
                onIdle = (sender, e) =>
                {
                    RhinoApp.Idle -= onIdle;
                    RhinoApp.RunScript(commandString, false);
                };

                RhinoApp.Idle += onIdle;
                RhinoApp.WriteLine($"Creating Matrix ring rail with diameter: {diameter}");
                Rhino.UI.Dialogs.ShowMessageBox(primaryMessage, "Ring Rail Created");
            }
            else
            {
                var doc = RhinoDoc.ActiveDoc;
                if (doc != null)
                {
                    Color fingerColor = Color.FromArgb(255, 173, 63, 63);

                    var attributes = new ObjectAttributes();
                    attributes.ColorSource = ObjectColorSource.ColorFromObject;
                    attributes.ObjectColor = fingerColor;

                    Plane frontPlane = new Plane(Point3d.Origin, Vector3d.YAxis);
                    var circle = new Circle(frontPlane, diameter / 2.0);

                    doc.Objects.AddCircle(circle, attributes);
                    doc.Views.Redraw();

                    // ** NEW LOGIC HERE **
                    // This block now builds the final message by appending a note to the primary message.
                    string finalMessage = primaryMessage;
                    string title = "Ring Rail Created";

                    if (gvCommandExists)
                    {
                        // This case only happens if the diameter was > 24.0
                        finalMessage += "\n\n---\nNOTE: Diameter is too large, can't make an acctual Ring Rali of matrixs.\n\nA standard circle has been created to use as Ring Rail.";
                        title = "PAY ATTENTION";
                    }
                    // For standalone Rhino users, no extra note is needed, so we just show the primary message.

                    RhinoApp.WriteLine($"Creating standard circle with diameter: {diameter}");
                    Rhino.UI.Dialogs.ShowMessageBox(finalMessage, title);
                }
            }
        }
    }
}