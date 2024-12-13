using System.Numerics;

namespace AdventOfCode;

public readonly struct Fraction
{
    private readonly BigInteger numerator;
    private readonly BigInteger denominator;

    public Fraction(BigInteger numerator, BigInteger denominator)
    {
        if (denominator < 0)
        {
            denominator *= -1;
            numerator *= -1;
        }

        var divisor = BigInteger.GreatestCommonDivisor(denominator, numerator);
        if (divisor < 0)
            throw new InvalidOperationException();

        this.numerator = numerator / divisor;
        this.denominator = denominator / divisor;

        if (this.denominator < 1)
            throw new InvalidOperationException();
    }

    public static Fraction Parse(string s)
    {
        var split = s.Split('/', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (split.Length == 1)
            return new(split[0].AsLong(), 1);

        if (split.Length != 2)
            throw new ArgumentException($"Cannot parse '{split}'");

        return new(split[0].AsLong(), split[1].AsLong());
    }

    public static implicit operator Fraction(long d) => new(d, 1);
    public static implicit operator Fraction(int d) => new(d, 1);
    public static implicit operator long(Fraction d) => (long)(d.numerator / d.denominator);
    public static implicit operator int(Fraction d) => (int)(d.numerator / d.denominator);
    public static implicit operator double(Fraction d) => (double)d.numerator / (double)d.denominator;

    public static Fraction operator +(Fraction first, Fraction second)
    {
        return new(first.numerator * second.denominator + second.numerator * first.denominator, first.denominator * second.denominator);
    }

    public static Fraction operator -(Fraction first, Fraction second)
    {
        return new(first.numerator * second.denominator - second.numerator * first.denominator, first.denominator * second.denominator);
    }


    public static Fraction operator -(Fraction first)
    {
        return new(-first.numerator, first.denominator);
    }

    public static Fraction operator *(Fraction first, Fraction second)
    {
        return new Fraction(first.numerator * second.numerator, first.denominator * second.denominator);
    }

    public static Fraction operator /(Fraction first, Fraction second)
    {
        return first * second.Invert();
    }

    public Fraction Invert() => new(denominator, numerator);

    public static bool operator ==(Fraction first, Fraction second)
    {
        return first.Equals(second);
    }

    public static bool operator !=(Fraction first, Fraction second)
    {
        return !first.Equals(second);
    }

    public bool Equals(Fraction other)
    {
        return numerator * denominator == other.numerator * other.denominator;
    }

    public override bool Equals(object? obj)
    {
        return obj is Fraction other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(numerator, denominator);
    }

    public override string ToString()
    {
        if (denominator == 1)
            return numerator.ToString();

        return $"{numerator}/{denominator}";
    }

    public bool IsWholeNumber()
    {
        return BigInteger.GreatestCommonDivisor(denominator, numerator) == denominator;
    }
}