# Tutorial Steps

### Step 1

Create the following data type:
* Generic Content Grid
  * Property Editor: Grid Layout
  * Keep defaults

Create the following document types:
* Design
  * Phrase Attributes [Tab]
     * Phrase [TextString, Mandatory]
     * Who said this [TextString, Mandatory]
     * Description [RichText editor, Mandatory]
     * Reference Number [TextString, Mandatory]
   * Images [Tab]
     * Thumbnail [Media Picker, Mandatory]
     * Photos [Multiple Media Picker, Mandatory]
   * Pricing [Tab]
     * Price [TextString, Mandtory] 
   * Permissions
     * No child items 

* Design Collection
  * No Properties
  * Permissions
    * Allowed Child Nodes
      * Design

* Shop
  * No Properties
  * Permissions
    * Allowed Child Nodes
      * Design Collection

* Blog Index
  * No Properties
  * Permissions
    *  No child items

* Contact
  * No Properties
  * Permissions
    *  No child items

* Generic Content 
  * Content [Tab]
     * Body Content [Generic Content Grid, Mandatory]
  * Permissions
    * Allowed Child Nodes
      * Generic Content

* Unique Selling Point
  * Content [Tab]
     * Icon [TextString, Mandtory] 
     * Title [TextString, Mandtory] 
     * Description [RichText editor, Mandatory]
     * Button Title [TextString, Mandtory] 
     * Button Link [Content Picker, Mandtory] 
  * Permissions
    *  No child items
  
  * System Folder
    * No Properties
    * Permissions
      * Allowed Child Nodes
        * System Folder
        * Unique Selling Point

* Home
  * Intro [Tab]
     * Title [TextString, Mandatory]
     * Lead [RichText editor, Mandatory]
     * Phrase Samples [Textarea, Mandatory]
     * Carousel Images [Multiple Media Picker, Mandatory]
   * Marketing Messages [Tab]
     * Unique Selling Points [Multinode Treepicker, Mandatory, 3 items only]        
   * Permissions
      * Allow as Root - Yes        
      * Allowed Child Nodes
        * Generic Content
        * Shop
        * Blog Index
        * Contact
        * System Folder
        

### Step 2

* Create and or edit nuget.config in the solution root. Add the following:

```
<?xml version="1.0" encoding="utf-8"?>
<configuration>
    <packageSources>
        <add key="umbracoMyGet" 
             value="https://www.myget.org/F/uaas/api/v3/index.json" 
             protocolVersion="3" />
    </packageSources>
</configuration>
```

### Step 3

Add following nuget packages
* UmbracoCms.Headless.Client (Keep to the version from 20180726)
* UmbracoCms.Headless.Client.Web 

Command line syntax:
* dotnet add package UmbracoCms.Headless.Client -v 0.9.4-*
* dotnet add package UmbracoCms.Headless.Client.Web -v 0.9.0-*

### Step 4

Add the following to appsettings.config

```
{
    "umbracoHeadless": {
        "url": "https://YOUR-PROJECT-URL.s1.umbraco.io",
        "username": "YOUR@USERNAME.com",
        // add password in the user secrets file    
        "password": "YOUR-PASSWORD" 
    }
}
```

In User Secrets, add the following:
```
{
    "umbracoHeadless": {       
        "password": "YOUR-PASSWORD" 
    }
}
```

### Step 5

Edit Startup.cs file:  
* `using Umbraco.Headless.Client;`
* In ConfigureServices add the headless client services: `services.AddUmbracoHeadlessClient(Configuration);`

Edit HomeController.cs file:
* Add `using Umbraco.Headless.Client.Services;`
* Inject the HeadlessService and add create a new controller action
```
public HomeController(HeadlessService headlessService)
{
    this._headlessService = headlessService;
}
 
private readonly HeadlessService _headlessService;

public async Task<IActionResult> Headless()
{
    // Get all content
    var allContent = await _headlessService.Query().GetAll();
    return View(allContent);
}
```

Create a view in /Views/Home/Headless.cshtml:

```
@using Umbraco.Headless.Client.Models
@model IEnumerable<ContentItem>

<h2>All Content items & custom properties in your Umbraco headless CMS</h2>

@foreach(var item in Model){
<h3>@item.Name <small>@item.Id</small></h3>
<ul>
    @foreach (var property in item.Properties.Properties){
         <li><b>@property.Alias:</b>  @property.Value</li>
         }
</ul>
}

```

Go to https://localhost:5001/home/headless  


### Step 6

Create the Home.cs Model

```
using System.Collections.Generic;
using Umbraco.Headless.Client.Models;

namespace TeePhrase.Models
{
    public partial class Home : ContentItem
    {
        public string Title { get; set; }

        public string Lead { get; set; }

        public string PhraseSamples { get; set; }

        public List<MediaItem> CarouselImages { get; set; }

        public List<UniqueSellingPoint> UniqueSellingPoints { get; set; }
    }
}

```

Switch to `Tutorial\Step-6` branch

### Step 7

Edit Startup.cs to bootstrap Umbraco Headless Client
* `Add using Umbraco.Headless.Client.Web;`
* In ConfigureServices add the heeadless client services: `services.AddUmbracoHeadlessWebEngine();`
* In Configure replace the `UseMvc` block with `app.UseUmbracoHeadlessWebEngine();`

Replace HomeController.cs with the following:
```
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TeePhrase.Models;
using Umbraco.Headless.Client.Services;
using Umbraco.Headless.Client.Web;

namespace TeePhrase.Controllers
{
    public class HomeController : DefaultUmbracoController
    {
        public HomeController(UmbracoContext umbracoContext, HeadlessService headlessService) : base(umbracoContext, headlessService)
        {
        }

        public override Task<IActionResult> Index()
        {
            // get the content for the current route
            var content = UmbracoContext.GetContent(false);

            // map the ContentItem to a custom model called Home
            var model = HeadlessService.MapTo<Home>(content);

            // return the view which will be located at
            return Task.FromResult((IActionResult)View(model));
        }

    }
}
```

Replace Views/Home/Index.cshtml with the following:
```
@model Home

<main role="main">
    <div class="container">

        <div class="row my-4">
            <div class="col-lg-4">
                <h1>@Model.Title</h1>
                @Html.Raw(Model.Lead)
                <ul class="list list-magic strong bigger">
                    @foreach (var item in Model.PhraseSamplesList)
                    {
                        <li>@item</li>
                    }
                </ul>
            </div>
            <!-- /.col-md-4 -->

            @await Html.PartialAsync("_CarouselPartial", Model.CarouselImages)
        </div>
        <!-- /.row -->

        @await Html.PartialAsync("_ShopItemsPartial")

        <div class="row my-4">
            @foreach (var usp in Model.UniqueSellingPoints)
            {
                <div class="col-lg-4 col-md-6">
                    <i class="material-icons md-150">@usp.Icon</i>
                    <h2>@usp.Title</h2>
                    @Html.Raw(usp.Description)
                    <p><a class="btn btn-secondary" href="@usp.ButtonLink.Url" role="button">@usp.ButtonTitle</a></p>
                </div>  //.col-lg-4
            }
        </div><!-- /.row -->
    </div>
</main>
```

Replace Views/_ViewImports.cshtml with the following:
```
@using TeePhrase
@using TeePhrase.Models
@using Umbraco.Headless.Client.Models
@using Umbraco.Headless.Client.Services

@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers

```

Replace Views/Shared/_CarouselPartial.cshtml with the following:

```
@model List<MediaItem>

<div class="col-md-8">
    <div id="myCarousel" class="carousel slide" data-ride="carousel">
        <ol class="carousel-indicators">
            @{
                var cssClass = "active";
            }
            @for (int i = 0; i < Model.Count(); i++)
            {
                <li data-target="#myCarousel" data-slide-to="@i" class="@cssClass"></li>
                cssClass = null;
            }
        </ol>
        <div class="carousel-inner">
            @{
                cssClass = "active";
            }
            @foreach (var photo in Model)
            {
                <div class="carousel-item @cssClass">
                    <img class="d-block w-100" src="@photo.Url" alt="@photo.Name">
                </div>
                cssClass = null;
            }
        </div>
        <a class="carousel-control-prev" href="#myCarousel" role="button" data-slide="prev">
            <span class="carousel-control-prev-icon" aria-hidden="true"></span>
            <span class="sr-only">Previous</span>
        </a>
        <a class="carousel-control-next" href="#myCarousel" role="button" data-slide="next">
            <span class="carousel-control-next-icon" aria-hidden="true"></span>
            <span class="sr-only">Next</span>
        </a>
    </div>
</div>
```

### Step 8

Add Views/DefaultUmbraco/Index.cshtml
```
@model object
@Html.DisplayForModel()
```

Replace Views\Shared\_HeaderNavigation.cshtml
```
@inject HeadlessService HeadlessService
@{
    var site = await HeadlessService.GetSite(Model);
    PagedResult<ContentItem> allChildren = await HeadlessService.GetChildren(site);
    var children = allChildren.ToList().Where(x => x.Name != "System");
    var cssClass = Model.Url == "/" ? "active" : null;
}

<header>
    <nav class="navbar navbar-expand-md navbar-dark fixed-top bg-dark">
        <div class="container">
            <a class="navbar-brand" href="/">
                <img src="~/img/logo/shirt.svg" width="30" height="30" class="d-inline-block align-top" alt="">
                Tee-Phrases
            </a>
            <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarCollapse" aria-controls="navbarCollapse" aria-expanded="false" aria-label="Toggle navigation">
                <span class="navbar-toggler-icon"></span>
            </button>
            <div class="collapse navbar-collapse" id="navbarCollapse">
                <ul class="navbar-nav mr-auto">
                    <li class="nav-item @cssClass">
                        <a class="nav-link" href="/">Home <span class="sr-only">(current)</span></a>
                    </li>
                    @foreach (var child in children)
                    {
                        var grandChildren = await HeadlessService.GetChildren(child);
                        cssClass = Model.Url == child.Url || grandChildren.Any(x => Model.Url.StartsWith(x.Url)) ? "active" : null;
                        if (grandChildren.Count > 0)
                        {
                            <li class="nav-item dropdown @cssClass">
                                <a class="nav-link dropdown-toggle" data-toggle="dropdown" href="@child.Url" role="button" aria-haspopup="true" aria-expanded="false">@child.Name</a>
                                <div class="dropdown-menu">
                                    @foreach (var grandChild in grandChildren)
                                    {
                                        cssClass = Model.Url == grandChild.Url || Model.Url.StartsWith(grandChild.Url) ? "active" : null;
                                        <a class="dropdown-item @cssClass" href="@grandChild.Url">@grandChild.Name</a>
                                    }
                                    <div class="dropdown-divider"></div>
                                    @{
                                        cssClass = Model.Url == child.Url ? "active" : null;
                                    }
                                    <a class="dropdown-item @cssClass" href="@child.Url">@child.Name</a>
                                </div>
                            </li>
                        }
                        else
                        {
                            <li class="nav-item @cssClass">
                                <a href="@child.Url" class="nav-link">@child.Name</a>
                            </li>
                        }
                    }
                </ul>
                <form class="form-inline mt-2 mt-md-0">
                    <input class="form-control mr-sm-2" type="text" placeholder="Search" aria-label="Search">
                    <button class="btn btn-outline-success my-2 my-sm-0" type="submit">Search</button>
                </form>
            </div>
        </div>
    </nav>
</header>
```

### Step 9

Switch to `Tutorial\Step-9` branch

### Step 10

Replace code in Views/Shared/_ShopItemsPartial.cshtml

```
@inject HeadlessService HeadlessService
@model Shop
@{
    var designCollections = await HeadlessService.GetChildren<DesignCollection>(Model);
}

<div class="row">
    <div class="col-lg-12 my-4 filter">
        <div class="d-none d-sm-block btn-group no-focus" role="group" aria-label="Filter Designs">
            <button type="button" class="btn btn-outline-secondary active" data-filter="all">All Designs</button>
            @foreach (var singleCollection in designCollections)
            {
                <button type="button" class="btn btn-outline-secondary" data-filter="@singleCollection.Id">@singleCollection.Name</button>
            }
        </div>
        <div class="d-block d-sm-none btn-group-vertical no-focus" role="group" aria-label="Filter Designs">
            <button type="button" class="btn btn-outline-secondary active" data-filter="all">All Designs</button>
            @foreach (var singleCollection in designCollections)
            {
                <button type="button" class="btn btn-outline-secondary" data-filter="@singleCollection.Id">@singleCollection.Name</button>
            }
        </div>
    </div>
</div>
<div class="row portfolio-area">
    @foreach (var singleCollection in designCollections)
    {
        @foreach (var design in await HeadlessService.GetChildren<Design>(singleCollection))
        {
            @Html.DisplayFor(m => design, "DesignTile")
        }
    }
</div>
```

Replace code in Views/Shared/DisplayTemplates/DesignCollection.cshtml
```
@inject HeadlessService HeadlessService
@model DesignCollection

@await Html.PartialAsync("_PageHeader")

<div class="row portfolio-area">
    @foreach (var design in await HeadlessService.GetChildren<Design>(Model))
    {
        @Html.DisplayFor(m => design, "DesignTile")
    }
</div>
```

### Step 11

Replace code in Views/Shared/DisplayTemplates/GenericContent.cshtml 
```
@model GenericContent

@await Html.PartialAsync("_PageHeader")

@if (Model != null && Model.BodyContent.Sections != null)
{
    foreach (var section in Model.BodyContent.Sections)
    {
        foreach (var row in section.Rows)
        {
            <div class="row">
                @foreach (var area in row.Areas)
                {
                    foreach (var control in area.Controls)
                    {
                        var type = control.Editor.Name;
                        var columnClass = $"col-lg-{area.Grid}";
                        var content = control.Editor?.Name == "Headline" ? $"<h3>{control.Value}</h3>" : control.Value;
                        <div class="@columnClass">@Html.Raw(content)</div>
                    }
                }
            </div>
        }
    }
}
```
