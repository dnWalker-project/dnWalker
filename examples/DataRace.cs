// [file: DateRace.cs, started: 15-Mar-2007]
// Small C# program with data race - for MMC demo.

using System.Threading;
using System.Diagnostics;

class Cell { 
    int v=1;
    public int  Get()   { return v; }
    public void Add3()  { v = v+3;  } 
    public void Mul3()  { v = v*3;  } 
} 

class DataRace {
    public static void Main(string[] args) {
        Cell cell  = new Cell(); 
        Thread t1  = new Thread(new ThreadStart(cell.Add3));
        Thread t2  = new Thread(new ThreadStart(cell.Mul3));
        t1.Start(); t2.Start();
        // Assertion below should catch all possible values of cell.Get().
        Debug.Assert(cell.Get() == 1 || cell.Get() == 3 || 
                     cell.Get() == 4 || cell.Get() == 6 || cell.Get() == 12);
        System.Console.WriteLine(cell.Get());
    }
}
