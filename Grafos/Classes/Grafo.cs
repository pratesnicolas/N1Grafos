using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Text;

namespace Grafos.Classes;

public class Grafo(int vertices)
{
    public int[,] Matriz { get; private set; } = new int[vertices, vertices];
    public List<Vertice> Vertices { get; } = [];
    public List<Aresta> Arestas { get; } = [];

    public void AdicionarVertice(string novoVertice)
    {
        var idx = Vertices.Count;
        var vertice = new Vertice(novoVertice, IndiceParaLetra(idx));
        if (Vertices.Any(x => x.Nome == novoVertice.Trim()))
        {
            Console.WriteLine($"O vértice {novoVertice} já existe na matriz.");
            
            return;

        }

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

        Console.WriteLine($"\nVertice {vertice.Nome} adicionado com sucesso!");
    }

    public void RemoverVertice(string nomeVertice)
    {
        var vertice = Vertices.FirstOrDefault(x => x.Nome == nomeVertice.Trim());
        if (vertice is null)
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

    public void ConsultarVertice(Vertice vertice)
    {

        var existeCiclo = Arestas.Any(x => x.PossuiLaco());

        var grauEntradas = Arestas.Count(x => x.Destino.Nome == vertice.Nome.Trim() && x.Origem.Nome != vertice.Nome.Trim());
        var grauSaidas = Arestas.Count(x => x.Origem.Nome == vertice.Nome.Trim() && x.Destino.Nome != vertice.Nome.Trim());

        if (existeCiclo)
        {
            grauEntradas++;
            grauSaidas++;
        }

        if (vertice is not null)
        {
            Console.WriteLine($"\nVertice: {vertice.Nome}");
            Console.WriteLine($"Apelido: \"{vertice.Apelido}\"");
            Console.WriteLine($"\nGraus de Entrada e Saída: \n G(E) = {grauEntradas} | G(S) = {grauSaidas}");
            Console.WriteLine($"\nPossui laço: {(Arestas.Any(x => x.Origem.Nome == vertice.Nome && x.Destino.Nome == vertice.Nome) ? "Sim" : "Não")}");

        }
        else
        {
            Console.WriteLine("\nVertice não encontrado.");
        }
    }

    public void MostrarTempoProducaoMaquinas(Vertice maquinaInicial)
    {
        var tempoTotal = CalcularTempoProducao(maquinaInicial);
        Console.WriteLine($"Tempo total de produção: {tempoTotal} horas");

        Console.WriteLine("Gráfico de Tempo de Produção:");
        Console.WriteLine("-------------------------------");

        MostrarGraficoTempoProducao(maquinaInicial, 0);
    }

    private void MostrarGraficoTempoProducao(Vertice maquina, int nivel)
    {
        var horasTotais = CalcularTempoProducao(maquina);

        if (horasTotais != 0)
        {
            Console.WriteLine($"{new string(' ', nivel * 2)}{maquina.Nome}: Total de {horasTotais} horas");
        }

        var arestasSelecionadas = Arestas.Where(a => a.Origem == maquina);
        foreach (var aresta in arestasSelecionadas)
        {
            Console.WriteLine($"{new string(' ', nivel * 2 + 2)}{maquina.Nome} --({aresta.Peso}h)--> {aresta.Destino.Nome}");
            MostrarGraficoTempoProducao(aresta.Destino, nivel + 1);
        }
    }



    private int CalcularTempoProducao(Vertice maquina)
    {
        var tempoMaquina = 0;
        var arestasSelecionadas = Arestas.Where(a => a.Origem == maquina);

        foreach (var aresta in arestasSelecionadas)
        {
            var tempoAresta = aresta.Peso;
            var tempoDependencia = CalcularTempoProducao(aresta.Destino);
            var tempoTotal = tempoAresta + tempoDependencia;

            if (tempoTotal > tempoMaquina)
            {
                tempoMaquina = tempoTotal;
            }
        }

        return tempoMaquina;
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

        if (Matriz[Vertices.IndexOf(verticeOrigem), Vertices.IndexOf(verticeDestino)] > 0)
        {
            Console.WriteLine($"Essa aresta já existe.");
            return;
        }

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
            Console.WriteLine("O vértice de origem ou destino não existe.");
            return;
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

    public void ExibirMatrizPesos()
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
        Console.WriteLine(resultado.ToString());
    }

    public string ExibirMatriz()
    {
        //Esta matriz mudou para exibir 1 se a aresta existe e 0 se não existe
        var qtdVertices = Vertices.Count;
        var resultado = new StringBuilder();

        resultado.Append("    ");
        for (int i = 0; i < qtdVertices; i++)
        {
            resultado.Append(Vertices[i].Apelido + "   ");
        }
        resultado.AppendLine();
        for (int i = 0; i < qtdVertices; i++)
        {
            resultado.Append(Vertices[i].Apelido + "   ");
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

    public void VerificarMaiorPeso()
    {
        var arestaComMaiorPeso = Arestas.MaxBy(x => x.Peso);
        Console.WriteLine($"O maior gargalo é entre {arestaComMaiorPeso.Origem.Nome} e {arestaComMaiorPeso.Destino.Nome} e o tempo é de {arestaComMaiorPeso.Peso}");
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
    public void ExibirCaminhoCritico()
    {
        var caminhoCritico = ObterCaminhoCritico();
        if (caminhoCritico.Count == 0)
        {
            Console.WriteLine("\nNão há caminho crítico.");
            return;
        }

        ExibirTodosOsCaminhos();
        Console.WriteLine("\n\nCaminho Crítico: ");
        for (int i = 0; i < caminhoCritico.Count; i++)
        {
            var vertice = caminhoCritico[i];
            Console.Write(vertice.Nome);

            if (i < caminhoCritico.Count - 1)
            {
                var aresta = Arestas.First(a => a.Origem == vertice && a.Destino == caminhoCritico[i + 1]);
                Console.Write($" --({aresta.Peso}h)--> ");
            }
        }

        var tempoTotal = caminhoCritico.Zip(caminhoCritico.Skip(1), (origem, destino) =>
            Arestas.First(a => a.Origem == origem && a.Destino == destino).Peso).Sum();

        Console.WriteLine($"\nTempo total do caminho crítico: {tempoTotal} horas");


    }

    public void RealizarBuscaProfundidade(int idxVerticeOrigem, int idxVerticeDestino)
    {
        var verticeOrigem = Vertices[idxVerticeOrigem];
        var verticeDestino = Vertices[idxVerticeDestino];
        
        BuscaEmProfundidade(verticeOrigem, verticeDestino);
    }

    public void RealizarBuscaLargura(int idxVerticeOrigem, int idxVerticeDestino)
    {
        var verticeOrigem = Vertices[idxVerticeOrigem];
        var verticeDestino = Vertices[idxVerticeDestino];

        BuscaEmLargura(verticeOrigem, verticeDestino);
    }

    public List<Vertice> ObterCaminhoCritico()
    {
        // Ordenar topologicamente os vértices
        List<Vertice> ordemTopologica = OrdenarTopologicamente(Arestas, Vertices);
        Dictionary<Vertice, int> distancia = new Dictionary<Vertice, int>();
        Dictionary<Vertice, Vertice> predecessores = new Dictionary<Vertice, Vertice>();

        // Inicializa todas as distâncias com o valor mínimo e predecessores como nulos
        foreach (var vertice in ordemTopologica)
        {
            distancia[vertice] = int.MinValue;
            predecessores[vertice] = null;
        }

        // Define a distância do primeiro vértice como 0
        distancia[ordemTopologica[0]] = 0;

        // Calcula as distâncias máximas na ordem topológica
        foreach (var vertice in ordemTopologica)
        {
            foreach (var aresta in Arestas.Where(a => a.Origem == vertice))
            {
                int novaDistancia = distancia[vertice] + aresta.Peso;
                if (novaDistancia > distancia[aresta.Destino])
                {
                    distancia[aresta.Destino] = novaDistancia;
                    predecessores[aresta.Destino] = vertice;
                }
            }
        }

        // Encontra o vértice com a maior distância final (fim do caminho crítico)
        Vertice verticeFinal = distancia.OrderByDescending(d => d.Value).First().Key;

        // Constrói o caminho crítico partindo do vértice final
        List<Vertice> caminhoCritico = new List<Vertice>();
        Vertice verticeAtual = verticeFinal;

        while (verticeAtual != null)
        {
            caminhoCritico.Add(verticeAtual);
            verticeAtual = predecessores[verticeAtual];
        }

        // Reverte a lista para ter o caminho na ordem correta (do início ao fim)
        caminhoCritico.Reverse();
        return caminhoCritico;
    }

    public void ExibirTodosOsCaminhos()
    {
        // Ordenar topologicamente os vértices
        List<Vertice> ordemTopologica = OrdenarTopologicamente(Arestas, Vertices);
        Dictionary<Vertice, int> distancia = new Dictionary<Vertice, int>();
        Dictionary<Vertice, Vertice> predecessores = new Dictionary<Vertice, Vertice>();

        // Inicializa todas as distâncias com o valor mínimo e predecessores como nulos
        foreach (var vertice in ordemTopologica)
        {
            distancia[vertice] = int.MinValue;
            predecessores[vertice] = null;
        }

        // Define a distância do primeiro vértice como 0
        distancia[ordemTopologica[0]] = 0;

        // Calcula as distâncias máximas na ordem topológica
        foreach (var vertice in ordemTopologica)
        {
            foreach (var aresta in Arestas.Where(a => a.Origem == vertice))
            {
                int novaDistancia = distancia[vertice] + aresta.Peso;
                if (novaDistancia > distancia[aresta.Destino])
                {
                    distancia[aresta.Destino] = novaDistancia;
                    predecessores[aresta.Destino] = vertice;
                }
            }
        }

        // Exibir todos os caminhos a partir do vértice inicial
        foreach (var verticeFinal in Vertices)
        {
            if (distancia[verticeFinal] > int.MinValue)
            {
                // Constrói o caminho partindo do vértice final
                List<Vertice> caminho = new List<Vertice>();
                Vertice verticeAtual = verticeFinal;

                while (verticeAtual != null)
                {
                    caminho.Add(verticeAtual);
                    verticeAtual = predecessores[verticeAtual];
                }

                // Reverte o caminho para mostrar na ordem correta
                caminho.Reverse();

                // Verifica se o caminho tem mais de um vértice
                if (caminho.Count > 1) // Apenas imprime se o caminho contém mais de um vértice
                {
                    // Monta a string do caminho com pesos
                    string caminhoString = "";
                    int totalHoras = 0;

                    for (int i = 0; i < caminho.Count - 1; i++)
                    {
                        var origem = caminho[i];
                        var destino = caminho[i + 1];

                        // Encontra a aresta correspondente para obter o peso
                        var aresta = Arestas.FirstOrDefault(a => a.Origem == origem && a.Destino == destino);
                        if (aresta != null)
                        {
                            caminhoString += $"{origem.Nome} --({aresta.Peso}h)--> ";
                            totalHoras += aresta.Peso;
                        }
                    }
                    // Adiciona o último vértice ao caminho
                    caminhoString += caminho.Last().Nome;

                    // Exibe o caminho e o tempo total
                    Console.WriteLine($"Caminho: {caminhoString}, Tempo total: {totalHoras} horas");
                }
            }
        }
    }

    public List<Vertice> OrdenarTopologicamente(List<Aresta> arestas, List<Vertice> vertices)
    {
        List<Vertice> ordemTopologica = new List<Vertice>();
        HashSet<Vertice> visitados = new HashSet<Vertice>();
        Dictionary<Vertice, int> distancias = vertices.ToDictionary(v => v, v => int.MinValue);

        // Inicializa a distância do primeiro vértice como 0 (supondo que vertices[0] é a origem)
        if (vertices.Count > 0)
        {
            distancias[vertices[0]] = 0;
        }

        // Visitar cada vértice
        foreach (var vertice in vertices)
        {
            if (!visitados.Contains(vertice))
            {
                VisitarVertice(vertice, visitados, ordemTopologica, arestas);
            }
        }

        // Reverte a ordem topológica
        ordemTopologica.Reverse();

        // Atualiza as distâncias com base nas arestas
        foreach (var vertice in ordemTopologica)
        {
            // Busca todas as arestas cujo vértice atual é a origem
            foreach (var aresta in arestas.Where(a => a.Origem == vertice))
            {
                int novaDistancia = distancias[vertice] + aresta.Peso;
                if (novaDistancia > distancias[aresta.Destino])
                {
                    distancias[aresta.Destino] = novaDistancia;
                }
            }
        }

        // Encontrar o maior caminho
        //Vertice destinoFinal = ordemTopologica.OrderByDescending(v => distancias[v]).First();
        //Console.WriteLine($"\nCaminho crítico termina no vértice: {destinoFinal.Nome}, com tempo total de: {distancias[destinoFinal]} horas!\n");

        return ordemTopologica;
    }

    public void EncontrarVerticesIndependentes()
    {
        var verticesIndependentes = string.Empty;

        foreach (var vertice in Vertices)
        {
            if (!Arestas.Any(x => x.Destino.Nome == vertice.Nome.Trim()))
            {
                verticesIndependentes += $" " + vertice.Nome;
            }
        }
        Console.WriteLine($"Vértices independentes: {verticesIndependentes}");
    }

    public void Dijkstra(int origem, int destino)
    {
        var dijkstra = new Dijkstra();
        dijkstra.AlgoritmoDijkstra(Matriz, origem, destino, Vertices.Count);
    }

    public void BellmanFord(int origem, int destino)
    {
        var bellmanFord = new BellmanFord();
        bellmanFord.AlgorimoBellmanFord(this, origem, destino);
    }

    public void FloydWarshall()
    {
        var floydWarshall = new FloydWarshall();
        floydWarshall.AlgoritmoFloydWarshall(Matriz, Vertices.Count);
    }


    public void MostrarCaracteristicas()
    {
        var caracteristicas = string.Empty;
        Console.WriteLine("Tipo de grafo: Digrafo");
        foreach (var vertice in Vertices)
        {
            this.ConsultarVertice(vertice);            
        }
        if(Arestas.Any(x => x.PossuiValor()))
        {
            Console.WriteLine("\nO grafo possui arestas valoradas.");

        }
        else
        {
            Console.WriteLine("\nO grafo não possui arestas valoradas.");

        }

    }

    public void BuscaEmProfundidade(Vertice origem, Vertice destino)
    {
        if (origem == null || destino == null)
        {
            Console.WriteLine("Vértice de origem ou destino inválido.");
            return;
        }

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        var visitados = new HashSet<string>();
        var caminho = new Dictionary<string, string>();
        var custo = new Dictionary<string, int>();
        var todosCaminhos = new List<Caminho>();
        var caminhoAtual = new Caminho([], 0);

        custo[origem.Apelido] = 0;

        DfsTodosCaminhos(origem, destino, visitados, caminho, custo, todosCaminhos, caminhoAtual);

        stopwatch.Stop();

        if (todosCaminhos.Count > 0)
        {
            Console.WriteLine("\nCaminhos encontrados até o destino:");
            foreach (var caminhoEncontrado in todosCaminhos.OrderBy(c => c.CustoTotal))
            {
                Console.WriteLine("\n");
                ExibirCaminho(caminhoEncontrado.Vertices);
                
            }

            var caminhoOtimo = todosCaminhos.OrderBy(c => c.CustoTotal).First();
            Console.WriteLine("\n============= Caminho ótimo =============\n");
            ExibirCaminho(caminhoOtimo.Vertices);
        }
        else
        {
            Console.WriteLine("\nCaminho não encontrado até o destino.");
        }

        long elapsedTicks = stopwatch.ElapsedTicks;
        double elapsedMicroseconds = (elapsedTicks / (double)Stopwatch.Frequency) * 1_000_000;
        double elapsedNanoseconds = (elapsedTicks / (double)Stopwatch.Frequency) * 1_000_000_000;

        Console.WriteLine($"\nTempo de processamento: {stopwatch.ElapsedMilliseconds} ms / {elapsedMicroseconds} µs / {elapsedNanoseconds} ns\n");
    }

    private void DfsTodosCaminhos(Vertice atual, Vertice destino, HashSet<string> visitados, Dictionary<string, string> caminho, Dictionary<string, int> custo, List<Caminho> todosCaminhos, Caminho caminhoAtual)
    {
        visitados.Add(atual.Apelido);
        caminhoAtual.Vertices.Add(atual);

        if (atual.Equals(destino))
        {
            caminhoAtual.CustoTotal = ObterCustoDoCaminho(caminhoAtual.Vertices);
            todosCaminhos.Add(new Caminho([..caminhoAtual.Vertices], caminhoAtual.CustoTotal));
        }
        else
        {
            foreach (var vizinho in ObterVizinhos(atual))
            {
                if (!visitados.Contains(vizinho.Apelido))
                {
                    caminho[vizinho.Apelido] = atual.Apelido;
                    custo[vizinho.Apelido] = custo[atual.Apelido] + ObterPeso(atual, vizinho);
                    DfsTodosCaminhos(vizinho, destino, visitados, caminho, custo, todosCaminhos, caminhoAtual);
                }
            }
        }

        visitados.Remove(atual.Apelido);
        caminhoAtual.Vertices.Remove(atual);
        caminhoAtual.CustoTotal = ObterCustoDoCaminho(caminhoAtual.Vertices);
    }

    public List<Vertice> ObterVizinhos(Vertice atual)
    {
        List<Vertice> vizinhos = new List<Vertice>();
        for (int i = 0; i < Vertices.Count; i++)
        {
            if (Matriz[Vertices.IndexOf(atual), i] != 0)
            {
                vizinhos.Add(Vertices[i]);
            }
        }
        return vizinhos;
    }

    public int ObterPeso(Vertice origem, Vertice destino)
    {
        return Arestas.FirstOrDefault(a => a.Origem == origem && a.Destino == destino)?.Peso ?? 0;
    }

    private void ExibirCaminho(List<Vertice> caminho)
    {
        var custoTotal = 0;
        for (int i = 0; i < caminho.Count - 1; i++)
        {
            var verticeAtual = caminho[i];
            var proximoVertice = caminho[i + 1];
            
            var aresta = Arestas.FirstOrDefault(a => a.Origem == verticeAtual && a.Destino == proximoVertice);

            if (aresta != null)
            {
                Console.Write($"{verticeAtual.Nome} --({aresta.Peso})--> ");
                custoTotal += aresta.Peso;
            }
            else
            {
                Console.Write($"{verticeAtual.Nome} -> ");
            }
        }

        // Exibir o último vértice sem seta
        Console.WriteLine(caminho.Last().Nome);
        Console.WriteLine($"\tCusto total: {custoTotal} hrs");
    }

    private int ObterCustoDoCaminho(List<Vertice> caminho)
    {
        var custoTotal = 0;

        for (int i = 0; i < caminho.Count - 1; i++)
        {
            var verticeAtual = caminho[i];
            var proximoVertice = caminho[i + 1];

            var peso = ObterPeso(verticeAtual, proximoVertice);

            if (peso != 0)
            {
                custoTotal += peso;
            }
        }

        return custoTotal;

    }

    private bool Dfs(Vertice atual, Vertice destino, HashSet<Vertice> visitados, Dictionary<Vertice, Vertice> caminho, Dictionary<Vertice, int> custo, int custoAtual)
    {
        if (atual == destino)
        {
            custo[atual] = custoAtual;
            return true;
        }

        visitados.Add(atual);

        foreach (Aresta aresta in Arestas)
        {
            if (aresta.Origem == atual && !visitados.Contains(aresta.Destino))
            {
                caminho[aresta.Destino] = atual;
                custo[aresta.Destino] = custoAtual + aresta.Peso;

                if (Dfs(aresta.Destino, destino, visitados, caminho, custo, custoAtual + aresta.Peso))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public void BuscaEmLargura(Vertice origem, Vertice destino)
    {
        if (origem == null || destino == null)
        {
            Console.WriteLine("Vértice de origem ou destino inválido.");
            return;
        }

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        var visitados = new HashSet<Vertice>();
        var fila = new Queue<Vertice>();
        var caminho = new Dictionary<Vertice, Vertice>();
        var custo = new Dictionary<Vertice, int>();
        var todosCaminhos = new List<Caminho>();

        fila.Enqueue(origem);
        custo[origem] = 0;
        while (fila.Count > 0)
        {
            Vertice verticeAtual = fila.Dequeue();

            if (verticeAtual == destino)
            {
                var caminhoAtual = new Caminho(new List<Vertice>(), 0);
                Vertice v = destino;
                while (v != null)
                {
                    caminhoAtual.Vertices.Insert(0, v);
                    caminho.TryGetValue(v, out v);
                }

                caminhoAtual.CustoTotal = ObterCustoDoCaminho(caminhoAtual.Vertices);

                if (!todosCaminhos.Any(c => c.Vertices.SequenceEqual(caminhoAtual.Vertices)))
                {
                    todosCaminhos.Add(new Caminho(new List<Vertice>(caminhoAtual.Vertices), caminhoAtual.CustoTotal));
                }
            }

            if (!visitados.Contains(verticeAtual))
            {
                visitados.Add(verticeAtual);

                foreach (Aresta aresta in Arestas)
                {
                    if (aresta.Origem == verticeAtual && !visitados.Contains(aresta.Destino))
                    {
                        fila.Enqueue(aresta.Destino);
                        caminho[aresta.Destino] = verticeAtual;
                        custo[aresta.Destino] = custo[verticeAtual] + aresta.Peso;
                    }
                }
            }
        }

        stopwatch.Stop();

        if (todosCaminhos.Count > 0)
        {
            Console.WriteLine("\nCaminhos encontrados até o destino:\n");
            foreach (var caminhoEncontrado in todosCaminhos)
            {
                Console.WriteLine(string.Join(" -> ", caminhoEncontrado.Vertices.Select(v => v.Nome)));
                Console.WriteLine($"\tCusto total: {caminhoEncontrado.CustoTotal} hrs");
            }

            var caminhoOtimo = todosCaminhos.OrderBy(c => c.CustoTotal).First();
            Console.WriteLine("\n============= Caminho ótimo =============\n");
            Console.WriteLine(string.Join(" -> ", caminhoOtimo.Vertices.Select(v => v.Nome)));
            Console.WriteLine($"\tCusto total: {caminhoOtimo.CustoTotal} hrs");
        }
        else
        {
            Console.WriteLine("\nCaminho não encontrado até o destino.\n");
        }

        var elapsedTicks = stopwatch.ElapsedTicks;
        var elapsedMicroseconds = (elapsedTicks / (double)Stopwatch.Frequency) * 1_000_000;
        var elapsedNanoseconds = (elapsedTicks / (double)Stopwatch.Frequency) * 1_000_000_000;

        Console.WriteLine($"\nTempo de processamento: {stopwatch.ElapsedMilliseconds} ms / {elapsedMicroseconds} µs / {elapsedNanoseconds} ns\n");
    }

    // Função auxiliar para visitar os vértices na ordem topológica
    private void VisitarVertice(Vertice vertice,
                                HashSet<Vertice> visitados,
                                List<Vertice> ordemTopologica,
                                List<Aresta> arestas)
    {
        visitados.Add(vertice);

        // Visita todos os destinos a partir das arestas que saem do vértice
        foreach (var aresta in arestas.Where(a => a.Origem == vertice))
        {
            if (!visitados.Contains(aresta.Destino))
            {
                VisitarVertice(aresta.Destino, visitados, ordemTopologica, arestas);
            }
        }

        // Adiciona o vértice à lista da ordem topológica
        ordemTopologica.Add(vertice);
    }

    // Converte índice em letra (0 = A, 1 = B, etc.)
    private string IndiceParaLetra(int indice)
    {
        return ((char)('A' + indice)).ToString();
    }


    public static Grafo NovoGrafo(int vertices)
        => new(vertices);
}
