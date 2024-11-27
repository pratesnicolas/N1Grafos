namespace Grafos.Classes;

public class BellmanFord
{
    public void AlgorimoBellmanFord(Grafo grafo, int origem, int destino)
    {
        int numVertices = grafo.Vertices.Count;

        int[] distancias = new int[numVertices]; 
        int[] pai = new int[numVertices];

        for (int i = 0; i < numVertices; i++)
        {
            distancias[i] = int.MaxValue; // Define uma matriz de distâncias  onde o valor inicial para todos os vértices é infinito(int.MaxValue),
                                          // exceto para o vértice de origem, que é inicializado como 0.

            pai[i] = -1; //Cria uma matriz de predecessores pai para rastrear os caminhos.
        }

        distancias[origem] = 0;

        //Repete o processo de relaxamento para todas as arestas do grafo V-1 vezes,
        //onde V é o número de vértices.Isso garante que o caminho mais curto(que contém no máximo V - 1 arestas) seja encontrado.

        for (int i = 0; i < numVertices - 1; i++)
        {
            for (int u = 0; u < numVertices; u++)
            {
                for (int v = 0; v < numVertices; v++)
                {
                    if (grafo.Matriz[u, v] != 0)
                    {
                        int peso = grafo.Matriz[u, v];

                        //Se a distância acumulada no vértice u somada ao peso da aresta (u, v)
                        //for menor que a distância atual do vértice v, atualiza a distância e define u como o predecessor de v.

                        if (distancias[u] != int.MaxValue && distancias[u] + peso < distancias[v])
                        {
                            distancias[v] = distancias[u] + peso;
                            pai[v] = u;
                        }
                    }
                }
            }
        }

        //Detecção de ciclos negativos
        bool cicloNegativo = false;
        for (int u = 0; u < numVertices; u++)
        {
            for (int v = 0; v < numVertices; v++)
            {
                if (grafo.Matriz[u, v] != 0)
                {
                    int peso = grafo.Matriz[u, v];

                    // Se uma distância ainda puder ser relaxada, há um ciclo negativo
                    if (distancias[u] != int.MaxValue && distancias[u] + peso < distancias[v])
                    {
                        cicloNegativo = true;
                        Console.WriteLine("Ciclo negativo detectado!");
                        break;
                    }
                }
            }

            if (cicloNegativo) break;
        }

        if (!cicloNegativo)
            Console.WriteLine("Não há ciclos negativos.");
        
        
        MostrarCaminho(distancias, pai, origem, destino, grafo.Vertices);
    }

    private void MostrarCaminho(int[] distancias, int[] pai, int origem, int destino, List<Vertice> vertices)
    {
        Console.Write($"Caminho mínimo entre {vertices[origem].Nome} e {vertices[destino].Nome}: ");

        if (distancias[destino] == int.MaxValue)
        {
            Console.WriteLine("Nenhum caminho encontrado.");
            return;
        }

        int atual = destino;
        Stack<int> caminho = new Stack<int>();
        caminho.Push(atual);

        while (pai[atual] != -1)
        {
            atual = pai[atual];
            caminho.Push(atual);
        }

        Console.Write(string.Join(" -> ", caminho.Select(v => vertices[v].Nome)));
        Console.WriteLine($"\nCusto total: {distancias[destino]}");
    }
}
