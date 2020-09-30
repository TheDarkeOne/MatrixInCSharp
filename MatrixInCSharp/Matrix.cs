using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MatrixInCSharp
{
    public class Matrix
    {
        public int runs;
        public int Cols2Print = 119;
        readonly char[] line;
        public string alpha1 = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        readonly Semaphore semaphore;
        readonly Semaphore sem;

        public Matrix()
        {
            line = new char[Cols2Print + 1];
            semaphore = new Semaphore(0, Cols2Print);
            sem = new Semaphore(0, Cols2Print);
        }
        public void TestThread(object id)
        {
            Random rand = new Random();
            int j = 0, i = (int)id;
            bool win = false;
            for (int k = 0; k < runs; k++)
            {
                semaphore.WaitOne();
                if (rand.Next() % 500 == 0 || win)
                {
                    win = true;
                    line[i] = alpha1[j];
                    if (alpha1[j] == 'Z')
                    {
                        win = false;
                        j = 0;
                    }
                    else
                    {
                        j++;
                    }
                    sem.Release(1);
                }
                else
                {
                    line[i] = ' ';
                    sem.Release(1);
                }
            }
        }

        public void MatrixRun(int length)
        {
            runs = length;
            Console.ForegroundColor = ConsoleColor.Green;

            Thread[] pIds = new Thread[Cols2Print];
            Console.WriteLine("main: begin\n");
            Console.WriteLine($"Running with {runs} repetitions\n");

            for (int k = 0; k < Cols2Print; k++)
            {
                ParameterizedThreadStart start = new ParameterizedThreadStart(TestThread);
                pIds[k] = new Thread(start)
                {
                    Name = "thread_" + k
                };
                pIds[k].Start(k);
            }

            Thread.Sleep(1000);

            for (int j = 0; j < runs; j++)
            {
                for (int i = 0; i < Cols2Print; i++)
                {
                    semaphore.Release(1);
                }

                for (int i = 0; i < Cols2Print; i++)
                {
                    sem.WaitOne();
                }

                Console.WriteLine(line);
            }

            Console.WriteLine("...DONE!...\n");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
