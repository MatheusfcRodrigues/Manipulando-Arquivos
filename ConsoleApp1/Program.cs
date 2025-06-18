using System;
using System.IO;

class Program
{
    const int MAX = 100;
    static string cidade;
    static int capacidadeVIP, capacidadePrioritario, capacidadeComum;
    static string[] nomesVIP = new string[MAX];
    static int[] idadesVIP = new int[MAX];
    static int[] numerosVIP = new int[MAX];
    static int qtdVIP = 0;

    static string[] nomesPrioritario = new string[MAX];
    static int[] idadesPrioritario = new int[MAX];
    static int[] numerosPrioritario = new int[MAX];
    static int qtdPrioritario = 0;

    static string[] nomesComum = new string[MAX];
    static int[] idadesComum = new int[MAX];
    static int[] numerosComum = new int[MAX];
    static int qtdComum = 0;

    static string ultimoNomeEntrou = "", ultimoTipoEntrou = "";
    static int ultimoIdadeEntrou = 0, ultimoNumeroEntrou = 0;
    static string ultimoNomeSaiu = "", ultimoTipoSaiu = "";
    static int ultimoIdadeSaiu = 0, ultimoNumeroSaiu = 0;

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
                    RegistrarEntrada();
                    break;
                case 2:
                    RegistrarSaida();
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
        Console.WriteLine($"\n--- Evento em {cidade} ---");
        Console.WriteLine("1. Registrar entrada");
        Console.WriteLine("2. Registrar saída");
        Console.WriteLine("3. Consultar ingressos disponíveis");
        Console.WriteLine("4. Exibir resumo");
        Console.WriteLine("5. Listar espectadores");
        Console.WriteLine("6. Sair");
        Console.Write("Escolha uma opção: ");
        return int.Parse(Console.ReadLine());
    }

    static void RegistrarEntrada()
    {
        Console.Write("Nome: ");
        string nome = Console.ReadLine();
        Console.Write("Idade: ");
        int idade = int.Parse(Console.ReadLine());
        Console.Write("Número do Ingresso: ");
        int numero = int.Parse(Console.ReadLine());
        Console.Write("Tipo de Ingresso (VIP, Prioritario, Comum): ");
        string tipo = Console.ReadLine().ToLower();

        if (tipo == "vip")
        {
            if (qtdVIP < capacidadeVIP)
            {
                nomesVIP[qtdVIP] = nome;
                idadesVIP[qtdVIP] = idade;
                numerosVIP[qtdVIP] = numero;
                qtdVIP++;
                RegistrarUltimoEntrou(nome, idade, numero, "VIP");
                Console.WriteLine("Entrada registrada com sucesso!");
            }
            else Console.WriteLine("Não há ingressos VIP disponíveis.");
        }
        else if (tipo == "prioritario")
        {
            if (qtdPrioritario < capacidadePrioritario)
            {
                nomesPrioritario[qtdPrioritario] = nome;
                idadesPrioritario[qtdPrioritario] = idade;
                numerosPrioritario[qtdPrioritario] = numero;
                qtdPrioritario++;
                RegistrarUltimoEntrou(nome, idade, numero, "Prioritario");
                Console.WriteLine("Entrada registrada com sucesso!");
            }
            else Console.WriteLine("Não há ingressos Prioritários disponíveis.");
        }
        else if (tipo == "comum")
        {
            if (qtdComum < capacidadeComum)
            {
                nomesComum[qtdComum] = nome;
                idadesComum[qtdComum] = idade;
                numerosComum[qtdComum] = numero;
                qtdComum++;
                RegistrarUltimoEntrou(nome, idade, numero, "Comum");
                Console.WriteLine("Entrada registrada com sucesso!");
            }
            else Console.WriteLine("Não há ingressos Comuns disponíveis.");
        }
        else Console.WriteLine("Tipo inválido.");
    }

    static void RegistrarSaida()
    {
        Console.Write("Número do Ingresso: ");
        int numero = int.Parse(Console.ReadLine());
        Console.Write("Tipo de Ingresso (VIP, Prioritario, Comum): ");
        string tipo = Console.ReadLine().ToLower();

        if (tipo == "vip")
        {
            RemoverEspectador(numero, nomesVIP, idadesVIP, numerosVIP, ref qtdVIP, "VIP");
        }
        else if (tipo == "prioritario")
        {
            RemoverEspectador(numero, nomesPrioritario, idadesPrioritario, numerosPrioritario, ref qtdPrioritario, "Prioritario");
        }
        else if (tipo == "comum")
        {
            RemoverEspectador(numero, nomesComum, idadesComum, numerosComum, ref qtdComum, "Comum");
        }
        else Console.WriteLine("Tipo inválido.");
    }

    static void RemoverEspectador(int numero, string[] nomes, int[] idades, int[] numeros, ref int qtd, string tipo)
    {
        bool encontrado = false;
        for (int i = 0; i < qtd; i++)
        {
            if (numeros[i] == numero)
            {
                RegistrarUltimoSaiu(nomes[i], idades[i], numeros[i], tipo);
                for (int j = i; j < qtd - 1; j++)
                {
                    nomes[j] = nomes[j + 1];
                    idades[j] = idades[j + 1];
                    numeros[j] = numeros[j + 1];
                }
                qtd--;
                encontrado = true;
                Console.WriteLine("Saída registrada.");
                break;
            }
        }
        if (!encontrado)
            Console.WriteLine("Espectador não encontrado.");
    }

    static void ConsultarIngressos()
    {
        string saida = $"Ingressos disponíveis:\nVIP: {capacidadeVIP - qtdVIP}\nPrioritário: {capacidadePrioritario - qtdPrioritario}\nComum: {capacidadeComum - qtdComum}\n";
        Console.WriteLine(saida);
        File.WriteAllText("arquivo.txt", $"Cidade: {cidade}\n{saida}");
    }

    static void ExibirResumo()
    {
        int total = qtdVIP + qtdPrioritario + qtdComum;

        string resumo = $"Cidade: {cidade}\n";
        resumo += $"Total de espectadores: {total}\n";
        resumo += $"VIP: {qtdVIP} espectadores ({Percentual(qtdVIP, total):F2}%) | {capacidadeVIP - qtdVIP} ingressos disponíveis\n";
        resumo += $"Prioritário: {qtdPrioritario} espectadores ({Percentual(qtdPrioritario, total):F2}%) | {capacidadePrioritario - qtdPrioritario} ingressos disponíveis\n";
        resumo += $"Comum: {qtdComum} espectadores ({Percentual(qtdComum, total):F2}%) | {capacidadeComum - qtdComum} ingressos disponíveis\n";

        if (ultimoNomeEntrou != "")
            resumo += $"Último que entrou: {ultimoNomeEntrou}, {ultimoIdadeEntrou} anos, ingresso {ultimoNumeroEntrou} ({ultimoTipoEntrou})\n";
        if (ultimoNomeSaiu != "")
            resumo += $"Último que saiu: {ultimoNomeSaiu}, {ultimoIdadeSaiu} anos, ingresso {ultimoNumeroSaiu} ({ultimoTipoSaiu})\n";

        Console.WriteLine(resumo);
        File.WriteAllText("arquivo.txt", resumo);
    }

    static void ListarEspectadores()
    {
        void Listar(string tipo, string[] nomes, int[] idades, int[] numeros, int qtd)
        {
            Console.WriteLine($"\n{tipo.ToUpper()} ---");
            for (int i = 0; i < qtd; i++)
                Console.WriteLine($"Nome: {nomes[i]}, Idade: {idades[i]}, Ingresso: {numeros[i]}, Tipo: {tipo}");
        }

        Listar("VIP", nomesVIP, idadesVIP, numerosVIP, qtdVIP);
        Listar("Prioritário", nomesPrioritario, idadesPrioritario, numerosPrioritario, qtdPrioritario);
        Listar("Comum", nomesComum, idadesComum, numerosComum, qtdComum);
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

    static void RegistrarUltimoEntrou(string nome, int idade, int numero, string tipo)
    {
        ultimoNomeEntrou = nome;
        ultimoIdadeEntrou = idade;
        ultimoNumeroEntrou = numero;
        ultimoTipoEntrou = tipo;
    }

    static void RegistrarUltimoSaiu(string nome, int idade, int numero, string tipo)
    {
        ultimoNomeSaiu = nome;
        ultimoIdadeSaiu = idade;
        ultimoNumeroSaiu = numero;
        ultimoTipoSaiu = tipo;
    }
}
