using System.Text;

namespace N1Grafos.Entities;

public class Grafo(int vertices)
{
    public int[,] Matriz { get; private set; } = new int[vertices, vertices];
    public int Vertices => Matriz.GetLength(0);

    public void AdicionarAresta(int posicaoInicial, int posicaoFinal)
    {
        Matriz[posicaoInicial - 1, posicaoFinal - 1] = 1;
    }

    public string ExibirMatriz()
    {
        StringBuilder sb = new();
        var contador = 0;

        sb.Append("Matriz Adjacente: ");
        sb.AppendLine();

        foreach (var vertice in Matriz)
        {
            sb.Append(vertice);
            sb.Append(' ');
            contador++;
            if (contador % Vertices == 0)
                sb.AppendLine();
        }

        return sb.ToString();
    }

    public static Grafo NovoGrafo(int vertices)
    => new(vertices);
}
