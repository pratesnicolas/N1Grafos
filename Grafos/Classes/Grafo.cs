using System.Text;

namespace Grafos.Classes;

public class Grafo(int vertices)
{
    public int[,] Matriz { get; private set; } = new int[vertices, vertices];
    public List<Vertice> Vertices { get; } = [];
    public List<Aresta> Arestas { get; } = [];

    public void AdicionarVertice(Vertice vertice)
    {
        Vertices.Add(vertice);
        var qtdVertices = Vertices.Count;
        var novaMatriz = new int[qtdVertices, qtdVertices];
        for(int i = 0; i < qtdVertices - 1;  i++)
        {
            for(int j = 0; j < qtdVertices - 1; j++)
            {
                novaMatriz[i, j] = Matriz[i, j];
            }
        }
        Matriz = novaMatriz;
    }

    public string ExibirMatriz()
    {
        var qtdVertices = Vertices.Count;
        var resultado = new StringBuilder();

        // Adiciona os nomes dos vértices na primeira linha
        resultado.Append("    "); // Espaço para alinhar a primeira coluna
        for (int i = 0; i < qtdVertices; i++)
        {
            resultado.Append(Vertices[i].Nome + " ");
        }
        resultado.AppendLine(); // Quebra de linha após a primeira linha

        // Adiciona a matriz com os nomes dos vértices nas linhas e colunas
        for (int i = 0; i < qtdVertices; i++)
        {
            resultado.Append(Vertices[i].Nome + " "); // Adiciona o nome do vértice na linha
            for (int j = 0; j < qtdVertices; j++)
            {
                resultado.Append(Matriz[i, j] + "   "); // Adiciona valor da aresta ou 0
            }
            resultado.AppendLine(); // Quebra de linha após cada linha da matriz
        }

        return resultado.ToString(); // Retorna a string final
    }

    public void AdicionarAresta(string origem,
                                string destino,
                                int peso)
    {
        Vertice verticeOrigem = Vertices.Find(x => x.Nome == origem);
        Vertice verticeDestino = Vertices.Find(x => x.Nome == destino);

        if(verticeOrigem is null || verticeDestino is null) 
        {
            throw new Exception("O vertice de origem ou de destino não existem.");
        }

        var aresta = new Aresta(verticeOrigem,
                                verticeDestino,
                                peso);
        Arestas.Add(aresta);

        Matriz[Vertices.IndexOf(verticeOrigem), Vertices.IndexOf(verticeDestino)] = aresta.Peso;
    }

    public static Grafo NovoGrafo(int vertices)
    => new(vertices);
}
