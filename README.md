[![Build status](https://ci.appveyor.com/api/projects/status/6hd9w1wn2506nxn3?svg=true)](https://ci.appveyor.com/project/Filipedguy/objectmapper)
[![NuGet](https://img.shields.io/nuget/v/ObjectMapper.Framework.svg)](https://www.nuget.org/packages/ObjectMapper.Framework/)
[![Codacy Badge](https://api.codacy.com/project/badge/Grade/da850256ddbe4d488e74b400cd73e843)](https://www.codacy.com/app/Filipedguy/ObjectMapper?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=Filipedguy/ObjectMapper&amp;utm_campaign=Badge_Grade)

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

You can use annotations to define target property

```csharp
public class HelloWorld 
{
	[PropertyMap("MyHelloProp")]
	public string Hello { get; set; }

	[PropertyMap("MyWorldProp")]
	public string World { get; set; }
}
```

And you can set custom values for the target object using lambda

```csharp
var mapper = new Mapper();

var helloWorld = new HelloWorld();

mapper.CreateMapper<DestinyType>()
		.CustomMap(x => x.CustomProperty, DateTime.Now)
      	.Map(helloWorld);
```

### Nuget

You can install ObjectMapper from PM console:

```
PM> Install-Package ObjectMapper.Framework
```

### License

ObjectMapper is Copyright &copy; 2017 Filipe da Cunha Vasconcellos and other contributors under the [MIT license](LICENSE.md).
