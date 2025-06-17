using System;
using System.IO;
using System.Collections.Generic;

class Program
{
    static string cidade;
    static int capacidadeVIP, capacidadePrioritario, capacidadeComum;
    static List<Espectador> vip = new List<Espectador>();
    static List<Espectador> prioritario = new List<Espectador>();
    static List<Espectador> comum = new List<Espectador>();
    static Espectador ultimoEntrou = null;
    static Espectador ultimoSaiu = null;

    class Espectador
    {
        public string Nome;
        public int Idade;
        public int Numero;
        public string Tipo;
    }

    static void Main()
    {
        LerArquivoEntrada();

        int opcao;
        do
        {
            opcao = Menu();
            switch (opcao)
            {
                case 1:
                    Console.Write("Nome: ");
                    string nome = Console.ReadLine();
                    Console.Write("Idade: ");
                    int idade = int.Parse(Console.ReadLine());
                    Console.Write("Número do Ingresso: ");
                    int numero = int.Parse(Console.ReadLine());
                    Console.Write("Tipo de Ingresso (VIP, Prioritario, Comum): ");
                    string tipo = Console.ReadLine();
                    RegistrarEntrada(nome, idade, numero, tipo);
                    break;

                case 2:
                    Console.Write("Número do Ingresso: ");
                    int numeroS = int.Parse(Console.ReadLine());
                    Console.Write("Tipo de Ingresso (VIP, Prioritario, Comum): ");
                    string tipoS = Console.ReadLine();
                    RegistrarSaida(numeroS, tipoS);
                    break;

                case 3:
                    ConsultarIngressos();
                    break;

                case 4:
                    ExibirResumo();
                    break;

                case 5:
                    ListarEspectadores();
                    break;
            }
        } while (opcao != 6);
    }

    static int Menu()
    {
        Console.WriteLine($"--- Evento em {cidade} ---");
        Console.WriteLine("1. Registrar entrada");
        Console.WriteLine("2. Registrar saída");
        Console.WriteLine("3. Consultar ingressos disponíveis");
        Console.WriteLine("4. Exibir resumo");
        Console.WriteLine("5. Listar espectadores");
        Console.WriteLine("6. Sair");
        Console.Write("Escolha uma opção: ");
        return int.Parse(Console.ReadLine());
    }

    static void RegistrarEntrada(string nome, int idade, int numeroIngresso, string tipo)
    {
        var espectador = new Espectador { Nome = nome, Idade = idade, Numero = numeroIngresso, Tipo = tipo };
        switch (tipo.ToLower())
        {
            case "vip":
                if (vip.Count < capacidadeVIP)
                {
                    vip.Add(espectador);
                    ultimoEntrou = espectador;
                    Console.WriteLine("Entrada registrada com sucesso!");
                }
                else Console.WriteLine("Não há ingressos VIP disponíveis.");
                break;

            case "prioritario":
                if (prioritario.Count < capacidadePrioritario)
                {
                    prioritario.Add(espectador);
                    ultimoEntrou = espectador;
                    Console.WriteLine("Entrada registrada com sucesso!");
                }
                else Console.WriteLine("Não há ingressos Prioritários disponíveis.");
                break;

            case "comum":
                if (comum.Count < capacidadeComum)
                {
                    comum.Add(espectador);
                    ultimoEntrou = espectador;
                    Console.WriteLine("Entrada registrada com sucesso!");
                }
                else Console.WriteLine("Não há ingressos Comuns disponíveis.");
                break;

            default:
                Console.WriteLine("Tipo inválido.");
                break;
        }
    }

    static void RegistrarSaida(int numeroIngresso, string tipo)
    {
        List<Espectador> lista = null;
    switch (tipo.ToLower())
    {
    case "vip":
        lista = vip;
        break;
    case "prioritario":
        lista = prioritario;
        break;
    case "comum":
        lista = comum;
        break;
    }

        if (lista != null)
        {
            var espectador = lista.Find(e => e.Numero == numeroIngresso);
            if (espectador != null)
            {
                lista.Remove(espectador);
                ultimoSaiu = espectador;
                Console.WriteLine("Saída registrada.");
            }
            else Console.WriteLine("Espectador não encontrado.");
        }
        else Console.WriteLine("Tipo inválido.");
    }

    static void ConsultarIngressos()
    {
        int vipDisp = capacidadeVIP - vip.Count;
        int priDisp = capacidadePrioritario - prioritario.Count;
        int comDisp = capacidadeComum - comum.Count;

        string saida = $"Ingressos disponíveis:\nVIP: {vipDisp}\nPrioritário: {priDisp}\nComum: {comDisp}\n";
        Console.WriteLine(saida);
        File.WriteAllText("arquivo.txt", $"Cidade: {cidade}\n{saida}");
    }

    static void ExibirResumo()
    {
        int total = vip.Count + prioritario.Count + comum.Count;
        int vipDisp = capacidadeVIP - vip.Count;
        int priDisp = capacidadePrioritario - prioritario.Count;
        int comDisp = capacidadeComum - comum.Count;

        string resumo = $"Cidade: {cidade}\n";
        resumo += $"Total de espectadores: {total}\n";
        resumo += $"VIP: {vip.Count} espectadores ({Percentual(vip.Count, total):F2}%) | {vipDisp} ingressos disponíveis\n";
        resumo += $"Prioritário: {prioritario.Count} espectadores ({Percentual(prioritario.Count, total):F2}%) | {priDisp} ingressos disponíveis\n";
        resumo += $"Comum: {comum.Count} espectadores ({Percentual(comum.Count, total):F2}%) | {comDisp} ingressos disponíveis\n";

        if (ultimoEntrou != null)
            resumo += $"Último que entrou: {ultimoEntrou.Nome}, {ultimoEntrou.Idade} anos, ingresso {ultimoEntrou.Numero} ({ultimoEntrou.Tipo})\n";
        if (ultimoSaiu != null)
            resumo += $"Último que saiu: {ultimoSaiu.Nome}, {ultimoSaiu.Idade} anos, ingresso {ultimoSaiu.Numero} ({ultimoSaiu.Tipo})\n";

        Console.WriteLine(resumo);
        File.WriteAllText("arquivo.txt", resumo);
    }

    static void ListarEspectadores()
    {
        void ListarCategoria(string tipo, List<Espectador> lista)
        {
            Console.WriteLine($"{tipo.ToUpper()} ---");
            foreach (var e in lista)
                Console.WriteLine($"Nome: {e.Nome}, Idade: {e.Idade}, Ingresso: {e.Numero}, Tipo: {e.Tipo}");
        }

        ListarCategoria("VIP", vip);
        ListarCategoria("Prioritário", prioritario);
        ListarCategoria("Comum", comum);
    }

    static void LerArquivoEntrada()
    {
        if (!File.Exists("arquivo.txt"))
        {
            Console.WriteLine("Arquivo 'arquivo.txt' não encontrado.");
            Environment.Exit(1);
        }

        string[] linhas = File.ReadAllLines("arquivo.txt");
        if (linhas.Length < 4)
        {
            Console.WriteLine("Arquivo 'arquivo.txt' está incompleto.");
            Environment.Exit(1);
        }

        cidade = linhas[0];
        capacidadeVIP = int.Parse(linhas[1]);
        capacidadePrioritario = int.Parse(linhas[2]);
        capacidadeComum = int.Parse(linhas[3]);
    }

    static double Percentual(int parte, int total)
    {
        return total == 0 ? 0 : (parte * 100.0) / total;
    }
}
