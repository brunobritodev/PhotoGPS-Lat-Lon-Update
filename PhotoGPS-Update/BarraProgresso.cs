namespace PhotoGPS_Update
{
    internal class BarraProgresso
    {
        public static void ExibirBarraDeProgresso(int progresso, int tamanhoTotal, int barraTamanho = 50)
        {
            Console.CursorVisible = false;
            var corOriginal = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkGreen;

            var cursorTop = Console.CursorTop;
            var cursorLeft = Console.CursorLeft;

            Console.Write("[");
            var posicaoNaBarra = (int)(progresso * (double)barraTamanho / tamanhoTotal);
            for (int i = 0; i < barraTamanho; i++)
            {
                if (i < posicaoNaBarra) Console.Write("█");
                else if (i == posicaoNaBarra) Console.Write(">");
                else Console.Write(" ");
            }
            Console.Write("] ");
            var percentual = (int)(progresso * 100.0 / tamanhoTotal);
            Console.Write($"{percentual}% - {progresso}");

            Console.SetCursorPosition(cursorLeft, cursorTop);
            Console.ForegroundColor = corOriginal;
            Console.CursorVisible = true;
        }
        public static void ExibirQuantidade(int itens, string textoBase)
        {
            Console.CursorVisible = false;
            var corOriginal = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.White;

            var cursorTop = Console.CursorTop;
            var cursorLeft = Console.CursorLeft;

            Console.Write("[ ");
            Console.Write($"{textoBase}: {itens:D}");
            Console.Write(" ] ");

            Console.SetCursorPosition(cursorLeft, cursorTop);
            Console.ForegroundColor = corOriginal;
            Console.CursorVisible = true;
        }
    }
}
