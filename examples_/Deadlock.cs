using System.Threading;

class Deadlock {
    class PossibleDeadlock {
        object l1 = new object();
        object l2 = new object();

        public void Get12() {
            lock (l1) {
                lock (l2) {
                    System.Console.WriteLine("Got locks l1 and l2");
                }
            }
        }
        public void Get21() {
            lock (l2) {
                lock (l1) {
                    System.Console.WriteLine("Got locks l2 and l1");
                }
            }
        }
    }
    
    public static void Main(string[] args) {
        PossibleDeadlock dl = new PossibleDeadlock();
        Thread t1 = new Thread(new ThreadStart(dl.Get12)); t1.Start();
        Thread t2 = new Thread(new ThreadStart(dl.Get21)); t2.Start();
    }
}
