using System.Runtime;

namespace Grafos.Classes;

public class Dijkstra
{

    // Método para encontrar o índice do vértice com o menor custo .
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

    //Função auxiliar para imprimir o caminho
    private void MostrarCaminho(int[] distancias, int[] pai, int origem, int destino, List<Vertice> vertices)
    {
        if (distancias[destino] == int.MaxValue)
        {
            Console.WriteLine("Não existe um caminho de {0} a {1}", vertices[origem].Nome, vertices[destino].Nome);
            return;
        }

        Console.Write("Caminho mínimo entre {0} e {1}: ", vertices[origem].Nome, vertices[destino].Nome);

        // Cria uma pilha para armazenar o caminho
        Stack<string> caminho = new Stack<string>();

        // Rastrear o caminho a partir do destino
        int crawl = destino;
        while (crawl != origem)
        {
            caminho.Push(vertices[crawl].Nome);  // Adiciona o vértice ao caminho
            crawl = pai[crawl];  // Move para o vértice anterior
        }

        // Adicionar o vértice de origem ao caminho
        caminho.Push(vertices[origem].Nome);

        while (caminho.Count > 0)
        {
            Console.Write(caminho.Pop());
            if (caminho.Count > 0)
            {
                Console.Write(" -> ");
            }
        }

        Console.WriteLine("\nCusto total: {0}", distancias[destino]);
    }

    public void AlgoritmoDijkstra(int[,] grafo, int origem, int destino, int qtdVertices, List<Vertice> vertices)
    {
        int[] distances = new int[qtdVertices]; // vetor para armazenar as distâncias mínimas
        bool[] visited = new bool[qtdVertices];  // vetor para marcar os vértices visitados
        int[] parent = new int[qtdVertices]; // vetor para armazenar os pais dos vértices


        // Em primeiro lugar, iniciamos todas as distâncias como infinitas(int.MaxValue), visited como false e parent como -1
        for (int i = 0; i < qtdVertices; i++)
        {
            distances[i] = int.MaxValue;
            visited[i] = false;
            parent[i] = -1;
        }

        //Em segundo lugar, definimos a distância do vértice de origem a partir de si mesmo como 0
        distances[origem] = 0;

        for (int count = 0; count < qtdVertices - 1; count++) //Então, encontramos o caminho mais curto para todos os vértices
        {
            //Para isso, devemos selecionar o vértice com distância mínima a partir do conjunto de vértices que ainda não foram processados.
            int u = DistanciaMinima(distances, visited, qtdVertices); 
            visited[u] = true; // Devemos marcar o vértice selecionado como visited para que não façamos o cálculo duas vezes.
     

            for (int v = 0; v < qtdVertices; v++)
            {
                //Caso o caminho mais curto seja encontrado, definimos o novo valor o caminho mais curto
                if (!visited[v] && grafo[u, v] != 0 && distances[u] != int.MaxValue && distances[u] + grafo[u, v] < distances[v])
                {
                    distances[v] = distances[u] + grafo[u, v]; // Definindo o valor como o caminho mais curto
                    parent[v] = u; //Reconstrução do caminho, vértice 'u' precede o vértice 'v'
                }
            }
        }

        //Depois de todos os vértices forem processados, devolvemos o resultado contendo o valor do caminho mais curto a partir de vértice de origem até o vértice de destino.

        MostrarCaminho(distances, parent, origem, destino, vertices);
    }

}
