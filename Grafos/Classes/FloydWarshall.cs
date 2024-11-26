namespace Grafos.Classes;

public class FloydWarshall
{
    public void AlgoritmoFloydWarshall(int[,] graph, int qtdVertices)
    {
        int[,] distancia = new int[qtdVertices, qtdVertices];

        for (int i = 0; i < qtdVertices; ++i)
        {
            for (int j = 0; j < qtdVertices; ++j)
            {
                if (i == j)
                    distancia[i, j] = 0; 
                else if (graph[i, j] == 0)
                    distancia[i, j] = int.MaxValue;
                else
                    distancia[i, j] = graph[i, j]; 
            }
        }

        for (int k = 0; k < qtdVertices; ++k)
        {
            for (int i = 0; i < qtdVertices; ++i)
            {
                for (int j = 0; j < qtdVertices; ++j)
                {
                    if (distancia[i, k] != int.MaxValue && distancia[k, j] != int.MaxValue)
                    {
                        distancia[i, j] = Math.Min(distancia[i, j], distancia[i, k] + distancia[k, j]);
                    }
                }
            }
        }


        MostrarCaminho(distancia, qtdVertices);
    }

    private void MostrarCaminho(int[,] distancia, int qtdVertices)
    {
        Console.WriteLine("Caminho mínimo entre todos os pares de vértices:");

        for (int i = 0; i < qtdVertices; ++i)
        {
            for (int j = 0; j < qtdVertices; ++j)
            {
                if (distancia[i, j] == int.MaxValue)
                    Console.Write("INF".PadLeft(7));
                else
                    Console.Write(distancia[i, j].ToString().PadLeft(7));
            }

            Console.WriteLine();
        }
    }
}
