using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples
{
    public class ExceptionHandling
    {
        // System exception
        // exception declared here
        // nested try
        // multiple catch clauses
        // catch clause with filter
        // no matching catch
        // no catch
        public static bool MethodWithoutCatch(int x, int y)
        {
            return x / y == 3;
        }

        public static bool MethodWithFinally(int x, int y)
        {
            int z;
            try
            {
                z = x / y;
            }
            finally
            {
                z = -1;
            }

            return z > 0;
        }

        public static int MethodWithCatchAndFinally(int x, int y)
        {
            int z = 0;
            try
            {
                z = x / y;
            }
            catch (DivideByZeroException e)
            {
                z = -10;
            }
            finally
            {
                z -= 4;
            }

            z -= 2;

            return z;
        }

        public static bool MethodWithConcreteExceptionHandlerWithoutParameter(int x, int y)
        {
            try
            {
                return x / y == 3;
            }
            catch (DivideByZeroException)
            {
                return false;
            }
        }

        public static bool MethodWithConcreteExceptionHandlerWithParameter(int x, int y)
        {
            try
            {
                return x / y == 3;
            }
            catch (DivideByZeroException e)
            {
                Console.Out.WriteLine(e.Message);
                return false;
            }
        }

        public static bool CallMethodWithoutCatch(int x, int y)
        {
            try
            {
                return MethodWithoutCatch(x, y);
            }
            catch (DivideByZeroException)
            {
                return false;
            }
        }

        public static bool UncaughtException(int x, int y)
        {
            try
            {
                if (y > 10)
                {
                    throw new ArgumentException("y > 10 violated", nameof(y));
                }

                return x / y == 3;
            }
            catch (DivideByZeroException)
            {
                return false;
            }
        }

        public static int MultipleExceptionFilters(int x, int y)
        {
            try
            {
                if (y > 10)
                {
                    throw new ArgumentException("y > 10 violated", nameof(y));
                }

                if (x / y == 3)
                {
                    return 1;
                }

                return 0;
            }
            catch (DivideByZeroException)
            {
                return -1;
            }
            catch (Exception)
            {
                return -2;
            }
        }
    }
}
