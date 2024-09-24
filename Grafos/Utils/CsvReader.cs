using Grafos.Classes;

namespace Grafos.Utils
{
    public static class CsvReader
    {
        public static Grafo ProcessarCsv(string caminho)
        {
            var grafo = Grafo.NovoGrafo(0); 
            var verticesEncontrados = new HashSet<string>();

            if (string.IsNullOrEmpty(caminho))
            {
                Console.WriteLine("\nNenhum CSV informado, portanto, começando com o Grafo vazio.\n\n");
                return grafo;
            } 
               

            using (var reader = new StreamReader(caminho))
            {
                var primeiraLinha = reader.ReadLine();

                while (!reader.EndOfStream)
                {
                    var linha = reader.ReadLine();
                    var valores = linha.Split(',');

                    if (valores.Length == 3)
                    {
                        var origem = valores[0];
                        var destino = valores[1];
                        var peso = int.Parse(valores[2]);

                        if (!verticesEncontrados.Contains(origem))
                        {
                            grafo.AdicionarVertice(origem);
                            verticesEncontrados.Add(origem);
                        }

                        if (!verticesEncontrados.Contains(destino))
                        {
                            grafo.AdicionarVertice(destino);
                            verticesEncontrados.Add(destino);
                        }

                        grafo.AdicionarAresta(origem, destino, peso);
                    }
                    else
                    {
                        throw new Exception("Formato inválido no arquivo CSV.");
                    }
                }
            }

            Console.WriteLine("\nGrafo carregado com sucesso!\n\n");
            return grafo;

        }
    }

}

