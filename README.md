[![Chat on Gitter](https://img.shields.io/gitter/room/fody/fody.svg?style=flat&max-age=86400)](https://gitter.im/Fody/Fody)
[![NuGet Status](http://img.shields.io/nuget/v/Publicize.Fody.svg?style=flat&max-age=86400)](https://www.nuget.org/packages/Publicize.Fody/)

Converts internal and private members to public and adds [EditorBrowsable(EditorBrowsableState.Advanced)].


## This is an add-in for [Fody](https://github.com/Fody/Fody/) 

![Icon](https://raw.githubusercontent.com/Fody/Publicize/master/package_icon.png)

Converts non-public members to public hidden members

[Introduction to Fody](http://github.com/Fody/Fody/wiki/SampleUsage)


## Usage

See also [Fody usage](https://github.com/Fody/Fody#usage).


### NuGet installation

Install the [Publicize.Fody NuGet package](https://nuget.org/packages/Publicize.Fody/) and update the [Fody NuGet package](https://nuget.org/packages/Fody/):

```powershell
PM> Install-Package Fody
PM> Install-Package Publicize.Fody
```

The `Install-Package Fody` is required since NuGet always defaults to the oldest, and most buggy, version of any dependency.


### Add to FodyWeavers.xml

Add `<Publicize/>` to [FodyWeavers.xml](https://github.com/Fody/Fody#add-fodyweaversxml)

```xml
<?xml version="1.0" encoding="utf-8" ?>
<Weavers>
  <Publicize />
</Weavers>
```

In order to mark public compiler generated types such as, closure objects generated from lambda expressions. Add the `IncludeCompilerGenerated` attribute to the `Publicize` config element.
```xml
<?xml version="1.0" encoding="utf-8" ?>
<Weavers>
  <Publicize IncludeCompilerGenerated="true" />
</Weavers>
```


## Icon

<a href="http://thenounproject.com/noun/hide/#icon-No8013" target="_blank">Hide</a> designed by <a href="http://thenounproject.com/Luis" target="_blank">Luis Prado</a> from The Noun Project