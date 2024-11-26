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
            distancias[i] = int.MaxValue;
            pai[i] = -1;
        }

        distancias[origem] = 0;

        for (int i = 0; i < numVertices - 1; i++)
        {
            for (int u = 0; u < numVertices; u++)
            {
                for (int v = 0; v < numVertices; v++)
                {
                    if (grafo.Matriz[u, v] != 0)
                    {
                        int peso = grafo.Matriz[u, v];

                        if (distancias[u] != int.MaxValue && distancias[u] + peso < distancias[v])
                        {
                            distancias[v] = distancias[u] + peso;
                            pai[v] = u;
                        }
                    }
                }
            }
        }

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
