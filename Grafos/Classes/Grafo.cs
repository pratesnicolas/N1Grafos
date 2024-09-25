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
        if(vertice is null)
        {
            Console.WriteLine($"Vértice {nomeVertice} não encontrado.");
            return;
        }
        Vertices.Remove(vertice);
        var arestasParaRemover = Arestas.Where(x => x.Origem.Nome == vertice.Nome || x.Destino.Nome == vertice.Nome).ToList();
        foreach (var aresta in arestasParaRemover)
        {
            Arestas.Remove(aresta);
        }

        AtualizarMatriz();
    }

    public void AtualizarVertice(Vertice vertice, string novoNomeVertice)
    {
        var oldNome = vertice.Nome;

        vertice.Nome = novoNomeVertice;
        Console.WriteLine($"Vértice {oldNome} atualizado para {novoNomeVertice} com sucesso!");
        Console.WriteLine(ExibirMatriz());
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

    public void ConsultarVertice(string nomeVertice)
    {
        var vertice = Vertices.FirstOrDefault(x => x.Nome.Equals(nomeVertice.Trim(), StringComparison.CurrentCultureIgnoreCase));

        var existeCiclo = Arestas.Any(x => x.PossuiLaco());

        var grauEntradas = Arestas.Count(x => x.Destino.Nome == nomeVertice.Trim() && x.Origem.Nome != nomeVertice.Trim());
        var grauSaidas = Arestas.Count(x => x.Origem.Nome == nomeVertice.Trim() && x.Destino.Nome != nomeVertice.Trim());

        if (existeCiclo)
        {
            grauEntradas++;
            grauSaidas++;
        }
        
        if (vertice != null)
        {
            Console.WriteLine($"Vertice consultado: {vertice.Nome}:");
            Console.WriteLine($"Graus de Entrada e Saída: G(E) = {grauEntradas} | G(S) = {grauSaidas}");
        }
        else
        {
            Console.WriteLine("Vertice não encontrado.");
        }
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
    public void RemoverAresta(Vertice origem,
                              Vertice destino)
    {

        if (origem is null || destino is null)
        {
            throw new Exception("O vertice de origem ou de destino não existem.");
        }

        Matriz[Vertices.IndexOf(origem), Vertices.IndexOf(destino)] = 0;
        Console.WriteLine($"Aresta entre {origem.Nome} e {destino.Nome} removida com sucesso!");
        Console.WriteLine(ExibirMatriz());
    }

    public void AtualizarAresta(string origem,
                                string destino,
                                int peso)
    {
        Vertice verticeOrigem = Vertices.Find(x => x.Nome.Equals(origem, StringComparison.CurrentCultureIgnoreCase));
        Vertice verticeDestino = Vertices.Find(x => x.Nome.Equals(destino, StringComparison.CurrentCultureIgnoreCase));

        if (verticeOrigem is null || verticeDestino is null)
        {
            throw new Exception("O vertice de origem ou de destino não existem.");
        }

        var aresta = Arestas.FirstOrDefault(x => x.Origem == verticeOrigem && x.Destino == verticeDestino) ?? throw new Exception("A aresta não existe.");
        var oldPeso = aresta.Peso;

        aresta.Peso = peso;
        Matriz[Vertices.IndexOf(verticeOrigem), Vertices.IndexOf(verticeDestino)] = peso;
        Console.WriteLine($"Peso alterado {verticeOrigem.Nome}-{verticeDestino.Nome}: {oldPeso} => {peso}.");
        Console.WriteLine($"Aresta entre {verticeOrigem.Nome} e {verticeDestino.Nome} atualizada com sucesso!");
        Console.WriteLine(ExibirMatriz());
    }

    public void ConsultarAresta(Vertice origem, Vertice destino)
    {

        if (origem is null || destino is null)
        {
            throw new Exception("O vertice de origem ou de destino não existem.");
        }

        var aresta = Arestas.FirstOrDefault(x => x.Origem == origem && x.Destino == destino);
        if (aresta is null)
        {
            Console.WriteLine("A aresta não existe.");
        }
        else
        {
            Console.WriteLine($"Aresta encontrada: Origem: {aresta.Origem.Nome}, Destino: {aresta.Destino.Nome}, Peso: {aresta.Peso}");
        }
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

    public string ExibirMatrizPesos()
    {
        //Esta matriz exibe os pesos das arestas ao invés de informar se a aresta existe ou não, pesos 0 são considerados.

        var qtdVertices = Vertices.Count;
        var resultado = new StringBuilder();
        var maiorValor = Arestas.Max(a => a.Peso);

        resultado.Append("    ");
        for (int i = 0; i < qtdVertices; i++)
        {
            if (maiorValor.ToString().Length > Vertices[i].Nome.Length)
            {
                resultado.Append(Vertices[i].Nome.PadLeft(maiorValor.ToString().Length) + "   ");
            }
            else
            {
                resultado.Append(Vertices[i].Nome.PadLeft(Vertices[i].Nome.Length) + "   ");
            }
        }
        resultado.AppendLine("\n");
        for (int i = 0; i < qtdVertices; i++)
        {
            resultado.Append(Vertices[i].Nome.PadRight(maiorValor.ToString().Length) + "   ");
            for (int j = 0; j < qtdVertices; j++)
            {
                resultado.Append(Matriz[i, j].ToString().PadRight(maiorValor.ToString().Length) + "   ");
            }
            resultado.AppendLine();
        }
        return resultado.ToString();
    }

    public string ExibirMatriz()
    {
        //Esta matriz mudou para exibir 1 se a aresta existe e 0 se não existe
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
                // Verifica se existe uma aresta entre os vértices i e j
                int valor = Arestas.Any(a => a.Origem == Vertices[i] && a.Destino == Vertices[j]) ? 1 : 0;
                resultado.Append(valor + "   ");
            }
            resultado.AppendLine();
        }
        return resultado.ToString();
    }

    public void VerificarDependencias(int idxMaquina)
    {
        var maquina = Vertices[idxMaquina];
        var visitados = new HashSet<Vertice>();
        VerificarDependenciasRecursivo(maquina, visitados, 0);
    }

    public int VerificarMaiorPeso()
    {
        var maiorPeso = Arestas.Max().Peso;
        return maiorPeso;
    }

    private void VerificarDependenciasRecursivo(Vertice maquina, HashSet<Vertice> visitados, int nivel)
    {
        if (visitados.Contains(maquina))
        {
            return;
        }

        visitados.Add(maquina);

        // Imprime a dependência com indentação baseada no nível
        Console.WriteLine(new string(' ', nivel * 2) + $"{nivel} - " + $"Máquina {maquina.Nome}");

        var arestasSelecionadas = Arestas.Where(a => a.Origem == maquina);
        foreach (var aresta in arestasSelecionadas)
        {
            VerificarDependenciasRecursivo(aresta.Destino, new HashSet<Vertice>(visitados), nivel + 1);
        }
    }

    public static Grafo NovoGrafo(int vertices)
    => new(vertices);
}
