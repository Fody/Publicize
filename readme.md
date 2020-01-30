# <img src="/package_icon.png" height="30px"> Publicize.Fody

[![Chat on Gitter](https://img.shields.io/gitter/room/fody/fody.svg)](https://gitter.im/Fody/Fody)
[![NuGet Status](https://img.shields.io/nuget/v/Publicize.Fody.svg)](https://www.nuget.org/packages/Publicize.Fody/)

Converts internal and private members to public and adds [EditorBrowsable(EditorBrowsableState.Advanced)].


### This is an add-in for [Fody](https://github.com/Fody/Home/)

**It is expected that all developers using Fody either [become a Patron on OpenCollective](https://opencollective.com/fody/contribute/patron-3059), or have a [Tidelift Subscription](https://tidelift.com/subscription/pkg/nuget-fody?utm_source=nuget-fody&utm_medium=referral&utm_campaign=enterprise). [See Licensing/Patron FAQ](https://github.com/Fody/Home/blob/master/pages/licensing-patron-faq.md) for more information.**


## Usage

See also [Fody usage](https://github.com/Fody/Home/blob/master/pages/usage.md).


### NuGet installation

Install the [Publicize.Fody NuGet package](https://nuget.org/packages/Publicize.Fody/) and update the [Fody NuGet package](https://nuget.org/packages/Fody/):

```powershell
PM> Install-Package Fody
PM> Install-Package Publicize.Fody
```

The `Install-Package Fody` is required since NuGet always defaults to the oldest, and most buggy, version of any dependency.


### Add to FodyWeavers.xml

Add `<Publicize/>` to [FodyWeavers.xml](https://github.com/Fody/Home/blob/master/pages/usage.md#add-fodyweaversxml)

```xml
<Weavers>
  <Publicize />
</Weavers>
```

In order to mark public compiler generated types such as, closure objects generated from lambda expressions. Add the `IncludeCompilerGenerated` attribute to the `Publicize` config element.
```xml
<Weavers>
  <Publicize IncludeCompilerGenerated="true" />
</Weavers>
```


## Icon

[Hide](https://thenounproject.com/noun/hide/#icon-No8013) designed by [Luis Prado](https://thenounproject.com/Luis) from [The Noun Project](https://thenounproject.com).