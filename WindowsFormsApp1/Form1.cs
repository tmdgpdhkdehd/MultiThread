using System;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private Thread[] threads; // 쓰레드 배열
        private bool isRunning; // 쓰레드 실행 여부
        public StringBuilder sb = new StringBuilder();

        public Form1()
        {
            InitializeComponent();
            threads = new Thread[5]; // 5개의 쓰레드 생성
            isRunning = false; // 초기값 false로 설정
        }

        // 쓰레드에서 실행할 메소드
        private void PrintId(int id)
        {
            try
            {
                bool[] isFirst = new bool[5] {true, true, true, true, true};
                int delay = 0;
                while (true)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        if (i == id) // 자신의 차례가 왔을 때
                        {
                            if (InvokeRequired)  // Invoke 예외 처리
                            {
                                if (isFirst[i])
                                {
                                    delay = id * 1000;
                                    isFirst[i] = false;
                                }
                                else
                                {
                                    delay = 5000;
                                }

                                Thread.Sleep(delay);

                                if (textBox1.InvokeRequired)
                                {
                                    sb.AppendLine(id.ToString());
                                    textBox1.Invoke(new MethodInvoker(delegate { textBox1.Text = sb.ToString(); }));
                                }
                                else
                                {
                                    textBox1.Text = sb.ToString(); ;
                                }

                                // 다음 스레드 실행을 위해 대기
                                Thread.Yield();
                            }
                        }
                    }
                }
            }
            catch (ThreadAbortException)
            {
                // 쓰레드 강제 종료 시 예외 처리
            }
        }

        // Start 버튼 클릭 이벤트 핸들러
        private void button1_Click(object sender, EventArgs e)
        {
            if (!isRunning) // 실행 중이 아니면
            {
                for (int i = 0; i < 5; i++)
                {
                    int id = i; // 각 쓰레드마다 고유한 ID 할당
                    threads[i] = new Thread(() => PrintId(id)); // 람다식으로 쓰레드 생성
                    threads[i].Start(); // 쓰레드 실행
                }
                isRunning = true; // 실행 중으로 설정
            }
        }

        // Stop 버튼 클릭 이벤트 핸들러
        private void button2_Click(object sender, EventArgs e)
        {
            if (isRunning) // 실행 중이면
            {
                for (int i = 0; i < 5; i++)
                {
                    threads[i].Abort(); // 쓰레드 강제 종료
                }
                isRunning = false; // 실행 중이 아니라고 설정
            }
        }
    }
}