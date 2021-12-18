

string allforms = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\AllForms.txt";


//Triangle------------------------------------------------------------
int[] TT = { 10, 3, 4 };

try
{
    Triangle t = new Triangle(TT);
}
catch (Exception ex)
{
    using (StreamWriter sw = new StreamWriter(allforms, true, System.Text.Encoding.Default))
    { sw.WriteLine(ex.Message); }
    Console.WriteLine(ex.Message);
}

class Triangle
{
    int[] param = new int[3];
    public Triangle(int[] c)
    {
        if (c[0] + c[1] <= c[2] ||
            c[2] + c[0] <= c[1] ||
            c[1] + c[2] <= c[0] ||
            c[0] <= 0 || c[1] <= 0 || c[2] <= 0)
        {
            throw new TriangleException(c, $"Triangle was not created:\nTriangle: {c[0]}, {c[1]}, {c[2]}.");
        }
        param = c;
    }
}

class TriangleException : GeometryException
{
    public TriangleException() { }
    public TriangleException(string message)
        : base(message) {  }
    public TriangleException(int[] c, string message)
        : base(message) { throw new GeometryException(c, message); }
}

class Quadrangle
{
    int[] c = new int[4];
    public Quadrangle(int[] c)
    {
        if (c[0] + c[1] + c[2] <= c[3] ||
            c[3] + c[0] + c[1] <= c[2] ||
            c[2] + c[3] + c[0] <= c[1] ||
            c[1] + c[2] + c[3] <= c[0] ||
            c[0] <= 0 || c[1] <= 0 || c[2] <= 0 || c[3] <= 0)
        {
            throw new QuadrangleException(c,  $"Quadrangle was not created:\nQuadrangle: {c[0]}, {c[1]}, {c[2]}, {c[3]}.");
        }
        this.c = c;
    }
}

class QuadrangleException : GeometryException
{
    public QuadrangleException() { }
    public QuadrangleException(string message)
        : base(message) { throw new GeometryException(); }
    public QuadrangleException(int[] c, string message)
        : base(message) { throw new GeometryException(c, message); }
}

class Circle
{
    int[] c;
    public Circle(int[] c)
    {
        if (c[0] <= 0)
        {
            throw new CircleException(c, $"Circle was not created:\nCircle: {c[0]}.");
        }
        this.c = c;
    }
}

class CircleException : GeometryException
{
    public CircleException() { }
    
    public CircleException(string message)
        : base(message) { throw new GeometryException(); }
    public CircleException(int[] c, string message)
        : base(message) { throw new GeometryException(c, message); }
}

public class GeometryException : Exception
{
    private int[] param { get; set; }
    public GeometryException() { }
    public GeometryException(string message)
    : base(message) { }
    public GeometryException(int[] c, string message) : base(message)
    {
        param = c;
        string allforms = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\AllForms.txt";
        string except = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\exception.txt";

        if (param.Count() == 4 || param.Count() == 3)
            using (StreamWriter sw = new StreamWriter(except, true, System.Text.Encoding.Default))
            { sw.WriteLine(message); }

        using (StreamWriter sw = new StreamWriter(allforms, true, System.Text.Encoding.Default))
        { sw.WriteLine(message); }
    }
    public GeometryException(string message, Exception inner)
        : base(message, inner) { }
}
