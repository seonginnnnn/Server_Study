using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServerCore
{
    class SpinLock
    {
        volatile int _locked = 0;

        // 만약 다른 스레드가 lock을 소유하고 있다면 그 lock이 반환될 때까지 계속 확인하며 기다리는 것
        public void Acquire() //획득하겠다
        {
            while (true)
            {
                //int original = Interlocked.Exchange(ref _locked, 1); // 1을 넣어주기 이전 값을 뱉어준다
                //if (original == 0)
                //    break;

                // CAS Compare-And-Swap
                int expected = 0;
                int desired = 1;
                if (Interlocked.CompareExchange(ref _locked, desired, expected) == expected)
                    break;
            }
        }

        public void Release() //획득한걸 내려놓겠다
        {
            _locked = 0;
        }
    }

    class Program
    {
        static int _num = 0;
        static SpinLock _lock = new SpinLock();

        static void Thread_1()
        {
            for (int i = 0; i < 10000; i++)
            {
                _lock.Acquire();
                _num++;
                _lock.Release();
            }
        }

        static void Thread_2()
        {
            for (int i = 0; i < 10000; i++)
            {
                _lock.Acquire();
                _num--;
                _lock.Release();
            }
        }

        static void Main(string[] args)
        {
            Task t1 = new Task(Thread_1);
            Task t2 = new Task(Thread_2);
            t1.Start();
            t2.Start();

            Task.WaitAll(t1, t2);

            Console.WriteLine(_num);
        }
    }
}
