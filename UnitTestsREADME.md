# Xunit ve Moq Temel Kavramları

## Mocked Object Üretme
Moq kütüphanesi, bağımlılıkları izole etmek ve testlerin belirli birimlere odaklanmasını sağlamak için nesneleri "mock"lama imkanı tanır. Mocked object, gerçek nesnelerin davranışlarını simüle ederek, bağımlı olduğunuz sınıf veya servislerin test edilecek birimle ilişkisini kontrol altında tutar.

### Örnek Kullanım:
```csharp
using Moq;

// Bir arayüz için mock nesnesi oluşturma
var mockRepository = new Mock<IRepository>();

// Metot davranışlarını ayarlama
mockRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
              .ReturnsAsync(new Product { Id = 1, Name = "Test Product" });

// Mock nesnesini kullanma
var result = await mockRepository.Object.GetByIdAsync(1);
Assert.Equal("Test Product", result.Name);
```
### Sık Kullanılan Moq Fonksiyonları:
- `Setup`: Mocked nesnenin metot veya özellik davranışını tanımlar.
- `Returns`: Bir metot çağrıldığında döndürülecek değeri ayarlar.
- `It.IsAny<T>()`: Herhangi bir türden herhangi bir değeri eşleştirir.
- `Verify`: Mocked bir metot veya özelliğin çağrılıp çağrılmadığını doğrular.

## Assert İşlemleri
Assert işlemleri, test edilen birimin davranışını doğrulamak için kullanılır. Xunit, bir dizi Assert metodu sunar.

### Sık Kullanılan Assert Fonksiyonları:
- `Assert.Equal(expected, actual)`: Beklenen ve gerçek değerlerin eşit olduğunu doğrular.
- `Assert.NotEqual(expected, actual)`: Beklenen ve gerçek değerlerin eşit olmadığını doğrular.
- `Assert.Null(object)`: Belirtilen nesnenin null olduğunu doğrular.
- `Assert.NotNull(object)`: Belirtilen nesnenin null olmadığını doğrular.
- `Assert.Throws<TException>(action)`: Belirtilen eylemin beklenen bir istisna fırlattığını doğrular.

### Örnek Kullanım:
```csharp
var result = new Product { Id = 1, Name = "Test Product" };

Assert.NotNull(result);
Assert.Equal("Test Product", result.Name);
```

## Fact ve Theory
Xunit, test metotlarını belirlemek için `Fact` ve `Theory` özniteliklerini kullanır.

### Fact
`Fact`, parametre almayan, belirli bir senaryoyu test etmek için kullanılan bağımsız bir test metodunu ifade eder.

### Örnek:
```csharp
[Fact]
public void Add_ShouldReturnCorrectSum()
{
    // Arrange
    var calculator = new Calculator();

    // Act
    var result = calculator.Add(2, 3);

    // Assert
    Assert.Equal(5, result);
}
```

### Theory
`Theory`, aynı testin farklı veri setleriyle çalıştırılmasını sağlar. Test metoduna parametreler geçirilebilir.

### Örnek:
```csharp
[Theory]
[InlineData(2, 3, 5)]
[InlineData(1, 1, 2)]
public void Add_ShouldReturnCorrectSum(int a, int b, int expectedSum)
{
    // Arrange
    var calculator = new Calculator();

    // Act
    var result = calculator.Add(a, b);

    // Assert
    Assert.Equal(expectedSum, result);
}
```



