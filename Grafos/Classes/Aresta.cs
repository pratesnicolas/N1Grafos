namespace Grafos.Classes;
public class Aresta(Vertice origem,
                    Vertice destino,
                    int peso)
{
    public Vertice Origem { get; set; } = origem;
    public Vertice Destino { get; set; } = destino;
    public int Peso { get; set; } = peso;

    public bool PossuiLaco()
    {
        //Verificar a lógica
        return Origem.Nome == Destino.Nome;
    }
    public bool PossuiValor()
    => Peso > 0;
}
