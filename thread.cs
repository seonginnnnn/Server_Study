using System;
using System.Threading;
using System.Threading.Tasks;

namespace _Thread
{
    class Program
    {
        static void MainThread(object state)
        {
            for (int i = 0; i < 5; i++)
                Console.WriteLine("Hello Thread!");
        }
        static void Main(string[] args)
        {
            ThreadPool.SetMinThreads(1, 1); //최소 스레드
            ThreadPool.SetMaxThreads(5, 5); //최대 스레드
     
            for (int i = 0; i < 5; i++)
            {
                Task t = new Task(() => { while (true) { } }, TaskCreationOptions.LongRunning);
                // 긴 작업이 소요될 가능성이 있는 Task를 지정하여 스레드풀에서 따로 관리되게 만듬
                t.Start();
                // 스레드 풀에 제한이 5개 이지만 스레드 함수가 실행됨
            }

            //    ThreadPool.QueueUserWorkItem((obj) => { while (true) { } }); // 람다식 문법 : (입력 파라미터) => { 실행문장 블럭 };
            
            ThreadPool.QueueUserWorkItem(MainThread);

            //Thread t = new Thread(MainThread); //스레드 선언
            //t.IsBackground = true; // 백그라운드에서 실행되면 메인이 종료하면 MainThread가 실행되든 말든 같이 종료
            //t.Start(); 스레드 실행

            //t.Join(); //MainThread 끝날때 까지 대기

            while (true)
            {

            }
        }
    }
}
