using System;
using System.IO;

class NoAVL
{
    public int Valor;
    public NoAVL Esquerda;
    public NoAVL Direita;
    public int Altura;

    public NoAVL(int valor)
    {
        Valor = valor;
        Altura = 1;
    }
}

class ArvoreAVL
{
    private NoAVL raiz;

    public void Inserir(int valor)
    {
        raiz = Inserir(raiz, valor);
    }

    private NoAVL Inserir(NoAVL no, int valor)
    {
        if (no == null) return new NoAVL(valor);
        if (valor < no.Valor) no.Esquerda = Inserir(no.Esquerda, valor);
        else if (valor > no.Valor) no.Direita = Inserir(no.Direita, valor);
        else return no;
        AtualizarAltura(no);
        return Balancear(no);
    }

    public void Remover(int valor)
    {
        raiz = Remover(raiz, valor);
    }

    private NoAVL Remover(NoAVL no, int valor)
    {
        if (no == null) return null;
        if (valor < no.Valor) no.Esquerda = Remover(no.Esquerda, valor);
        else if (valor > no.Valor) no.Direita = Remover(no.Direita, valor);
        else
        {
            if (no.Esquerda == null) return no.Direita;
            if (no.Direita == null) return no.Esquerda;
            NoAVL sucessor = MenorValor(no.Direita);
            no.Valor = sucessor.Valor;
            no.Direita = Remover(no.Direita, sucessor.Valor);
        }
        AtualizarAltura(no);
        return Balancear(no);
    }

    private NoAVL MenorValor(NoAVL no)
    {
        while (no.Esquerda != null) no = no.Esquerda;
        return no;
    }

    public void Buscar(int valor)
    {
        Console.WriteLine(Buscar(raiz, valor) ? "Valor encontrado" : "Valor nao encontrado");
    }

    private bool Buscar(NoAVL no, int valor)
    {
        if (no == null) return false;
        if (valor == no.Valor) return true;
        if (valor < no.Valor) return Buscar(no.Esquerda, valor);
        return Buscar(no.Direita, valor);
    }

    public void ImprimirPreOrdem()
    {
        Console.Write("Arvore em pre-ordem: ");
        PreOrdem(raiz);
        Console.WriteLine();
    }

    private void PreOrdem(NoAVL no)
    {
        if (no == null) return;
        Console.Write(no.Valor + " ");
        PreOrdem(no.Esquerda);
        PreOrdem(no.Direita);
    }

    public void ImprimirFatores()
    {
        Console.WriteLine("Fatores de balanceamento:");
        ImprimirFatores(raiz);
    }

    private void ImprimirFatores(NoAVL no)
    {
        if (no == null) return;
        Console.WriteLine("No {0}: Fator de balanceamento {1}", no.Valor, FatorBalanceamento(no));
        ImprimirFatores(no.Esquerda);
        ImprimirFatores(no.Direita);
    }

    public void ImprimirAltura()
    {
        Console.WriteLine("Altura da arvore: " + (raiz == null ? 0 : raiz.Altura - 1));
    }

    private void AtualizarAltura(NoAVL no)
    {
        int altEsq = no.Esquerda?.Altura ?? 0;
        int altDir = no.Direita?.Altura ?? 0;
        no.Altura = Math.Max(altEsq, altDir) + 1;
    }

    private int FatorBalanceamento(NoAVL no)
    {
        int altEsq = no.Esquerda?.Altura ?? 0;
        int altDir = no.Direita?.Altura ?? 0;
        return altEsq - altDir;
    }

    private NoAVL Balancear(NoAVL no)
    {
        int fb = FatorBalanceamento(no);
        if (fb > 1)
        {
            if (FatorBalanceamento(no.Esquerda) < 0)
                no.Esquerda = RotacionarEsquerda(no.Esquerda);
            return RotacionarDireita(no);
        }
        if (fb < -1)
        {
            if (FatorBalanceamento(no.Direita) > 0)
                no.Direita = RotacionarDireita(no.Direita);
            return RotacionarEsquerda(no);
        }
        return no;
    }

    private NoAVL RotacionarDireita(NoAVL y)
    {
        NoAVL x = y.Esquerda;
        NoAVL T2 = x.Direita;
        x.Direita = y;
        y.Esquerda = T2;
        AtualizarAltura(y);
        AtualizarAltura(x);
        return x;
    }

    private NoAVL RotacionarEsquerda(NoAVL x)
    {
        NoAVL y = x.Direita;
        NoAVL T2 = y.Esquerda;
        y.Esquerda = x;
        x.Direita = T2;
        AtualizarAltura(x);
        AtualizarAltura(y);
        return y;
    }
}

class Program
{
    static void Main(string[] args)
    {
        ArvoreAVL arvore = new ArvoreAVL();
        foreach (var linha in File.ReadAllLines("entrada.txt"))
        {
            string[] partes = linha.Split();
            if (partes.Length == 0) continue;
            switch (partes[0])
            {
                case "I": arvore.Inserir(int.Parse(partes[1])); break;
                case "R": arvore.Remover(int.Parse(partes[1])); break;
                case "B": arvore.Buscar(int.Parse(partes[1])); break;
                case "P": arvore.ImprimirPreOrdem(); break;
                case "F": arvore.ImprimirFatores(); break;
                case "H": arvore.ImprimirAltura(); break;
            }
        }
    }
}