using System.Runtime;

namespace Grafos.Classes;

public class Dijkstra
{
    private int DistanciaMinima(int[] distancias, bool[] visitados, int qtdVertices)
    {
        int min = int.MaxValue;
        int minIndex = -1;

        for (int v = 0; v < qtdVertices; v++)
        {
            if (visitados[v] == false && distancias[v] <= min)
            {
                min = distancias[v];
                minIndex = v;
            }
        }

        return minIndex;
    }

    private void MostrarCaminho(int[] distancias, int[] pai, int origem, int destino)
    {
        Console.Write("Caminho mínimo entre {0} e {1}: ", origem, destino);

        int crawl = destino;
        Console.Write(crawl);

        while (pai[crawl] != -1)
        {
            Console.Write(" <- {0}", pai[crawl]);
            crawl = pai[crawl];
        }

        Console.WriteLine("\nCusto total: {0}", distancias[destino]);
    }

    public void AlgoritmoDijkstra(int[,] graph, int origem, int destino, int qtdVertices)
    {
        int[] distances = new int[qtdVertices]; // vetor para armazenar as distâncias mínimas
        bool[] visited = new bool[qtdVertices]; // vetor para marcar os vértices visitados
        int[] parent = new int[qtdVertices]; // vetor para armazenar os pais dos vértices

        // Inicializa os valores das distâncias, visitados e pais
        for (int i = 0; i < qtdVertices; i++)
        {
            distances[i] = int.MaxValue;
            visited[i] = false;
            parent[i] = -1;
        }

        distances[origem] = 0; // a distância do vértice inicial é sempre 0

        // Encontra o caminho mínimo para todos os vértices
        for (int count = 0; count < qtdVertices - 1; count++)
        {
            int u = DistanciaMinima(distances, visited, qtdVertices);
            visited[u] = true;

            for (int v = 0; v < qtdVertices; v++)
            {
                if (!visited[v] && graph[u, v] != 0 && distances[u] != int.MaxValue && distances[u] + graph[u, v] < distances[v])
                {
                    distances[v] = distances[u] + graph[u, v];
                    parent[v] = u;
                }
            }
        }

        // Imprime o caminho mínimo entre o vértice inicial e o vértice destino
        MostrarCaminho(distances, parent, origem, destino);
    }

}
