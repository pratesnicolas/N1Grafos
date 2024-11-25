using Grafos.Classes;
using Grafos.Utils;

namespace Grafos;

public sealed class Program
{
    private static void Main()
    {
        Console.WriteLine("1 - Informe o local do arquivo .csv que contém as informações do grafo:");
        string caminho = Console.ReadLine();

        var grafo = CsvReader.ProcessarCsv(caminho);
        var continueRunning = true;

        while (continueRunning)
        {
            Console.WriteLine(grafo.ExibirMatriz());
            Console.WriteLine("---- MENU ----");
            Console.WriteLine("1 - Adicionar vértice.");
            Console.WriteLine("2 - Remover vértice.");
            Console.WriteLine("3 - Adicionar aresta.");
            Console.WriteLine("4 - Remover aresta.");
            Console.WriteLine("5 - Atualizar aresta.");
            Console.WriteLine("6 - Atualizar vértice.");
            Console.WriteLine("7 - Consultar aresta.");
            Console.WriteLine("8 - Consultar vértice.");
            Console.WriteLine("9 - Submenu: Funções.");
            Console.WriteLine("10 - Listar informações do grafo.");
            Console.WriteLine("11 - Exibir pesos.");
            Console.WriteLine("12 - Djikstra");
            Console.WriteLine("0 - Encerrar.");

            Console.WriteLine("\nEscolha sua opção: ");
            var opcaoInput = Console.ReadLine();

            if (int.TryParse(opcaoInput, out var opcao))
            {
                switch (opcao)
                {
                    case 0:
                        continueRunning = false;
                        break;
                    case 1:
                        AddVertice(grafo);
                        break;
                    case 2:
                        RemoveVertice(grafo);
                        break;
                    case 3:
                        AddAresta(grafo);
                        break;
                    case 4:
                        RemoveAresta(grafo);
                        break;
                    case 5:
                        UpdateAresta(grafo);
                        break;
                    case 6:
                        UpdateVertice(grafo);
                        break;
                    case 7:
                        ShowAresta(grafo);
                        break;
                    case 8:
                        ShowVertice(grafo);
                        break;
                    case 9:
                        ShowFunctionsMenu(grafo);
                        break;
                    case 10:
                        MostrarCaracteristicas(grafo);
                        break;
                    case 11:
                        ExibirMatrizPesos(grafo);
                        break;
                    case 12:
                        Djisktra(grafo);
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Opcao invalida. Tente novamente.");
                        break;
                }
            }
            else
            {
                switch (-1)
                {
                    default:
                        Console.Clear();
                        Console.WriteLine("Opcao invalida. Tente novamente.");
                     break;
                }
            }

            Console.Clear();
        }
    }

    public static void DisplayDependency(Grafo grafo)
    {
        Console.WriteLine("\nVertices disponiveis:\n");

        ShowVertices(grafo);

        Console.Write("\nInforme o vertice que deseja verificar as dependencias: ");
        var idxVertice = int.Parse(Console.ReadLine()) - 1;

        var continueDisplay = true;

        while (continueDisplay)
        {
            Console.Clear();
            grafo.VerificarDependencias(idxVertice);
            Console.WriteLine("\nPressione qualquer tecla para voltar ao menu...");
            Console.ReadKey();
            continueDisplay = false;
            Console.Clear();
        }
    }

    public static void ShowFunctionsMenu(Grafo grafo)
    {
        var continueRunning = true;

        while (continueRunning)
        {
            Console.Clear();
            Console.WriteLine("---- SUBMENU - FUNÇÕES DO SISTEMA ----");
            Console.WriteLine("1 - Verificar dependências de uma máquina específica.");
            Console.WriteLine("2 - Verificar o tempo da linha de produção a partir de uma máquina específica.");
            Console.WriteLine("3 - Exibir caminho crítico.");
            Console.WriteLine("4 - Exibir máquinas independentes.");
            Console.WriteLine("5 - Exibir o maior gargalo entre as máquinas.");
            Console.WriteLine("0 - Voltar ao menu principal.");

            Console.WriteLine("\nEscolha sua opção: ");
            var opcaoInput = Console.ReadLine();

            if (int.TryParse(opcaoInput, out var opcao))
            {
                switch (opcao)
                {
                    case 0:
                        continueRunning = false;
                        break;
                    case 1:
                        DisplayDependency(grafo);
                        break;
                    case 2:
                        VerifyProductionTime(grafo);
                        break;
                    case 3:
                        GetCriticalPath(grafo);
                        break;
                    case 4:
                        MostrarMaquinasIndependentes(grafo);
                        break;
                    case 5:
                        MostrarMaiorGargalo(grafo);
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Opcao invalida. Tente novamente.");
                        break;
                }
            }
            else
            {
                switch (-1)
                {
                    default:
                        Console.Clear();
                        Console.WriteLine("Opcao invalida. Tente novamente.");
                        break;
                }
            }
        }
    }

    public static void RemoveVertice(Grafo grafo)
    {
        Console.WriteLine("\nEscolha qual o vértice que deseja remover:");
        ShowVertices(grafo);

        var indexVertice = int.Parse(Console.ReadLine()) - 1;

        var vertice = grafo.Vertices[indexVertice].Nome;
        grafo.RemoverVertice(vertice);
        Console.WriteLine(grafo.ExibirMatriz());
        Console.WriteLine($"\nVertice {vertice} removido com sucesso!");
    }

    public static void AddVertice(Grafo grafo)
    {
        Console.WriteLine("\nInforme o nome do vértice que deseja adicionar:");
        var vertice = Console.ReadLine();
        var continueDisplay = true;

        while (continueDisplay)
        {
            Console.Clear();
            grafo.AdicionarVertice(vertice);
            Console.WriteLine(grafo.ExibirMatriz());
            Console.WriteLine("\nPressione qualquer tecla para voltar ao menu...");
            Console.ReadKey();
            continueDisplay = false;
        }

    }

    public static void AddAresta(Grafo grafo)
    {
        Console.WriteLine("\nVértices disponiveis:\n");

        for (var i = 0; i < grafo.Vertices.Count; i++)
        {
            Console.WriteLine($"{i + 1} - {grafo.Vertices[i].Nome}");
        }

        Console.WriteLine("\nInforme o código do vértice de origem:");
        var indexOrigem = int.Parse(Console.ReadLine()) - 1;
        Console.WriteLine("\nInforme o código do vértice de destino:");
        var indexDestino = int.Parse(Console.ReadLine()) -1;
        Console.WriteLine("\nInforme o peso da aresta:");
        var peso = int.Parse(Console.ReadLine());

        var origem = grafo.Vertices[indexOrigem].Nome;
        var destino = grafo.Vertices[indexDestino].Nome;
        grafo.AdicionarAresta(origem, destino, peso);
        Console.WriteLine($"\nAresta entre {origem} e {destino} adicionada com sucesso!");
        Console.WriteLine(grafo.ExibirMatriz());
    }

    public static void RemoveAresta(Grafo grafo)
    {
        Console.WriteLine("\nVértices disponiveis:\n");
        ShowVertices(grafo);
        
        Console.Write("\nInforme o código do vértice de origem: ");
        var idxStartVertice = int.Parse(Console.ReadLine()) - 1;
        Console.WriteLine("\nInforme o código do vértice de destino: ");
        var idxEndVertice = int.Parse(Console.ReadLine()) - 1;

        var continueDisplay = true;

        while (continueDisplay)
        {
            Console.Clear();
            grafo.RemoverAresta(grafo.Vertices[idxStartVertice], grafo.Vertices[idxEndVertice]);
            Console.WriteLine("\nPressione qualquer tecla para voltar ao menu...");
            Console.ReadKey();
            continueDisplay = false;
            Console.Clear();
        }

    }

    public static void UpdateAresta(Grafo grafo)
    {
        Console.WriteLine("\nVértices disponiveis:\n");

        for (var i = 0; i < grafo.Vertices.Count; i++)
        {
            Console.WriteLine($"{i + 1} - {grafo.Vertices[i].Nome}");
        }

        Console.Write("\nInforme o código do vértice de origem: ");
        var idxStartVertice = int.Parse(Console.ReadLine()) - 1;
        Console.WriteLine("\nInforme o código do vértice de destino:");
        var idxEndVertice = int.Parse(Console.ReadLine()) - 1;
        Console.WriteLine("\nInforme o novo peso da aresta:");
        var peso = int.Parse(Console.ReadLine());

        var continueDisplay = true;

        while (continueDisplay)
        {
            Console.Clear();
            grafo.AtualizarAresta(grafo.Vertices[idxStartVertice].Nome, grafo.Vertices[idxEndVertice].Nome, peso);
            Console.WriteLine("\nPressione qualquer tecla para voltar ao menu...");
            Console.ReadKey();
            continueDisplay = false;
            Console.Clear();
        }
    }

    public static void UpdateVertice(Grafo grafo)
    {
        Console.WriteLine("\nVértices disponiveis:\n");

        for (var i = 0; i < grafo.Vertices.Count; i++)
        {
            Console.WriteLine($"{i + 1} - {grafo.Vertices[i].Nome}");
        }

        Console.WriteLine("\nInforme o código do vértice que deseja atualizar:");
        var idxVertice = int.Parse(Console.ReadLine()) - 1;

        Console.WriteLine("\nInforme o novo nome do vértice:");
        var novoNomeVertice = Console.ReadLine();

        var continueDisplay = true;

        while (continueDisplay)
        {
            Console.Clear();
            grafo.AtualizarVertice(grafo.Vertices[idxVertice], novoNomeVertice);
            Console.WriteLine("\nPressione qualquer tecla para voltar ao menu...");
            Console.ReadKey();
            continueDisplay = false;
            Console.Clear();
        }

    }

    public static void ShowAresta(Grafo grafo)
    {
        Console.WriteLine("\nVértices disponiveis:\n");

        for (var i = 0; i < grafo.Vertices.Count; i++)
        {
            Console.WriteLine($"{i + 1} - {grafo.Vertices[i].Nome}");
        }

        Console.WriteLine("\nInforme o código do vértice de origem da aresta:");
        var idxStartVertice = int.Parse(Console.ReadLine()) - 1;

        Console.WriteLine("\nInforme o código do vértice de destino da aresta:");
        var idxEndVertice = int.Parse(Console.ReadLine()) - 1;

        var continueDisplay = true;

        while (continueDisplay)
        {
            Console.Clear();
            grafo.ConsultarAresta(grafo.Vertices[idxStartVertice], grafo.Vertices[idxEndVertice]);
            Console.WriteLine("\nPressione qualquer tecla para voltar ao menu...");
            Console.ReadKey();
            continueDisplay = false;
            Console.Clear();
        }
       
    }

    public static void ShowVertice(Grafo grafo)
    {
        Console.WriteLine("\nVértices disponiveis:\n");

        for (var i = 0; i < grafo.Vertices.Count; i++)
        {
            Console.WriteLine($"{i + 1} - {grafo.Vertices[i].Nome} ({grafo.Vertices[i].Apelido})");
        }

        Console.WriteLine("\nInforme o código do vértice que deseja consultar:");
        var idxVertice = int.Parse(Console.ReadLine()) - 1;

        var continueDisplay = true;

        while (continueDisplay)
        {
            Console.Clear();
            grafo.ConsultarVertice(grafo.Vertices[idxVertice]);
            Console.WriteLine("\nPressione qualquer tecla para voltar ao menu...");
            Console.ReadKey();
            continueDisplay = false;
            Console.Clear();
        }
        
    }

    public static void VerifyProductionTime(Grafo grafo)
    {
        Console.WriteLine("\nSelecione a máquina inicial:");

        ShowVertices(grafo);

        var opcaoInput = Console.ReadLine();
        var continueDisplay = true;

        if (int.TryParse(opcaoInput, out var opcao) && opcao >= 1 && opcao <= grafo.Vertices.Count)
        {
            var maquinaInicial = grafo.Vertices[opcao - 1];

            while (continueDisplay)
            {
                Console.Clear();
                grafo.MostrarTempoProducaoMaquinas(maquinaInicial);
                Console.WriteLine("\nPressione qualquer tecla para voltar ao menu...");
                Console.ReadKey();
                continueDisplay = false;
                Console.Clear();
            }
        }
        else
        {
            Console.WriteLine("Opção inválida. Tente novamente.");
        }
    }

    public static void GetCriticalPath(Grafo grafo)
    {

        var continueDisplay = true;

        while (continueDisplay)
        {
            Console.Clear();
            grafo.ExibirCaminhoCritico();
            Console.WriteLine("\nPressione qualquer tecla para voltar ao menu...");
            Console.ReadKey();
            continueDisplay = false;
            Console.Clear();
        }

    }

    public static void MostrarMaiorGargalo(Grafo grafo)
    {
        var continueDisplay = true;

        while (continueDisplay)
        {
            Console.Clear();
            grafo.VerificarMaiorPeso();
            Console.WriteLine("\nPressione qualquer tecla para voltar ao menu...");
            Console.ReadKey();
            continueDisplay = false;
            Console.Clear();
        }
    }

    public static void MostrarMaquinasIndependentes(Grafo grafo)
    {
        var continueDisplay = true;

        while (continueDisplay)
        {
            Console.Clear();
            grafo.EncontrarVerticesIndependentes();
            Console.WriteLine("\nPressione qualquer tecla para voltar ao menu...");
            Console.ReadKey();
            continueDisplay = false;
            Console.Clear();
        }
    }

    public static void MostrarCaracteristicas(Grafo grafo)
    {
        var continueDisplay = true;

        while (continueDisplay)
        {
            Console.Clear();
            grafo.MostrarCaracteristicas();
            Console.WriteLine("\nPressione qualquer tecla para voltar ao menu...");
            Console.ReadKey();
            continueDisplay = false;
            Console.Clear();
        }
    }

    public static void ExibirMatrizPesos(Grafo grafo)
    {
        var continueDisplay = true;

        while (continueDisplay)
        {
            Console.Clear();
            grafo.ExibirMatrizPesos();
            Console.WriteLine("\nPressione qualquer tecla para voltar ao menu...");
            Console.ReadKey();
            continueDisplay = false;
            Console.Clear();
        }
    }


    public static void Djisktra(Grafo grafo)
    {
        Console.WriteLine("\nVértices disponiveis:\n");

        for (var i = 0; i < grafo.Vertices.Count; i++)
        {
            Console.WriteLine($"{i + 1} - {grafo.Vertices[i].Nome}");
        }

        Console.WriteLine("\nInforme o código do vértice de origem:");
        var idxStartVertice = int.Parse(Console.ReadLine()) - 1;

        Console.WriteLine("\nInforme o código do vértice de destino");
        var idxEndVertice = int.Parse(Console.ReadLine()) - 1;

        var continueDisplay = true;

        while (continueDisplay)
        {
            Console.Clear();
            grafo.Dijkstra(idxStartVertice, idxEndVertice);
            Console.WriteLine("\nPressione qualquer tecla para voltar ao menu...");
            Console.ReadKey();
            continueDisplay = false;
            Console.Clear();
        }
    }
    public static void ShowVertices(Grafo grafo)
    {
        for (var i = 0; i < grafo.Vertices.Count; i++)
        {
            Console.WriteLine($"{i + 1} - {grafo.Vertices[i].Nome}");
        }
    }
} 