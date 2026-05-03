using System;
using System.Collections.Generic;
using Tekla.Structures.Model;
using Tekla.Structures.Geometry3d;

namespace TeklaFrameGenerator
{
    public class FrameGenerator
    {
        private readonly Model _model;
        private readonly FrameGeneratorData _data;

        public FrameGenerator(Model model, FrameGeneratorData data)
        {
            _model = model;
            _data = data;
        }

        public void Generate()
        {
            _model.CommitChanges();

            foreach (var bay in _data.Bays)
            {
                GenerateBay(bay);
            }

            _model.CommitChanges();
        }

        private void GenerateBay(Bay bay)
        {
            double spacing = bay.EqualSpacing ? bay.TotalSpacing / bay.NumberOfFrames : 0;
            
            for (int i = 0; i < bay.NumberOfFrames; i++)
            {
                double xPosition = bay.EqualSpacing ? i * spacing : bay.ArbitrarySpacings[i];
                
                // Generate main frame
                GenerateFrame(xPosition, bay);
                
                // Generate truss if enabled
                if (_data.Truss.IsEnabled)
                {
                    GenerateTruss(xPosition, bay);
                }
                
                // Generate deck slab if enabled
                if (_data.DeckSlab.IsEnabled)
                {
                    GenerateDeckSlab(xPosition, bay);
                }
            }

            // Generate purlins and bracings between frames
            GeneratePurlins();
            GenerateBracings();
        }

        private void GenerateFrame(double xPosition, Bay bay)
        {
            var geom = _data.Geometry;
            
            // Calculate key points
            Point leftBase = new Point(xPosition, 0, 0);
            Point rightBase = new Point(xPosition, geom.Width, 0);
            Point leftTop = new Point(xPosition, 0, geom.ColumnHeight);
            Point rightTop = new Point(xPosition, geom.Width, geom.ColumnHeight);
            Point ridge = new Point(xPosition, geom.Width / 2, geom.ColumnHeight + geom.RoofHeight);

            // Create columns
            var leftColumn = CreateBeam(leftBase, leftTop, _data.Project.Materials.Columns);
            var rightColumn = CreateBeam(rightBase, rightTop, _data.Project.Materials.Columns);

            // Create roof beams based on roof type
            if (geom.RoofType == RoofType.Gable)
            {
                var leftRafter = CreateBeam(leftTop, ridge, _data.Project.Materials.Beams);
                var rightRafter = CreateBeam(ridge, rightTop, _data.Project.Materials.Beams);
            }
            else if (geom.RoofType == RoofType.SinglePitch)
            {
                var roofBeam = CreateBeam(leftTop, rightTop, _data.Project.Materials.Beams);
            }
        }

        private void GenerateTruss(double xPosition, Bay bay)
        {
            var truss = _data.Truss;
            var geom = _data.Geometry;

            // Truss chord points
            Point leftBottom = new Point(xPosition, 0, geom.ColumnHeight);
            Point rightBottom = new Point(xPosition, geom.Width, geom.ColumnHeight);
            Point ridge = new Point(xPosition, geom.Width / 2, geom.ColumnHeight + truss.Height);

            // Top chords
            var topChordLeft = CreateBeam(leftBottom, ridge, truss.TopChordSection, truss.TopChordMaterial);
            var topChordRight = CreateBeam(ridge, rightBottom, truss.TopChordSection, truss.TopChordMaterial);

            // Bottom chord (if not using main beams)
            if (!truss.UseMainBeamsAsBottomChord)
            {
                var bottomChord = CreateBeam(leftBottom, rightBottom, 
                    truss.BottomChordSection, truss.BottomChordMaterial);
            }

            // Truss posts/diagonals
            GenerateTrussWeb(xPosition, leftBottom, rightBottom, ridge, truss);
        }

        private void GenerateTrussWeb(double x, Point left, Point right, Point ridge, TrussData truss)
        {
            int numberOfPosts = truss.NumberOfPosts;
            double spacing = _data.Geometry.Width / (numberOfPosts + 1);

            for (int i = 1; i <= numberOfPosts; i++)
            {
                double yPos = i * spacing;
                double zHeight = CalculateTrussHeightAtPosition(yPos, left, right, ridge);
                
                Point bottomPoint = new Point(x, yPos, left.Z);
                Point topPoint = new Point(x, yPos, zHeight);

                // Vertical post
                if (truss.HasVerticalPosts)
                {
                    var post = CreateBeam(bottomPoint, topPoint, 
                        truss.PostSection, truss.PostMaterial);
                }

                // Diagonals
                if (truss.HasDiagonals && i < numberOfPosts)
                {
                    double nextYPos = (i + 1) * spacing;
                    double nextZHeight = CalculateTrussHeightAtPosition(nextYPos, left, right, ridge);
                    
                    Point nextTopPoint = new Point(x, nextYPos, nextZHeight);
                    
                    var diagonal = CreateBeam(topPoint, nextTopPoint, 
                        truss.DiagonalSection, truss.DiagonalMaterial);
                }
            }
        }

        private double CalculateTrussHeightAtPosition(double yPos, Point left, Point right, Point ridge)
        {
            // Linear interpolation for gable roof
            if (yPos <= _data.Geometry.Width / 2)
            {
                double ratio = yPos / (_data.Geometry.Width / 2);
                return left.Z + (ridge.Z - left.Z) * ratio;
            }
            else
            {
                double ratio = (yPos - _data.Geometry.Width / 2) / (_data.Geometry.Width / 2);
                return ridge.Z + (right.Z - ridge.Z) * ratio;
            }
        }

        private void GenerateDeckSlab(double xPosition, Bay bay)
        {
            var deck = _data.DeckSlab;
            var geom = _data.Geometry;

            // Deck slab beams
            double beamSpacing = deck.BeamSpacing;
            int numberOfBeams = (int)(geom.Width / beamSpacing) + 1;

            for (int i = 0; i < numberOfBeams; i++)
            {
                double yPos = i * beamSpacing;
                if (yPos > geom.Width) yPos = geom.Width;

                Point start = new Point(xPosition, yPos, geom.ColumnHeight);
                Point end = new Point(xPosition, yPos, geom.ColumnHeight);

                // Create deck beam (spanning between frames)
                // This would connect to adjacent frames
            }

            // Deck slab columns if enabled
            if (deck.HasColumns)
            {
                // Create intermediate columns
            }
        }

        private void GeneratePurlins()
        {
            // Generate purlins between frames along the roof
            var purlinSpacing = _data.Purlins.Spacing;
            // Implementation for purlin generation
        }

        private void GenerateBracings()
        {
            // Generate wall and roof bracings
            // Implementation for bracing generation
        }

        private Beam CreateBeam(Point start, Point end, string profile, string material)
        {
            var beam = new Beam
            {
                StartPoint = start,
                EndPoint = end,
                Profile = new Profile { ProfileString = profile },
                Material = new Material { MaterialString = material },
                Class = 1,
                Name = "FRAME_MEMBER",
                Finish = "GALVANIZED"
            };

            beam.Insert();
            return beam;
        }

        private Column CreateColumn(Point position, double height, string profile, string material)
        {
            var column = new Column
            {
                BasePoint = position,
                TopPoint = new Point(position.X, position.Y, position.Z + height),
                Profile = new Profile { ProfileString = profile },
                Material = new Material { MaterialString = material },
                Class = 1,
                Name = "FRAME_COLUMN"
            };

            column.Insert();
            return column;
        }
    }

    // Data Classes
    public class FrameGeneratorData
    {
        public List<Bay> Bays { get; set; } = new List<Bay>();
        public ProjectData Project { get; set; } = new ProjectData();
        public GeometryData Geometry { get; set; } = new GeometryData();
        public TrussData Truss { get; set; } = new TrussData();
        public DeckSlabData DeckSlab { get; set; } = new DeckSlabData();
        public PurlinsData Purlins { get; set; } = new PurlinsData();
        public BracingsData Bracings { get; set; } = new BracingsData();
    }

    public class Bay
    {
        public int Number { get; set; }
        public string Name { get; set; }
        public int NumberOfFrames { get; set; }
        public bool EqualSpacing { get; set; }
        public double TotalSpacing { get; set; }
        public List<double> ArbitrarySpacings { get; set; } = new List<double>();
        public bool IsMirror { get; set; }
    }

    public class ProjectData
    {
        public Point InsertionPoint { get; set; } = new Point(0, 0, 0);
        public double Rotation { get; set; }
        public bool SetMaterialsForAll { get; set; } = true;
        public Materials Materials { get; set; } = new Materials();
    }

    public class Materials
    {
        public string Columns { get; set; } = "S355";
        public string Beams { get; set; } = "S355";
        public string TrussChords { get; set; } = "S355";
        public string TrussDiagonals { get; set; } = "S355";
        public string Purlins { get; set; } = "S355";
        public string Bracings { get; set; } = "S235";
    }

    public class GeometryData
    {
        public RoofType RoofType { get; set; } = RoofType.Gable;
        public bool BaySymmetry { get; set; } = true;
        public double RoofHeight { get; set; } = 2550;
        public double ColumnHeight { get; set; } = 5300;
        public double FoundationHeight { get; set; } = 300;
        public double Width { get; set; } = 10000;
        public double RoofAngle { get; set; } = 27.02;
        public SupportType LeftSupport { get; set; } = SupportType.Fixed;
        public SupportType RightSupport { get; set; } = SupportType.Fixed;
        public string ColumnSection { get; set; } = "HEA300";
        public string BeamSection { get; set; } = "IPE400";
    }

    public enum RoofType { SinglePitch, Gable }
    public enum SupportType { Fixed, Pinned, Free }

    public class TrussData
    {
        public bool IsEnabled { get; set; }
        public TrussType Type { get; set; } = TrussType.Gable;
        public bool ContinuousChords { get; set; }
        public bool MomentsReleased { get; set; } = true;
        public int NumberOfPosts { get; set; } = 4;
        public double Height { get; set; } = 1000;
        public double DiagonalHeight { get; set; } = 700;
        public bool HasVerticalPosts { get; set; } = true;
        public bool HasDiagonals { get; set; } = true;
        public bool UseMainBeamsAsBottomChord { get; set; } = true;
        public string TopChordSection { get; set; } = "CHS139.7x5";
        public string BottomChordSection { get; set; } = "CHS139.7x5";
        public string PostSection { get; set; } = "CHS88.9x4";
        public string DiagonalSection { get; set; } = "CHS60.3x3.2";
        public string TopChordMaterial { get; set; } = "S355";
        public string BottomChordMaterial { get; set; } = "S355";
        public string PostMaterial { get; set; } = "S355";
        public string DiagonalMaterial { get; set; } = "S355";
    }

    public enum TrussType { Gable, Flat, Curved }

    public class DeckSlabData
    {
        public bool IsEnabled { get; set; }
        public bool Left { get; set; }
        public bool EntireBay { get; set; }
        public bool Right { get; set; }
        public double LeftHeight { get; set; } = 2800;
        public double RightHeight { get; set; } = 2800;
        public double LeftWidth { get; set; } = 4000;
        public double RightWidth { get; set; } = 4000;
        public double BeamSpacing { get; set; } = 300;
        public bool HasColumns { get; set; }
        public string BeamSection { get; set; } = "IPE300";
        public string ColumnSection { get; set; } = "HEA200";
        public string Material { get; set; } = "S355";
    }

    public class PurlinsData
    {
        public double Spacing { get; set; } = 1500;
        public string Section { get; set; } = "C200x75";
        public string Material { get; set; } = "S355";
    }

    public class BracingsData
    {
        public bool WallBracing { get; set; } = true;
        public bool RoofBracing { get; set; } = true;
        public string Section { get; set; } = "L80x8";
        public string Material { get; set; } = "S235";
    }
}