namespace Grafos.Classes;

public class Caminho(List<Vertice> vertices, int custoTotal)
{
    public List<Vertice> Vertices { get; set; } = vertices;
    public int CustoTotal { get; set; } = custoTotal;
}