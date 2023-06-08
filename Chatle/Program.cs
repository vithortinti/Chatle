using APSRede.Conversation;
using APSRede.Conversation.Interfaces;
using System.Net.Sockets;

public class Program
{
    public static void Main(string[] args)
    {
        while (true)
        {
            try
            {
                int choice = StartMenuOptions();
                Console.Clear();
                InitConversation(choice);
                break;
            }
            catch (Exception ex)
            {
                Console.Clear();
                WriteLineErrorMessage(ex.Message);
                continue;
            }
        }
    }

    private static void InitConversation(int option)
    {
        switch (option)
        {
            case 1:
                Console.Write("Insira uma porta válida para iniciar a conversa: ");
                int port = int.Parse(Console.ReadLine());
                HostChat hostChat = new HostChat(port);
                Console.WriteLine($"IP: {hostChat.IpAddress}\nPorta: {hostChat.Port}\nAguardando conexão...");
                var client = hostChat.Start();
                WriteLineSucessMessage($"Conexão estabelecida com sucesso!\nCliente: {client}");
                WaitMessageInterface(hostChat);
                SendMessageInterface(hostChat);
                break;
            case 2:
                Console.WriteLine("Insira o IP e porta do host em que deseja conectar-se:");
                Console.Write("IP: ");
                string hostIp = Console.ReadLine();
                Console.Write("Porta: ");
                int hostPort = int.Parse(Console.ReadLine());
                ClientChat clientChat = new ClientChat(hostIp, hostPort);
                WriteLineSucessMessage("Conexão estabelecida com sucesso!");
                WaitMessageInterface(clientChat);
                SendMessageInterface(clientChat);
                break;
        }
    }

    private static int StartMenuOptions()
    {
        Console.WriteLine("********************************");
        Console.WriteLine("************ CHATLE ************");
        Console.WriteLine("********************************\n");

        Console.Write("1 - Abrir uma conversa na rede\n" +
            "2 - Conectar-se a um host\n" +
            "-> ");

        int choice = int.Parse(Console.ReadLine());

        if (choice < 1 || choice > 2)
        {
            throw new Exception("Escolha uma opção válida");
        }

        return choice;
    }

    private static void WriteLineErrorMessage(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.ForegroundColor = ConsoleColor.White;
    }

    private static void WriteLineSucessMessage(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(message);
        Console.ForegroundColor = ConsoleColor.White;
    }

    private static void SendMessageInterface(IChatMessage chat)
    {
        while (true)
        {
            string message = Console.ReadLine().Trim();
            if (string.IsNullOrEmpty(message))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("(Mensagens vazias não são enviadas)");
                Console.ForegroundColor = ConsoleColor.White;
                continue;
            }

            try
            {
                chat.SendMessage(message);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("(Enviada e recebida)");
                Console.ForegroundColor = ConsoleColor.White;
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("(Não enviada e nem recebida)");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }

    private static void WaitMessageInterface(IChatMessage chat)
    {
        Thread thread = new Thread(() =>
        {
            while (true)
            {
                try
                {
                    string message = chat.WaitMessage();
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write(chat.ConnectedIpAddress + " ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write($"> {message}");
                    Console.WriteLine();
                }
                catch (Exception ex)
                {
                    WriteLineErrorMessage("Conexão encerrada.");
                    Console.ReadKey();
                    Environment.Exit(0);
                }
            }
        });
        thread.Start();
    }
}