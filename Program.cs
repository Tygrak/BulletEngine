using System;

namespace BulletEngine{
    public static class Program{
        [STAThread]
        static void Main(){
            using (var game = new BGame())
                game.Run();
        }
    }
}
