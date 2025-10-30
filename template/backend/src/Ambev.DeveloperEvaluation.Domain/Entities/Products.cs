namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class Product
{
    public Guid Id { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public string Description { get; private set; } = string.Empty;

    public decimal Price { get; private set; }

    public int StockQuantity { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime? UpdatedAt { get; private set; }

    private Product() { }

    public Product(string name, string description, decimal price, int stockQuantity)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        Price = price;
        StockQuantity = stockQuantity;
        CreatedAt = DateTime.UtcNow;
    }

    public static Product Create(string name, string description, decimal price, int stockQuantity)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("O nome do produto não pode estar vazio", nameof(name));

        if (price < 0)
            throw new ArgumentException("O preço não pode ser negativo", nameof(price));

        if (stockQuantity < 0)
            throw new ArgumentException("A quantidade em estoque não pode ser negativa", nameof(stockQuantity));

        return new Product(name, description, price, stockQuantity);
    }

    public void Update(string? name, string? description, decimal? price, int? stockQuantity)
    {
        
        if (!string.IsNullOrWhiteSpace(name))
            Name = name;

        if (!string.IsNullOrWhiteSpace(description))
            Description = description;

        if (price.HasValue)
        {
            if (price.Value < 0)
                throw new ArgumentException("O preço não pode ser negativo", nameof(price));
            Price = price.Value;
        }

        if (stockQuantity.HasValue)
        {
            if (stockQuantity.Value < 0)
                throw new ArgumentException("A quantidade em estoque não pode ser negativa", nameof(stockQuantity));
            StockQuantity = stockQuantity.Value;
        }

        UpdatedAt = DateTime.UtcNow;
    }
}