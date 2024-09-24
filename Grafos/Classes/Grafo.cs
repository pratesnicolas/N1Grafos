using System.Text;

namespace Grafos.Classes;

public class Grafo(int vertices)
{
    public int[,] Matriz { get; private set; } = new int[vertices, vertices];
    public List<Vertice> Vertices { get; } = [];
    public List<Aresta> Arestas { get; } = [];

    public void AdicionarVertice(string novoVertice)
    {
        var vertice = new Vertice(novoVertice); 
        Vertices.Add(vertice);
        var qtdVertices = Vertices.Count;
        var novaMatriz = new int[qtdVertices, qtdVertices];
        for (int i = 0; i < qtdVertices - 1; i++)
        {
            for (int j = 0; j < qtdVertices - 1; j++)
            {
                novaMatriz[i, j] = Matriz[i, j];
            }
        }
        Matriz = novaMatriz;
    }

    public void RemoverVertice(string nomeVertice)
    {
        var vertice = Vertices.FirstOrDefault(x => x.Nome == nomeVertice.Trim());
        Vertices.Remove(vertice);
        var arestasParaRemover = Arestas.Where(x => x.Origem.Nome == vertice.Nome || x.Destino.Nome == vertice.Nome).ToList();
        foreach (var aresta in arestasParaRemover)
        {
            Arestas.Remove(aresta);
        }

        AtualizarMatriz();
    }

   

    public string MostrarVertices()
    {
        string vertices = string.Empty;
        foreach (var vertice in Vertices)
        {
            vertices += (" " + vertice.Nome);
        }

        return vertices;
    }

    public void AdicionarAresta(string origem,
                                string destino,
                                int peso)
    {
        Vertice verticeOrigem = Vertices.Find(x => x.Nome.Equals(origem, StringComparison.CurrentCultureIgnoreCase));
        Vertice verticeDestino = Vertices.Find(x => x.Nome.Equals(destino, StringComparison.CurrentCultureIgnoreCase));

        if (verticeOrigem is null || verticeDestino is null)
        {
            throw new Exception("O vertice de origem ou de destino não existem.");
        }

        var aresta = new Aresta(verticeOrigem,
                                verticeDestino,
                                peso);
        Arestas.Add(aresta);

        Matriz[Vertices.IndexOf(verticeOrigem), Vertices.IndexOf(verticeDestino)] = aresta.Peso;
    }
    public void RemoverAresta(string origem,
                              string destino)
    {
        Vertice verticeOrigem = Vertices.Find(x => x.Nome.Equals(origem, StringComparison.CurrentCultureIgnoreCase));
        Vertice verticeDestino = Vertices.Find(x => x.Nome.Equals(destino, StringComparison.CurrentCultureIgnoreCase));

        if (verticeOrigem is null || verticeDestino is null)
        {
            throw new Exception("O vertice de origem ou de destino não existem.");
        }

        Matriz[Vertices.IndexOf(verticeOrigem), Vertices.IndexOf(verticeDestino)] = 0;
    }

    public void AtualizarMatriz()
    {
        int qtdVertices = Vertices.Count;
        var novaMatriz = new int[qtdVertices, qtdVertices];

        for (int i = 0; i < qtdVertices; i++)
        {
            for (int j = 0; j < qtdVertices; j++)
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

        resultado.Append("    ");
        for (int i = 0; i < qtdVertices; i++)
        {
            resultado.Append(Vertices[i].Nome + "   ");
        }
        resultado.AppendLine();
        for (int i = 0; i < qtdVertices; i++)
        {
            resultado.Append(Vertices[i].Nome + "   ");
            for (int j = 0; j < qtdVertices; j++)
            {
                resultado.Append(Matriz[i, j] + "   ");
            }
            resultado.AppendLine();
        }
        return resultado.ToString();
    }
    public static Grafo NovoGrafo(int vertices)
    => new(vertices);
}
