<!-- CREATING NUGET PACKAGE AND CI

[![Build status](link)](link)

[![NuGet](imagemlink)](https://www.nuget.org/packages/ObjectMapper/)

-->

### ObjectMapper

ObjectMapper is another, but yet useful mapper for object-to-object.

### Getting started

Use the annotations, to indicate what properties shall be mapped

```csharp
public class HelloWorld 
{
	[PropertyMap]
	public string Hello { get; set; }

	[PropertyMap]
	public string World { get; set; }
}
```
To do the mapping, use the Mapper class (or the IMapper interface in a dependency injection scenario)

```csharp
var mapper = new Mapper();

var helloWorld = new HelloWorld();

mapper.CreateMapper<DestinyType>()
      	.Map(helloWorld);
```
<!-- NOT YET

### Nuget

You can install ObjectMapper from PM console:

```
PM> Install-Package ObjectMapper
```

-->

### License

ObjectMapper is Copyright &copy; 2017 Filipe da Cunha Vasconcellos and other contributors under the [MIT license](LICENSE.md).