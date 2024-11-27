namespace Grafos.Classes;

public class FloydWarshall
{
    
    public void AlgoritmoFloydWarshall(int[,] graph, List<Vertice> vertices)
    {
        int qtdVertices = vertices.Count;

        //Primeiro, inicializamos a matriz de distâncias com base na quantidade de vertices.
        int[,] distancia = new int[qtdVertices, qtdVertices];
        int[,] predecessores = new int[qtdVertices, qtdVertices]; //Inicialização dos predecessores para mostrar o caminho posteriormente.

        for (int i = 0; i < qtdVertices; ++i)
        {
            for (int j = 0; j < qtdVertices; ++j)
            {
                if (i == j)
                {
                    distancia[i, j] = 0; //A distância do vértice para si mesmo é 0.
                    predecessores[i, j] = -1; // Sem predecessor
                }
                else if (graph[i, j] != 0)
                {
                    distancia[i, j] = graph[i, j]; //A distância mínima posível é peso desses vértices.
                    predecessores[i, j] = i; // Inicializa o predecessor como o nó de origem
                }
                else
                {
                    distancia[i, j] = int.MaxValue; //Caso não haja nenhuma arestra entre dois vértices, isso será representado com o valor infinito (int.MaxValue).
                    predecessores[i, j] = -1; // Sem caminho conhecido
                }
            }
        }

        // Executa o algoritmo de Floyd-Warshall
        for (int k = 0; k < qtdVertices; ++k) // Usando os vértices 0...k como pontos intermedíariios, o caminho mais curto entre i e j é dado por k.
        {
            for (int i = 0; i < qtdVertices; ++i)
            {
                for (int j = 0; j < qtdVertices; ++j)
                {
                    if (distancia[i, k] != int.MaxValue && distancia[k, j] != int.MaxValue)
                    {
                        int novaDistancia = distancia[i, k] + distancia[k, j]; //A fórmula usada para calcular o camiho mais curto etre i e j.
                        if (novaDistancia < distancia[i, j])
                        {
                            distancia[i, j] = novaDistancia; //Se um um novo valor para o caminho mais cursto for encontrado, nós armazenamos ele nesta linha.
                            predecessores[i, j] = predecessores[k, j]; // Atualiza o predecessor
                        }
                    }
                }
            }
        }

        //Função para mostrar o caminho.

        MostrarCaminhos(distancia, predecessores, vertices);
    }

    private void MostrarCaminhos(int[,] distancia, int[,] predecessores, List<Vertice> vertices)
    {
        int qtdVertices = vertices.Count;

        Console.WriteLine("Matriz de distâncias mínimas:");
        Console.Write("".PadLeft(10));
        foreach (var v in vertices)
        {
            Console.Write(v.Apelido.PadLeft(10));
        }
        Console.WriteLine();

        for (int i = 0; i < qtdVertices; ++i)
        {
            Console.Write(vertices[i].Apelido.PadLeft(10));
            for (int j = 0; j < qtdVertices; ++j)
            {
                if (distancia[i, j] == int.MaxValue)
                    Console.Write("INF".PadLeft(10));
                else
                    Console.Write(distancia[i, j].ToString().PadLeft(10));
            }
            Console.WriteLine();
        }

        Console.WriteLine("\nCaminhos mínimos entre todos os pares de vértices:");
        for (int i = 0; i < qtdVertices; ++i)
        {
            for (int j = 0; j < qtdVertices; ++j)
            {
                if (i != j && distancia[i, j] != int.MaxValue)
                {
                    Console.WriteLine($"{vertices[i].Nome}({vertices[i].Apelido}) para {vertices[j].Nome}({vertices[j].Apelido}):");
                    ImprimirTodosOsCaminhos(predecessores, vertices, i, j);
                    Console.WriteLine($" (Custo do caminho ótimo: {distancia[i, j]})\n");
                }
            }
        }
    }

    private void ImprimirTodosOsCaminhos(int[,] predecessores, List<Vertice> vertices, int inicio, int fim)
    {
        if (inicio == fim)
        {
            Console.Write(vertices[inicio].Nome);
            return;
        }

        if (predecessores[inicio, fim] == -1)
        {
            Console.Write("Nenhum caminho");
            return;
        }

        // Caminho ótimo de início ao predecessor
        ImprimirTodosOsCaminhos(predecessores, vertices, inicio, predecessores[inicio, fim]);
        Console.Write($" -> {vertices[fim].Nome}");
    }
}
