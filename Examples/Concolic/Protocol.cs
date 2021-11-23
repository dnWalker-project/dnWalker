using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples.Concolic
{
    public class Protocol
    {
        //@Symbolic("true")
        private int buffer_empty = 1;

        // pdu p, ack; // pdus
        //@Symbolic("true")
        public int expect = 0; // next expected seq. nr

        public void msg(int sequence, int content)
        {
            if (sequence < 0) return;

            System.Console.Out.WriteLine("expect = " + expect);
            var prevExpect = expect;
            if (expect > 0)
                prevExpect--;

            if ((buffer_empty == 1) && ((sequence + 7) % 7 == (prevExpect + 2) % 2))
            {  // this is as expected
                expect++;
                buffer_empty = 0;
                // OK message will be passed to upper layer
            }
            else
            {
                System.Diagnostics.Debug.Assert(false);
                // message is discarded
            }
        }

        public void recv_ack(int value)
        {
            if (buffer_empty == 1)
            {
                System.Diagnostics.Debug.Assert(false);
            }
            else
            {
                if (value == (((expect - 1) + 2) % 2))
                {
                    // ack is enabled, message is consumed
                    buffer_empty = 1 - buffer_empty;
                }
                else
                {
                    // not the right sequence
                    System.Diagnostics.Debug.Assert(false);
                }
            }
        }

        public void reset()
        {
            //    if (buffer_empty == 1) assert false;
            buffer_empty = 1;
            expect = 0;
        }/*

        public static void main(String[] args)
        {
            System.out.println("-------- In main!");
            Protocol p = new Protocol();
            p.msg(0, 6);
            p.recv_ack(0);
            p.reset();
        }*/
    }
}
