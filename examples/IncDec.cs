// [file: IncDec.cs, started: 18-Dec-2007]

using System.Threading;                 // for Thread
using System.Diagnostics;               // for Debug.Assert

public class IncDec
{
    private int xx;
    private int yy;
    private int MAX;

    public IncDec(int MAX)
    {
        this.MAX = MAX;
    }

    public int Value => MAX;

    public void Inc()
    {
        while (true)
            xx = (xx + 1) % MAX;
    }

    public void Dec()
    {
        while (true)
            yy = (yy - 1) % MAX;
    }

    public void Mon()
    {
        Debug.Assert(xx + yy < MAX - 1);
    }

    public static void One()
    {
        var incdec = new IncDec(30);
        var inc = new Thread(new ThreadStart(incdec.Inc));
        var dec = new Thread(new ThreadStart(incdec.Dec));
        var mon = new Thread(new ThreadStart(incdec.Mon));
        inc.Start(); dec.Start(); mon.Start();
    }

    public static int Two()
    {
        var incdec = new IncDec(30);
        return incdec.Value;
    }

    public static int Three(int value)
    {
        var incdec = new IncDec(value);
        return incdec.Value * 12;
    }
}