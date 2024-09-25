﻿using Grafos.Classes;
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
            Console.WriteLine("9 - Verificar dependências.");
            Console.WriteLine("10 - Verificar máquinas quem podem trabalhar em paralelo.");
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
                        DisplayDependency(grafo);
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

    public static void DisplayDependency(Grafo grafo)
    {
        Console.WriteLine("\nVertices disponiveis:\n");

        for (var i = 0; i < grafo.Vertices.Count; i++)
        {
            Console.WriteLine($"{i + 1} - {grafo.Vertices[i].Nome}");
        }

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

    public static void RemoveVertice(Grafo grafo)
    {
        Console.WriteLine("\nEscolha qual o vértice que deseja remover:");
        Console.WriteLine(grafo.MostrarVertices());
        var verticeParaRemover = Console.ReadLine();
        grafo.RemoverVertice(verticeParaRemover);
        Console.WriteLine(grafo.ExibirMatriz());
        Console.WriteLine($"\nVertice {verticeParaRemover} removido com sucesso!");
    }

    public static void AddVertice(Grafo grafo)
    {
        Console.WriteLine("\nInforme o nome do vértice que deseja adicionar:");
        var vertice = Console.ReadLine();
        grafo.AdicionarVertice(vertice);
        Console.WriteLine(grafo.ExibirMatriz());
        Console.WriteLine($"\nVertice {vertice} adicionado com sucesso!");
    }

    public static void AddAresta(Grafo grafo)
    {
        Console.WriteLine("\nInforme o vértice de origem:");
        var origem = Console.ReadLine();
        Console.WriteLine("\nInforme o vértice de destino:");
        var destino = Console.ReadLine();
        Console.WriteLine("\nInforme o peso da aresta:");
        var peso = int.Parse(Console.ReadLine());
        grafo.AdicionarAresta(origem, destino, peso);
        Console.WriteLine($"\nAresta entre {origem /*origem.Nome*/} e {destino /*destino.Nome*/} adicionada com sucesso!");
        Console.WriteLine(grafo.ExibirMatriz());
    }

    public static void RemoveAresta(Grafo grafo)
    {
        Console.WriteLine("\nVértices disponiveis:\n");

        for (var i = 0; i < grafo.Vertices.Count; i++)
        {
            Console.WriteLine($"{i + 1} - {grafo.Vertices[i].Nome}");
        }

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

        Console.Write("\nInforme o código do vértice de origem:: ");
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
        Console.WriteLine("\nInforme o vértice que deseja consultar:");
        var vertice = Console.ReadLine();

        var continueDisplay = true;

        while (continueDisplay)
        {
            Console.Clear();
            grafo.ConsultarVertice(vertice);
            Console.WriteLine("\nPressione qualquer tecla para voltar ao menu...");
            Console.ReadKey();
            continueDisplay = false;
            Console.Clear();
        }
        
    }

    public static void ShowMaquinasParalelas (Grafo grafo)
    {
      

    }
}