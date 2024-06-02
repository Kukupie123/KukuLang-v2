namespace FrontEnd;

public enum PrattPrecedence
{
    Lowest = 1,
    Comparison = 2, // Comparators: ==, !=, <, <=, >, >=
    Sum = 3,
    Product = 4,
    BooleanOr = 5, // ||
    BooleanAnd = 6, // &&

}
