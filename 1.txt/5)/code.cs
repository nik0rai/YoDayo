using System;

namespace ConsoleApp4
{
    public interface IToolKit
    {
        string[] GetTools();
    }
    public interface IParts
    {
        string[] GetParts();
    }
    public interface IInfo
    {
        string GetName();
        string GetColor();
    }

    public class Table : IToolKit, IParts, IInfo
    {
        public string TableName = "Ustah";
        public string TableColor = "White";
        public string GetName() => TableName;
        public string GetColor() => TableColor;

        public string[] TableTools = { "Hammer", "Screwdriver", "Saw" };
        public string[] TableParts = { "Table top", "Table leg" };
        public string[] GetTools() => TableTools;
        public string[] GetParts() => TableParts;
    }
    public class Wardrobe : IToolKit, IParts, IInfo
    {
        public string WardrobeName = "Hersser";
        public string WardrobeColor = "Black";
        public string GetName() => WardrobeName;
        public string GetColor() => WardrobeColor;

        public string[] WardrobeTools = { "Drill", "Jigsaw", "Puncher" };
        public string[] WardrobeParts = { "Frame", "Shelf", "Handle" };
        public string[] GetTools() => WardrobeTools;
        public string[] GetParts() => WardrobeParts;
    }
    public class FloorLamp : IToolKit, IParts, IInfo
    {
        public string FloorLampName = "Looter";
        public string FloorLampColor = "Gray";
        public string GetName() => FloorLampName;
        public string GetColor() => FloorLampColor;

        public string[] FloorLampTools = { "Nippers", "Spatula", "Screwdriver" };
        public string[] FloorLampParts = { "Tube", "Lamp", "Lampshade" };
        public string[] GetTools() => FloorLampTools;
        public string[] GetParts() => FloorLampParts;
    }

    public class FurnitureKit<TContent> where TContent : IToolKit, IParts, IInfo
    {
        TContent content = default;
        public FurnitureKit(TContent content) => this.content = content;
        public override string ToString()
        {
            string s = "\nParts: ";
            foreach (var parts in content.GetParts())
                s += $"{parts}, ";
            s = s.Remove(s.LastIndexOf(','));

            s += "\nTools: ";
            foreach (var tools in content.GetTools())            
                s += $"{tools}, ";
            s = s.Remove(s.LastIndexOf(','));

            s += $"\nName: {content.GetName()}\nColor: {content.GetColor()}";
            return s;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Table table = new();
            Wardrobe wardrobe = new();
            FloorLamp floorlamp = new();

            Console.WriteLine("\t   Table");
            FurnitureKit<Table> tablefurniture = new(table);
            Console.WriteLine(tablefurniture);

            Console.WriteLine("\n\n\n\t  Wardrobe");
            FurnitureKit<Wardrobe> wardrobefurniture = new(wardrobe);
            Console.WriteLine(wardrobefurniture);

            Console.WriteLine("\n\n\n\t  FloorLamp");
            FurnitureKit<FloorLamp> floorlampfurniture = new(floorlamp);
            Console.WriteLine(floorlampfurniture);
        }
    }
}
