using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples.Concolic.Features.Arrays
{
    public class Array
    {
        public static int comp(int i, int[] arr)
        {
            var a = arr[i];
            return 1 / a;
        }

        /*
         @Test
2 public void test0() {
3 comp(0,new int[]{1});
4 }
5
6 @Test(expected =
java.lang.ArithmeticException.class)
7 public void test1() {
8 comp(0,new int[]{0});
9 //leads to java.lang.ArithmeticException
10 }
11
12 @Test(expected =
java.lang.ArrayIndexOutOfBoundsException.class
)
13 public void test2() {
14 comp(1073741824,new int[]{0});
15 //leads to java.lang.ArrayIndexOutOfBoundsException
16 }
17
18 @Test(expected =
java.lang.ArrayIndexOutOfBoundsException.class
)
19 public void test3() {
20 comp(−2147483648,new int[0]);
21 //leads to java.lang.ArrayIndexOutOfBoundsException
22 }
*/
    }
}
